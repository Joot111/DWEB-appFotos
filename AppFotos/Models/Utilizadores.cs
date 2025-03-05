using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models
{
    /// <summary>
    /// utilizadores não anónimos da aplicação
    /// </summary>
    public class Utilizadores
    {
        /// <summary>
        /// Identificador do Utilizador
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do Utilizador
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Morada do Utilizador
        /// </summary>
        public string Morada { get; set; }

        /// <summary>
        /// Código Postal da morada do Utilizador
        /// </summary>
        public string CodPostal { get; set; }

        /// <summary>
        /// País da morada do Utilizador
        /// </summary>
        public string Pais { get; set; }
        
        /// <summary>
        /// Número de Identificaçao Fiscal do Utilizador 
        /// </summary>
        public string NIF { get; set; }

        /// <summary>
        /// Número de Telemóvel
        /// </summary>
        public string Telemovel { get; set; }
    }
}
