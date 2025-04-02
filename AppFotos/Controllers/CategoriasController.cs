﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;
using Microsoft.IdentityModel.Tokens;

namespace AppFotos.Controllers
{
    public class CategoriasController : Controller
    {
        /// <summary>
        /// referencia a Base de Dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorias = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            // mostra a View de nome 'Create',
            // que está na pasta 'Categorias'
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // Responde a uma resposta do browser feita em POST
        [ValidateAntiForgeryToken] // Proteção contra ataques
        public async Task<IActionResult> Create([Bind("Nome")] Categorias novaCategoria)
        {
            // Tarefas
            // - ajustar o nome das variáveis
            // - ajustar os anotadores, neste caso em concreto,
            //   eliminar o ID do 'Bind'
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(novaCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Aconteceu um erro na gravação de dados.");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(novaCategoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            // Se chego aqui, há categorias para editar

            // guardar os dados do objeto que vai ser enviado para o browser pelo Utilizador
            HttpContext.Session.SetInt32("CategoriaID", categoria.Id);
            HttpContext.Session.SetString("Acao", "Categorias/Edit");

            // mostro a View
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [Bind("Id,Nome")] Categorias categoriaAlterada)
        {
            // A anotação FromRoute lê o valor do ID da rota
            // se houver alterações à 'rota', alterações indevidas
            if (id != categoriaAlterada.Id)
            {
                return RedirectToAction("Index");
            }

            // será que os dados que recebi,
            // são correspondentes ao objeto que enviei para o browser?
            var categoriaID = HttpContext.Session.GetInt32("CategoriaID");
            var acao = HttpContext.Session.GetString("Acao");
            // demorei muito tempo => timeout
            if (categoriaID == null || acao.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Demorou muito tempo. Tem de ser mais rápido. " +
                    "Já não consegue alterar a 'categoria'. Tem de reiniciar o processo.");
                return View(categoriaAlterada);
            }

            // Houve adulteração dos dados
            if (categoriaID != categoriaAlterada.Id || acao != "Categorias/Edit")
            {
                // O utilizador está a tentar alterar outro objeto diferente do que recebeu
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaAlterada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriasExists(categoriaAlterada.Id))
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
            return View(categoriaAlterada);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            // guardar os dados do objeto que vai ser enviado para o browser pelo Utilizador
            HttpContext.Session.SetInt32("CategoriaID", categoria.Id);
            HttpContext.Session.SetString("Acao", "Categorias/Delete");

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            // será que os dados que recebi,
            // são correspondentes ao objeto que enviei para o browser?
            var categoriaID = HttpContext.Session.GetInt32("CategoriaID");
            var acao = HttpContext.Session.GetString("Acao");
            // demorei muito tempo => timeout
            if (categoriaID == null || acao.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Demorou muito tempo. Tem de ser mais rápido. " +
                    "Já não consegue alterar a 'categoria'. Tem de reiniciar o processo.");
                return View(categoria);
            }

            // Houve adulteração dos dados
            if (categoriaID != categoria.Id || acao != "Categorias/Delete")
            {
                // O utilizador está a tentar alterar outro objeto diferente do que recebeu
                return RedirectToAction("Index");
            }

            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriasExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
