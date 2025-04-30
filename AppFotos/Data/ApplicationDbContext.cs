﻿using AppFotos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppFotos.Data
{
    /// <summary>
    /// Esta classe representa a Base de Dados associada ao projeto.
    /// Se houve mais bases de dados, irão haver tantas classes 
    /// deste tipo, quantas as necessárias.
    /// 
    /// Esta classe é equivalente a: CREATE SCHEMA / CREATE DATABASE no SQL
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // especificar as tabelas associadas à BD
        /// <summary>
        /// Tabela Categorias
        /// </summary>
        public DbSet<Categorias> Categorias { get; set; }
        /// <summary>
        /// Tabela Utilizadores
        /// </summary>
        public DbSet<Utilizadores> Utilizadores { get; set; }
        /// <summary>
        /// Tabela Fotografias
        /// </summary>
        public DbSet<Fotografias> Fotografias { get; set; }
        /// <summary>
        /// Tabela Compras
        /// </summary>
        public DbSet<Compras> Compras { get; set; }
        /// <summary>
        /// Tabela Gostos
        /// </summary>
        public DbSet<Gostos> Gostos { get; set; }

        // este código deve ser colocado dentro da classe 'ApplicationDbContext'
        // serve para adicionar às Migrações um conjunto de registos que devem estar sempre presentes na 
        // base de dados do projeto, desde o seu início.
        // Esta técnica NÃO É ADEQUADA para a criação de dados de teste!!!

        /// <summary>
        /// este método é executado imediatamente antes 
        /// da criação da Base de Dados.
        /// É utilizado para adicionar as últimas instruções
        /// à criação do modelo
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 'importa' todo o comportamento do método, 
            // aquando da sua definição na SuperClasse
            base.OnModelCreating(modelBuilder);

            // criar os perfis de utilizador da nossa app
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "a", Name = "Admin", NormalizedName = "ADMIN" });

            // criar um utilizador para funcionar como ADMIN
            // função para codificar a password
            var hasher = new PasswordHasher<IdentityUser>();
            // criação do utilizador
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "admin",
                    UserName = "admin@mail.pt",
                    NormalizedUserName = "ADMIN@MAIL.PT",
                    Email = "admin@mail.pt",
                    NormalizedEmail = "ADMIN@MAIL.PT",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PasswordHash = hasher.HashPassword(null, "Aa0_aa")
                }
            );
            // Associar este utilizador à role ADMIN
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "admin", RoleId = "a" });


            /* ------------------------------------------------------------------ */

            // eventualmente, adicionar mais dados à BD
            // SÓ DEVERÁ SER ADICIONADA DESTA FORMA DADOS QUE SE DESEJAM MANTER
            // APÓS O FIM DO DESENVOLVIMENTO
            // dados de teste, NUNCA DEVEM aqui ser colocados
            // neste exercício, podemos adicionar algumas Categorias
            modelBuilder.Entity<Categorias>().HasData(
                new Categorias { Id = 1, Nome = "Flores" },
                new Categorias { Id = 2, Nome = "Animais" },
                new Categorias { Id = 3, Nome = "Mar" },
                new Categorias { Id = 4, Nome = "Pessoas" },
                new Categorias { Id = 5, Nome = "Casas" }
            );
            /* ------------------------------------------------------------------ */
        }
    }
}
