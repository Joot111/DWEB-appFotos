using System.ComponentModel.DataAnnotations;

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
        public string Titulo { get; set; }

        /// <summary>
        /// Descrição da Fotografia
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do ficheiro da fotografia 
        /// no disco rígido do servidor
        /// </summary>
        public string Ficheiro { get; set; }

        /// <summary>
        /// Data em que a fotografia foi tirada
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Preço de venda da fotografia
        /// </summary>
        public decimal Preco { get; set; }
    }
}
