using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1.ExercicioEFCoreCodeFirst.PL
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Gross { get; set; }
        public double Rating { get; set; }

        public int GenreID { get; set; }
        public Genre Genre { get; set; }
    }
}
