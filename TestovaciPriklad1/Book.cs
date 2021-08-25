using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestovaciPriklad1
{
    /// <summary>
    /// Trieda pre objekt s informáciami o knihe.
    /// </summary>
    class Book
    {
        /// <summary>
        /// Id knihy
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Nazov knihy
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Autor knihy
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Informacie o zapozicani hnihy
        /// </summary>
        public Borrowed borrowed { get; set; }
        /// <summary>
        /// Objekt pre uloženie inforácii o príslušnej knihe.
        /// </summary>
        /// <param name="id">ID knihy</param>
        /// <param name="Name">Názov knihy</param>
        /// <param name="Author">Autor knihy</param>
        /// <param name="borrowed">Informácie o človekovi čo si knihu zapožičal</param>
        public Book(int id, string Name, string Author, Borrowed borrowed)
        {
            this.id = id;
            this.Name = Name;
            this.Author = Author;
            this.borrowed = borrowed;
        }
    }
}
