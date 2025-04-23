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
        [Display(Name = "Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} não pode ser nulo.")]
        public string Nome { get; set; } = "";

        /// <summary>
        /// Morada do Utilizador
        /// </summary>
        [Display(Name = "Morada")]
        [StringLength(50)]
        public string Morada { get; set; } = "";

        /// <summary>
        /// Código Postal da morada do Utilizador
        /// </summary>
        [Display(Name = "Código Postal")]
        [StringLength(50)]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-Za-z ]+",
            ErrorMessage = "No {0} só são aceites algarismos e letras inglesas.")]
        public string? CodPostal { get; set; } 

        /// <summary>
        /// País da morada do Utilizador
        /// </summary>
        [Display(Name = "País")]
        [StringLength(50)]
        public string Pais { get; set; } = String.Empty;

        /// <summary>
        /// Número de Identificaçao Fiscal do Utilizador 
        /// </summary>
        [StringLength(9)]
        [RegularExpression("[1-9][0-9]{8}", ErrorMessage = "Deve escrever apenas 9 digitos no {0}.")]
        [Required(ErrorMessage = "O {0} não pode ser nulo.")]
        public string NIF { get; set; } = String.Empty;

        /// <summary>
        /// Número de Telemóvel
        /// </summary>
        [Display(Name = "Telemóvel")]
        // Pensado como App nacional, se for Internacional é necessário muito mais
        [StringLength(18)]
        [RegularExpression("(([+]|00)[0-9]{1,5})?[1-9][0-9]{5,10}", 
            ErrorMessage = "Escreva um nº de telefone. Pode adicionar o indicativo do país.")] // '-' '_'
        public string? Telemovel { get; set; }


        /// <summary>
        /// Este atributo servirá para fazer a 'ponte'
        /// entre a tabela dos Utilizadores e 
        /// a tabela da Autenticação da Microsoft Identity
        /// </summary>
        [StringLength(50)]
        public string UserName { get; set; } = String.Empty;

        /*******************************
         * Definição dos Relacionamentos
         *******************************
         */

        // Relacionamentos M - N

        /// <summary>
        /// Lista das fotografias que são propriedade do utilizador
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; } = [];

        /// <summary>
        /// Lista dos 'gostos' de fotografias do utilizador
        /// </summary>
        public ICollection<Gostos> ListaGostos { get; set; } = [];

        /// <summary>
        /// Lista das fotografias compradas pelo utilizador
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; } = [];
    }
}
