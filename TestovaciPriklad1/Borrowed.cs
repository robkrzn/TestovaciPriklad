using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestovaciPriklad1
{
    /// <summary>
    /// Trieda pre objekt s informáciami o zapožičaní knihy.
    /// </summary>
    class Borrowed
    {
        /// <summary>
        /// Meno človeka čo si zapožical knihu
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Priezvisko človeka čo si zapožičal knihu
        /// </summary>
        public string LastName;
        /// <summary>
        /// Datum zapožičania knihy
        /// </summary>
        //public DateTime From;
        public String From;

        /// <summary>
        /// Objekt pre uloženie informácii o zapožičání knihy
        /// </summary>
        /// <param name="FirstName">Meno človeka čo si knihu požičal</param>
        /// <param name="LastName">Priezvisko človeka čo si knihu požičal</param>
        /// <param name="From">Dátum kedy si knihu požičal</param>
        public Borrowed(string FirstName, string LastName, String From)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.From = From;
        }


    }
}
