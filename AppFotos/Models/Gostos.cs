using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models
{
    /// <summary>
    /// regista os 'gostos' que um utilizador da app tem
    /// pelas fotografias existentes
    /// </summary>
    [PrimaryKey(nameof(UtilizadorFK), nameof(FotografiaFK))] 
    public class Gostos
    {
        /// <summary>
        /// Data em que um utilizador marcou 
        /// que 'gosta' de uma fotografia
        /// </summary>
        public DateTime Data { get; set; }

        /*******************************
         * Definição dos Relacionamentos
         *******************************
         */

        // Relacionamentos 1 - N

        /// <summary>
        /// FK para referenciar o Utilizador que gosta da fotografia
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int UtilizadorFK { get; set; }

        /// <summary>
        /// FK para referenciar o Utilizador que gosta da fotografia
        /// </summary>
        public Utilizadores Utilizador { get; set; }

        /// <summary>
        /// FK para referenciar a fotografia que o utilizador gosto
        /// </summary>
        [ForeignKey(nameof(Fotografia))]
        public int FotografiaFK { get; set; }

        /// <summary>
        /// FK para referenciar a fotografia que o utilizador gosto
        /// </summary>
        public Fotografias Fotografia { get; set; }
    }
}
