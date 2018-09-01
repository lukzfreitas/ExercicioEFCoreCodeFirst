using System;
using System.Collections.Generic;
using System.Text;

namespace ExercicioEFCoreCodeFirst
{
    public class ActorMovie
    {
        public int ActorMovieID { get; set; }
        public string Character { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
