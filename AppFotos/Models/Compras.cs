using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models
{
    /// <summary>
    /// compras efetuadas na aplicação
    /// </summary>
    public class Compras
    {
        /// <summary>
        /// Identificador da compra
        /// </summary>
        [Key] // PK, int, autonumber
        public int Id { get; set; }

        /// <summary>
        /// Data da compra
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Estado da compra. 
        /// Representa um conjunto de valores pre-determinados 
        /// que representa a evolução da 'compra' 
        /// </summary>
        public Estados Estado { get; set; }

        /*******************************
         * Definição dos Relacionamentos
         *******************************
         */

        // Relacionamentos 1 - N

        /// <summary>
        /// FK para referenciar o Comprador da fotografia
        /// </summary>
        [ForeignKey(nameof(Comprador))]
        public int CompradorFK { get; set; }

        /// <summary>
        /// FK para referenciar o Comprador da fotografia
        /// </summary>
        // Anotação -> []
        public Utilizadores Comprador { get; set; } = null!;

        /// <summary>
        /// FK para referenciar as fotografias compradas
        /// </summary>
        public ICollection<Fotografias> ListaFotografiasCompradas { get; set; } = [];

    }

    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
    /// <summary>
    /// Estados associados a uma 'compra'
    /// </summary>
    public enum Estados
    {
        Pendente,
        Pago,
        Enviada,
        Entregue,
        Terminada
    }
}
