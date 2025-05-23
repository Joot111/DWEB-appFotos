﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;
using System.Globalization;

namespace AppFotos.Controllers
{
    public class FotografiasController : Controller
    {
        /// <summary>
        ///  referência à Base de Dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// objeto que contêm todas as caracterísiticas do servidor
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FotografiasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Fotografias
        public async Task<IActionResult> Index()
        {
            /* interrogação à BD feita em LINQ <==> SQL
             * SELECT *
             * FROM Fotografias f INNER JOIN Categorias c ON f.CatagoriaFK = c.Id 
             *                    INNER JOIN Utilizadores u ON f.DonoFK = u.Id 
             */
            // l de lista pequeno porque é uma variável local
            var listaFotografias = _context.Fotografias.Include(f => f.Categoria).Include(f => f.Dono);
            
            return View(await listaFotografias.ToListAsync());
        }

        // GET: Fotografias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /* interrogação à BD feita em LINQ <==> SQL
             * SELECT *
             * FROM Fotografias f INNER JOIN Categorias c ON f.CatagoriaFK = c.Id 
             *                    INNER JOIN Utilizadores u ON f.DonoFK = u.Id 
             * Where f.Id = id
             */
            var fotografia = await _context.Fotografias
                .Include(f => f.Categoria)
                .Include(f => f.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotografia == null)
            {
                return NotFound();
            }

            return View(fotografia);
        }

        // GET: Fotografias/Create
        public IActionResult Create()
        {
            // este é o primeiro método a ser chamado quando se pretende adicionar uma Fotografia

            // estes contentores servem para levar os dados das 'dropdows' para as views
            // SELECT *
            // FROM Categorias c
            // ORDER BY c.Nome
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c=>c.Nome), "Id", "Nome");
            // SELECT *
            // FROM Utilizadores u
            // ORDER BY u.Nome
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u=>u.Nome), "Id", "Nome");
            return View();
        }

        // POST: Fotografias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Ficheiro,Data,PrecoAux,CategoriaFK,DonoFK")] Fotografias fotografia,
            IFormFile imagemFoto)
        {
            // vars.auxiliares
            bool haErro = false;
            string nomeImagem = "";

            // Avaliar se há Categoria e Utilizador
            if(fotografia.CategoriaFK<= 0)
            {
                // Erro. Não foi escolhida a Categoria
                haErro = true;
                // crio msg de erro
                ModelState.AddModelError("", "Tem de escolher uma categoria");
            }

            if (fotografia.DonoFK <= 0)
            {
                // Erro. Não foi escolhido o Dono
                haErro = true;
                // crio msg de erro
                ModelState.AddModelError("", "Tem de escolher uma categoria");
            }

            /* Avaliar o ficheiro fornecido
             * - há ficheiro?
             *  - se não existir ficheiro, gerar msg erro e devolver à view o controlo
             *  - se há,
             *      - será uma imagem?
             *          - se não for, gerar msg erro e devolver à view o controlo
             *          - é,
             *              - determinar o novo nome do ficheiro
             *              - guardar na BD o nome do ficheiro
             *              - guardaro ficheiro no disco rígido do servidor
             */

            if (imagemFoto == null)
            {
                // não há imagem
                haErro = true;
                // crio msg de erro
                ModelState.AddModelError("", "Tem de submeter uma Fotografia");
            }
            else
            {
                // há ficheiro. Mas, é uma imagem?
                if(imagemFoto.ContentType != "image/jpeg" && imagemFoto.ContentType != "image/png")
                {
                    // ! (A==b || A==c) <=> (A!=b && A!=c)
                    // não há ficheiro
                    haErro = true;
                    // crio msg de erro
                    ModelState.AddModelError("", "Tem de submeter uma Fotografia");
                }
                else
                {
                    // há imagem,
                    // vamos processá-la
                    //*********************
                    // Novo nome para o ficheiro
                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString(); 
                    string extensao = Path.GetExtension(imagemFoto.FileName).ToLowerInvariant();
                    nomeImagem += extensao;

                    // guardar este nome na BD
                    fotografia.Ficheiro = nomeImagem;

                }
            }

            // Desligar a validação do atributo Ficheiro 
            ModelState.Remove("Ficheiro");

            // Avalia se os dados estão de acordo com o Model
            if (ModelState.IsValid && !haErro)
            {
                // transferir o valor do PrecoAux para o Preco
                // há necessidade de tratar a questão do . no meio da string
                // há necessidade de garantir que a conversão seja feita segunda uma 'cultura pt-pt'
                fotografia.Preco = Convert.ToDecimal(fotografia.PrecoAux.Replace('.',','), new CultureInfo("pt-PT"));

                // adicionar os dados da nova fotografia na BD
                _context.Add(fotografia);
                await _context.SaveChangesAsync();

                // *********************************************
                // guardar o ficheiro no disco rígido
                // *********************************************
                // determinar o local de armazenagem da imagem
                string localizacaoImagem = _webHostEnvironment.WebRootPath;
                localizacaoImagem = Path.Combine(localizacaoImagem,"imagens");
                if (!Directory.Exists(localizacaoImagem))
                {
                    Directory.CreateDirectory(localizacaoImagem);
                }
                // gerar o caminho completo para a imagem
                nomeImagem = Path.Combine(localizacaoImagem, nomeImagem);
                // agora, temos as condições para guardar a imagem no ficheiro
                using var stream = new FileStream(
                    nomeImagem, FileMode.Create
                    );
                await imagemFoto.CopyToAsync( stream );


                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c=>c.Nome), "Id", "Nome", fotografia.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u=>u.Nome), "Id", "Nome", fotografia.DonoFK);
            
            // Se chego aqui é pq correu mal...
            return View(fotografia);
        }

        // GET: Fotografias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotografia = await _context.Fotografias.FindAsync(id);
            if (fotografia == null)
            {
                return NotFound();
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c => c.Nome), "Id", "Nome", fotografia.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u => u.Nome), "Id", "Nome", fotografia.DonoFK);
            return View(fotografia);
        }

        // POST: Fotografias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Ficheiro,Data,Preco,CategoriaFK,DonoFK")] Fotografias fotografia,
            IFormFile imagemFoto)
        {
            if (id != fotografia.Id)
            {
                return NotFound();
            }

            // Na acção de Editar temos de fazes o mesmo tratamento dos dados
            // como foi feito na acção Create
            // Só há uma alteração!
            // Se o utilizador não quiser alterar a imagem,
            // NÃO PODE ser apagada da BD

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fotografia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FotografiasExists(fotografia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c => c.Nome), "Id", "Nome", fotografia.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u => u.Nome), "Id", "Nome", fotografia.DonoFK);
            return View(fotografia);
        }

        // GET: Fotografias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotografias = await _context.Fotografias
                .Include(f => f.Categoria)
                .Include(f => f.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotografias == null)
            {
                return NotFound();
            }

            return View(fotografias);
        }

        // POST: Fotografias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fotografia = await _context.Fotografias.FindAsync(id);
            if (fotografia != null)
            {
                // remover a imagem da BD
                _context.Fotografias.Remove(fotografia);
                // NÃO ESQUECER:
                // Apagar a imagem do disco rígido
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FotografiasExists(int id)
        {
            return _context.Fotografias.Any(e => e.Id == id);
        }
    }
}
