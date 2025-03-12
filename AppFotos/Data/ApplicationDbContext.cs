using AppFotos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppFotos.Data
{
    /// <summary>
    /// Esta classe representa a Base de Dados associada ao projeto
    /// Se houve mais base de dados, irão haver tantas classes 
    /// deste tipo, quantas as necessárias
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
    }

}
