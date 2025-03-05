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

        /*******************************
         * Definição dos Relacionamentos
         * *****************************
         */

        // Relacionamentos 1 - N

        /// <summary>
        /// FK para referenciar a categoria da fotografia
        /// </summary>
        [ForeignKey(nameof(Categoria))]
        public int CategoriaFK { get; set; }

        /// <summary>
        /// FK para referenciar a categoria da fotografia
        /// </summary>
        public Categorias Categoria { get; set; }

        /// <summary>
        /// FK para referenciar o Dono da fotografia
        /// </summary>
        [ForeignKey(nameof(Dono))]
        public int DonoFK { get; set; }

        /// <summary>
        /// FK para referenciar o Dono da fotografia
        /// </summary>
        public Utilizadores Dono { get; set; }

        // Relacionamentos M - N

        /// <summary>
        /// Lista de 'gostos' de uma fotografia
        /// </summary>
        public ICollection<Gostos> ListaGostos { get; set; }
    
        /// <summary>
        /// Lista de 'compras' que compraram a fotografia
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; }  
    }
}
