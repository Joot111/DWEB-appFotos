using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models
{
    /// <summary>
    /// fotografias disponíveis na aplicação
    /// </summary>
    public class Fotografias
    {
        /// <summary>
        /// Identificador de Fotografia
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Título da Fotografia
        /// </summary>
        public string Titulo { get; set; } = "";

        /// <summary>
        /// Descrição da Fotografia
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Nome do ficheiro da fotografia 
        /// no disco rígido do servidor
        /// </summary>
        public string Ficheiro { get; set; } = null!;

        /// <summary>
        /// Data em que a fotografia foi tirada
        /// </summary>
        [Display(Name = "Data")]
        [DataType(DataType.Date)] // tranforma o atributo, na BD, em 'Date'
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Preço de venda da fotografia
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// Atributo auxiliar para recolher o valor do Preço da Fotografia
        /// Será usado no 'Create' e no 'Edit
        /// </summary>
        [NotMapped]
        [Display(Name = "Preço")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(10)]
        [RegularExpression("[0-9]{1,7}([,.][0-9]{1,2})?", 
            ErrorMessage = "Só são aceites algarismos. Pode escrever duas casas decimais, separadas por . ou ,")]
        public string PrecoAux { get; set; } = string.Empty;

        /*******************************
         * Definição dos Relacionamentos
         * *****************************
         */

        // Relacionamentos 1 - N

        /// <summary>
        /// FK para referenciar a categoria da fotografia
        /// </summary>
        [ForeignKey(nameof(Categoria))]
        [Display(Name = "Categoria")]
        public int CategoriaFK { get; set; }

        /// <summary>
        /// FK para referenciar a categoria da fotografia
        /// </summary>
        public Categorias Categoria { get; set; } = null!;

        /// <summary>
        /// FK para referenciar o Dono da fotografia
        /// </summary>
        [ForeignKey(nameof(Dono))]
        [Display(Name = "Dono")]
        public int DonoFK { get; set; }

        /// <summary>
        /// FK para referenciar o Dono da fotografia
        /// </summary>
        // []
        [ValidateNever]
        public Utilizadores Dono { get; set; } = null!;

        // Relacionamentos M - N

        /// <summary>
        /// Lista de 'gostos' de uma fotografia
        /// </summary>
        public ICollection<Gostos> ListaGostos { get; set; } = [];

        /// <summary>
        /// Lista de 'compras' que compraram a fotografia
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; } = [];
    }
}
