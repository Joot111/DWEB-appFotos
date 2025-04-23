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
        [Key] // anotação -> restrição
        public int Id { get; set; }

        /// <summary>
        /// Nome da categoria associada à fotografia 
        /// </summary>
        [Required(ErrorMessage = "O {0} da Categoria é de preenchimento obrigatório.")]
        [StringLength(20)]
        [Display(Name = "Categoria")]
        public string Nome { get; set; } = ""; //<=> string.Empty;


        /*******************************
         * Definição dos Relacionamentos
         *******************************
         */

        // Relacionamentos 1 - N

        /// <summary>
        /// Lista das fotografias associadas a uma categoria
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; } = []; //<=> new HashSet<Fotografias>();

    }
}
