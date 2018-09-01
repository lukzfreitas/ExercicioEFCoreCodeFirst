using System;
using System.Collections.Generic;
using System.Text;

namespace ExercicioEFCoreCodeFirst
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }

        public ICollection<ActorMovie> ActorsMovies { get; set; }
    }
}
