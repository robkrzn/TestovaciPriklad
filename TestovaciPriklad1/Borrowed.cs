using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestovaciPriklad1
{
    
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

        public Borrowed(string FirstName, string LastName, String From)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.From = From;
        }


    }
}
