using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models
{
    /// <summary>
    /// categorias a que as fotografias podem ser associadas
    /// </summary>
    public class Categorias
    {
        /// <summary>
        /// Identificador da Categoria
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Nome da categoria associada à fotografia 
        /// </summary>
        public string Nome { get; set; }
        
    }
}
