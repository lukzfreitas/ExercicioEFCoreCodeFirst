//using ExercicioEFCoreCodeFirst.PL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExercicioEFCoreCodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MovieContext())
            {
                #region seeding
                if (db.Genres.Count() == 0)
                {
                    Seed(db);
                }
                #endregion
                #region consultas
                Console.WriteLine();
                Console.WriteLine("Todos os gêneros da base de dados (press any key...):");
                Console.ReadKey();
                foreach (Genre genero in db.Genres)
                {
                    Console.WriteLine("{0} \t {1}", genero.GenreID, genero.Name);
                }


                Console.WriteLine();
                Console.WriteLine("Todos os filmes do genero 'Action' (method syntax) (press any key...):");
                Console.ReadKey();
                var filmesAction = db.Movies.Where(m => m.GenreID == 1);
                foreach (Movie filme in filmesAction)
                {
                    Console.WriteLine("\t{0}", filme.Title);
                }


                Console.WriteLine();
                Console.WriteLine("Todos os filmes do genero 'Action' (query syntax) (press any key...):");
                Console.ReadKey();
                var filmesAction2 = from m in db.Movies
                                    where m.GenreID == 1
                                    select m;
                foreach (Movie filme in filmesAction2)
                {
                    Console.WriteLine("\t{0}", filme.Title);
                }

                Console.WriteLine();
                Console.WriteLine("Todos os filmes de cada genero (press any key...):");
                Console.ReadKey();
                var generosFilmes = from g in db.Genres.Include(gen => gen.Movies)
                                    select g;
                var generosFilmes2 = db.Genres.Include(gen => gen.Movies).ToList();
                foreach (var gf in generosFilmes2)
                {
                    Console.WriteLine("Filmes do genero: " + gf.Name);
                    foreach (var f in gf.Movies)
                    {
                        Console.WriteLine("\t{0}", f.Title);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Nomes dos filmes do diretor Quentin Tarantino (press any key...):");
                Console.ReadKey();
                var q1 = from f in db.Movies
                         where f.Director == "Quentin Tarantino"
                         select f.Title;
                var q2 = db.Movies.Where(f => f.Director == "Quentin Tarantino").Select(f => f.Title);
                foreach (String t in q1)
                {
                    Console.WriteLine(t);
                }

                Console.WriteLine();
                Console.WriteLine("Nomes e data dos filmes do diretor Quentin Tarantino (press any key...):");
                Console.ReadKey(); Console.ReadKey();
                var q3 = from f in db.Movies
                         where f.Director == "Quentin Tarantino"
                         select new { f.Title, f.ReleaseDate };
                foreach (var f in q3)
                {
                    Console.WriteLine("{0}\t {1}", f.ReleaseDate.ToShortDateString(), f.Title);
                }

                Console.WriteLine();
                Console.WriteLine("Todos os gêneros ordenados pelo nome (press any key...):");
                Console.ReadKey();
                var q4 = db.Genres.OrderBy(g => g.Name);
                foreach (var genero in q4)
                {
                    Console.WriteLine("{0}\t {1}", genero.Name, genero.Description);
                }

                Console.WriteLine();
                Console.WriteLine("Todos os filmes agrupados pelo anos de lançamento (press any key...):");
                Console.ReadKey();
                var q5 = from f in db.Movies
                         group f by f.ReleaseDate.Year;
                foreach (var ano in q5.OrderByDescending(g => g.Key))
                {
                    Console.WriteLine("Ano: {0}", ano.Key);
                    foreach (var filme in ano)
                    {
                        Console.WriteLine("\t{0:dd/MM}\t {1}", filme.ReleaseDate, filme.Title);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Projeção do faturamento total, quantidade de filmes e avaliação média agrupadas por gênero (press any key...):");
                Console.ReadKey();
                var q6 = from f in db.Movies
                         group f by f.Genre.Name into grpGen
                         select new
                         {
                             Categoria = grpGen.Key,
                             Filmes = grpGen,
                             Faturamento = grpGen.Sum(e => e.Gross),
                             Avaliacao = grpGen.Average(e => e.Rating),
                             Quantidade = grpGen.Count()
                         };
                foreach (var genero in q6)
                {
                    Console.WriteLine("Genero: {0}", genero.Categoria);
                    Console.WriteLine("\tFaturamento total: {0}\n\t Avaliação média: {1}\n\tNumero de filmes: {2}",
                                    genero.Faturamento, genero.Avaliacao, genero.Quantidade);
                }
                #endregion

                #region seeding
                if (db.Actors.Count() == 0)
                {
                    Seed(db);
                }
                #endregion

                #region consultas
                Console.WriteLine();
                Console.WriteLine("Todos os atores da base de dados (press any key...):");
                Console.ReadKey();
                foreach (Actor ator in db.Actors)
                {
                    Console.WriteLine("{0} \t {1}", ator.ActorId, ator.Name);
                }

                //Exercicio 10.a
                Console.WriteLine();
                Console.WriteLine("Lista do elenco do filme Star Wars (press any key...):");
                Console.ReadKey();
                var e1 =
                    from actor in db.Actors
                    join ActorMovie in db.Characters on actor.ActorId equals ActorMovie.ActorId 
                    join Movie in db.Movies on ActorMovie.MovieId equals Movie.ID
                    where Movie.Title == "Star Wars"
                    select actor;
                foreach (var e11 in e1)
                {
                    Console.WriteLine(e11.Name);
                }

                //Exercicio 10.b
                Console.WriteLine();
                Console.WriteLine("Lista dos atores por determinado personagem(press any key...):");
                Console.ReadKey();
                var e2 =
                    from actor in db.Actors
                    join ActorMovie in db.Characters on actor.ActorId equals ActorMovie.ActorId
                    where ActorMovie.Character == "James Bond"
                    select actor;
                foreach (var e21 in e2)
                {
                    Console.WriteLine(e21.Name);
                }

                //Exercicio 10.c
                Console.WriteLine();
                Console.WriteLine("Lista do ator que desempenhou mais vezes um determinado personagem(press any key...):");
                Console.ReadKey();
                //var e3 = from p in cacheProducts
                //         join MR in MasterRenewal on p.Id equals MR.MstSeq into MRRenewal
                //         let MR = MRRenewal.OrderByDescending(r => r.Id).FirstOrDefault()

                var e3 = from actor in db.Actors
                         join actorMovie in db.Characters on actor.ActorId equals actorMovie.ActorId  into result                         
                         let resultActors = result.OrderByDescending(r => r.ActorId).FirstOrDefault()
                         select actor;

                foreach (var e31 in e3)
                {
                    Console.WriteLine(e31.Name);
                }
                #endregion

                #region #operações de crud
                ////Adicionar um novo filme
                //Console.WriteLine("Adicionando um novo filme");
                //db.Movies.Add(new Movie
                //{
                //    Title = "Logan",
                //    Director = "James Mangold",
                //    Rating = 8.5,
                //    ReleaseDate = new DateTime(2017, 03, 24),
                //    GenreID = 1
                //});
                ////Remover o primeiro filme consultado
                //Console.WriteLine("Removendo um filme");
                //var todosFilmes = db.Movies.ToList();
                //db.Movies.Remove(todosFilmes[0]);
                ////Atualizar os dados de um filme
                //Console.WriteLine("Atualizando um filme");
                //Movie batman = todosFilmes.Where(f => f.Title == "The Dark Knight").FirstOrDefault();
                //if (batman != null)
                //{
                //    batman.Title = "Batman - " + batman.Title;
                //}
                ////Persistir as alterações (verifique o SQL gerado)
                //db.SaveChanges();
                #endregion

                Console.ReadKey();
            }
        }


        private static void Seed(MovieContext context)
        {
            var genres = new List<Genre>

            {

                new Genre { Name = "Action",  Description = "An action story is similar to adventure, and the protagonist usually takes a risky turn, which leads to desperate situations (including explosions, fight scenes, daring escapes, etc.)." },
                new Genre { Name = "Adventure",  Description = "An adventure story is about a protagonist who journeys to epic or distant places to accomplish something. It can have many other genre elements included within it, because it is a very open genre." },
                new Genre { Name = "Animation",  Description = "Technically speaking, animation is more of a medium than a film genre in and of itself; as a result, animated movies can run the gamut of traditional genres with the only common factor being that they don’t rely predominantly on live action footage." },
                new Genre { Name = "Comedy",  Description = "Comedy is a story that tells about a series of funny or comical events, intended to make the audience laugh. It is a very open genre, and thus crosses over with many other genres on a frequent basis." },
                new Genre { Name = "Romantic Comedy",  Description = "A subgenre which combines the romance genre with comedy, focusing on two or more individuals as they discover and attempt to deal with their romantic love, attractions to each other. The stereotypical plot line follows the boy-gets-girl, boy-loses-girl, boy gets girl back again sequence. " },
                new Genre { Name = "Crime",  Description = "A subgenre which combines the romance genre with comedy, focusing on two or more individuals as they discover and attempt to deal with their romantic love, attractions to each other. " },
                new Genre { Name = "Drama",  Description = "Within film, television and radio (but not theatre), drama is a genre of narrative fiction (or semi-fiction) intended to be more serious than humorous in tone," },
                new Genre { Name = "Sci-Fi",  Description = "Science fiction is similar to fantasy, except stories in this genre use scientific understanding to explain the universe that it takes place in. It generally includes or is centered on the presumed effects or ramifications of computers or machines; travel through space, time or alternate universes; alien life-forms; genetic engineering; or other such things. " },
                new Genre { Name = "Romance",  Description = "Romance novels are emotion-driven stories that are primarily focused on the relationship between the main characters of the story." },
                new Genre { Name = "Fantasy",  Description = "A fantasy story is about magic or supernatural forces, rather than technology, though it often is made to include elements of other genres, such as science fiction elements, for instance computers or DNA, if it happens to take place in a modern or future era. " },
                new Genre { Name = "Sport",  Description = "The coverage of sports as a television program, on radio and other broadcasting media. It usually involves one or more sports commentators describing the events as they happen, which is called colour commentary." },
                new Genre { Name = "Western",  Description = "tories in the Western genre are set in the American West, between the time of the Civil war and the early nineteenth century." },
                new Genre { Name = "Thriller",  Description = "A Thriller is a story that is usually a mix of fear and excitement. It has traits from the suspense genre and often from the action, adventure or mystery genres, but the level of terror makes it borderline horror fiction at times as well. " },
                new Genre { Name = "Family",  Description = "The family saga is a genre of literature which chronicles the lives and doings of a family or a number of related or interconnected families over a period of time. " }
             };


            //genres.ForEach(s => context.Genres.AddOrUpdate(g => g.Name, s));
            context.Genres.AddRange(genres);
            context.SaveChanges();

            var movies = new List<Movie> {
               new Movie
               {
                   Title = "Rio Bravo",
                   ReleaseDate = DateTime.Parse("4/15/1959",new CultureInfo("en-US")),
                   Director = "Howard Hawks",
                   GenreID =  genres.Single( g => g.Name == "Western").GenreID,
                   Rating = 8.1,
                   Gross = 5750000
               },


            new Movie {
                 Title = "The Shawshank Redemption",
                ReleaseDate = DateTime.Parse("10/14/1994",new CultureInfo("en-US")),
                Director = "Frank Darabont" ,
                GenreID =  genres.Single( g => g.Name == "Drama").GenreID,
                Gross = 28341469,
                Rating = 9.3
            },

            new Movie { Title = "The Godfather", Director = "Francis Ford Coppola", ReleaseDate = DateTime.Parse("3/24/1972",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Drama").GenreID,Gross = 134821952 , Rating = 9.2},
            new Movie { Title = "Pulp Fiction", Director = "Quentin Tarantino", ReleaseDate = DateTime.Parse("10/14/1994",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Thriller").GenreID,Gross = 107930000, Rating = 8.9},
            new Movie { Title = "Schindlers List", Director = "Steven Spielberg", ReleaseDate = DateTime.Parse("2/4/1994",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Drama").GenreID,Gross = 96067179, Rating = 8.9 },
            new Movie { Title = "The Dark Knight", Director = "Christopher Nolan", ReleaseDate = DateTime.Parse("7/18/2008",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 533316061, Rating = 9.0 },
            new Movie { Title = "The Lord of the Rings: The Return of the King", Director = "Peter Jackson", ReleaseDate = DateTime.Parse("12/17/2003",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 377019252, Rating = 8.9 },
            new Movie { Title = "Star Wars", Director = "George Lucas", ReleaseDate = DateTime.Parse("5/25/1977",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 460935665, Rating = 8.7 },
            new Movie { Title = "The Matrix", Director = "The Wachowski Brothers", ReleaseDate = DateTime.Parse("3/31/1999",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Sci-Fi").GenreID,Gross = 171383253, Rating = 8.7},
            new Movie { Title = "Forrest Gump", Director = "Robert Zemeckis", ReleaseDate = DateTime.Parse("7/6/1994",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Comedy").GenreID,Gross = 329691196, Rating = 8.8 },
            new Movie { Title = "Monty Python and the Holy Grail", Director = " Terry Gilliam & Terry Jones", ReleaseDate = DateTime.Parse("5/23/1975",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Comedy").GenreID,Gross=1229197, Rating = 8.3 },
            new Movie { Title = "2001: A Space Odyssey", Director = "Stanley Kubrick", ReleaseDate = DateTime.Parse("4/29/1968",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Sci-Fi").GenreID,Gross = 56715371, Rating = 8.3 },
            new Movie { Title = "Back to the Future", Director = "Robert Zemeckis", ReleaseDate = DateTime.Parse("1/22/1989",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Family").GenreID,Gross = 210609762, Rating = 8.5},
            new Movie { Title = "Monsters Inc", Director = "Pete Docter & David Silverman", ReleaseDate = DateTime.Parse("11/2/2001",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Family").GenreID,Gross = 289907418, Rating = 8.1},



            new Movie { Title = "Jurassic Park", Director = "Steven Spielberg", ReleaseDate = DateTime.Parse("06/25/1993",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Thriller").GenreID,Gross =356784000 , Rating = 8.1},

            new Movie { Title = "The Empire Strikes Back", Director = "Irvin Kershner", ReleaseDate = DateTime.Parse("07/21/1980",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Sci-Fi").GenreID,Gross =290158751 , Rating = 8.8},
            new Movie { Title = "Return of the Jedi", Director = "Richard Marquand", ReleaseDate = DateTime.Parse("06/10/1983",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Sci-Fi").GenreID,Gross = 309125409 , Rating = 8.4},
            new Movie { Title = "GoldenEye", Director = "Martin Campbell", ReleaseDate = DateTime.Parse("12/15/1995",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross =106635996 , Rating = 7.2 },
            new Movie { Title = "The World Is Not Enough", Director = "Michael Apted", ReleaseDate = DateTime.Parse("12/24/1999",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross =126930660 , Rating = 6.4 },
            new Movie { Title = "Die Another Day", Director = "Lee Tamahori", ReleaseDate = DateTime.Parse("01/10/2003",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 160201106, Rating = 6.1},
            new Movie { Title = "Tomorrow Never Dies", Director = "Roger Spottiswoode", ReleaseDate = DateTime.Parse("01/16/1998",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 125332007, Rating = 6.5},
            new Movie { Title = "Skyfall", Director = "Sam Mendes", ReleaseDate = DateTime.Parse("10/26/2012",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 304360277, Rating = 7.8},
            new Movie { Title = "Casino Royale", Director = "Martin Campbell", ReleaseDate = DateTime.Parse("12/15/2006",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Action").GenreID,Gross = 167007184, Rating = 8.0},

            //Exercicio 11
            new Movie {Title = "Harry Potter and the Philosopher's Stone", Director = "Chris Columbus", ReleaseDate = DateTime.Parse("16/11/2001",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Fantasy").GenreID,Gross = 974755371 , Rating = 7.6},
            new Movie {Title = "Harry Potter and the Chamber of Secrets", Director = "Chris Columbus", ReleaseDate = DateTime.Parse("15/11/2002",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Fantasy").GenreID,Gross = 878979634 , Rating = 7.4},
            new Movie {Title = "Harry Potter and the Prisoner of Azkaban", Director = "Alfonso Cuarón", ReleaseDate = DateTime.Parse("04/06/2004",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Fantasy").GenreID,Gross = 796688549 , Rating = 7.9},
            new Movie {Title = "Harry Potter and the Goblet of Fire", Director = "Mike Newell", ReleaseDate = DateTime.Parse("18/11/2005",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Fantasy").GenreID,Gross = 896911078 , Rating = 7.7},

            new Movie {Title = "Inside Llewyn Davis", Director = "Joel e Ethan Coen", ReleaseDate = DateTime.Parse("10/01/2014",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Drama").GenreID,Gross = 32900 , Rating = 7.5},
            new Movie {Title = "Ex_Machina", Director = "Alex Garland", ReleaseDate = DateTime.Parse("10/05/2015",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Sci-Fi").GenreID,Gross = 37000 , Rating = 7.7},
            new Movie {Title = "Mulan", Director = "Tony Bancroft e Barry Cook", ReleaseDate = DateTime.Parse("19/06/1998",new CultureInfo("en-US")), GenreID =  genres.Single( g => g.Name == "Animation").GenreID,Gross = 304000000 , Rating = 7.6},

            };



            //movies.ForEach(s => context.Movies.AddOrUpdate(f => f.Title, s));
            context.Movies.AddRange(movies);
            context.SaveChanges();


            #region #dados para teste do exercício de casting
            //      //
            //      // inicio casting
            //      //
            var actors = new List<Actor>            {
                       #region Jurassic Park
                      new Actor{ Name = "Jeff Goldblum", DateBirth =   DateTime.Parse("9/22/1952",new CultureInfo("en-US")) },
                      new Actor{ Name = "Sam Neill", DateBirth =   DateTime.Parse("9/14/1947",new CultureInfo("en-US")) },
                      new Actor{ Name = "Laura Dern", DateBirth =   DateTime.Parse("2/10/1967",new CultureInfo("en-US")) },
                      new Actor{ Name = "Richard Attenborough", DateBirth =   DateTime.Parse("8/29/1923",new CultureInfo("en-US")) },
                      new Actor{ Name = "Samuel L. Jackson", DateBirth =   DateTime.Parse("12/21/1948",new CultureInfo("en-US")) },
                     #endregion

                      #region Star Wars
                      new Actor{ Name = "Mark Hamill",DateBirth =   DateTime.Parse("9/25/1951",new CultureInfo("en-US")) },
                      new Actor{ Name = "Carrie Fisher",DateBirth =   DateTime.Parse("10/21/1956",new CultureInfo("en-US")) },
                      new Actor{ Name = "Harrison Ford",DateBirth =   DateTime.Parse("7/13/1942",new CultureInfo("en-US")) },
                      new Actor{ Name = "David Prowse",DateBirth =   DateTime.Parse("7/1/1935",new CultureInfo("en-US")) },
                      #endregion

                      #region Forrest Gump
                      new Actor{ Name = "Tom Hanks", DateBirth =   DateTime.Parse("7/9/1956",new CultureInfo("en-US")) },
                      new Actor{ Name = "Robin Wright", DateBirth =   DateTime.Parse("4/8/1966",new CultureInfo("en-US")) },
                      new Actor{ Name = "Gary Sinise", DateBirth =   DateTime.Parse("3/17/1955",new CultureInfo("en-US")) },
                      #endregion

                      #region GoldenEye
                      new Actor{ Name = "Pierce Brosnan", DateBirth =   DateTime.Parse("5/16/1953",new CultureInfo("en-US")) },
                      new Actor{ Name = "Famke Janssen", DateBirth =   DateTime.Parse("9/5/1964",new CultureInfo("en-US")) },
                      new Actor{ Name = "Judi Dench", DateBirth =   DateTime.Parse("12/9/1934",new CultureInfo("en-US")) },
                     #endregion

                      #region The World Is Not Enough
                      new Actor{ Name = "Sophie Marceau", DateBirth =   DateTime.Parse("11/17/1966",new CultureInfo("en-US")) },
                      #endregion

                      #region Die Another Day
                      new Actor{ Name = "Halle Berry", DateBirth =   DateTime.Parse("8/14/1966",new CultureInfo("en-US")) },
                      #endregion

                      #region Tomorrow Never Dies
                      new Actor{ Name = "Michelle Yeoh", DateBirth =   DateTime.Parse("8/6/1962",new CultureInfo("en-US")) },
                      #endregion

                      #region Skyfall
                      new Actor{ Name = "Daniel Craig", DateBirth =   DateTime.Parse("3/2/1968",new CultureInfo("en-US")) },
                      new Actor{ Name = "Javier Bardem", DateBirth =   DateTime.Parse("3/1/1969",new CultureInfo("en-US")) },
                      #endregion

                      #region Casino Royale
                      new Actor{ Name = "Eva Green", DateBirth =   DateTime.Parse("7/6/1980",new CultureInfo("en-US")) },
                      new Actor{ Name = "Mads Mikkelsen", DateBirth =   DateTime.Parse("11/22/1965",new CultureInfo("en-US")) },
                      #endregion

                      //Exercicio 11
                      #region Harry Potter
                      new Actor{Name = "Daniel Radcliffe", DateBirth =   DateTime.Parse("23/7/1989",new CultureInfo("en-US")) },
                      new Actor{Name = "Rupert Grint", DateBirth =   DateTime.Parse("24/8/1988",new CultureInfo("en-US")) },
                      new Actor{Name = "Emma Watson", DateBirth =   DateTime.Parse("15/4/1990",new CultureInfo("en-US")) },
                      #endregion

                      #region Inside Llewyn Davis
                      new Actor{Name = "Oscar Isaac", DateBirth =   DateTime.Parse("09/3/1979",new CultureInfo("en-US")) },
                      new Actor{Name = "Carey Mulligan", DateBirth =   DateTime.Parse("28/5/1985",new CultureInfo("en-US")) },
                      new Actor{Name = "Justin Timberlake", DateBirth =   DateTime.Parse("31/1/1981",new CultureInfo("en-US")) },
                      #endregion

                      #region Ex_Machina
                      new Actor{Name = "Domhnall Gleeson", DateBirth =   DateTime.Parse("12/5/1983",new CultureInfo("en-US")) },
                      new Actor{Name = "Alicia Vikander", DateBirth =   DateTime.Parse("03/10/1988",new CultureInfo("en-US")) },
                      new Actor{Name = "Oscar Isaac", DateBirth =   DateTime.Parse("09/3/1979",new CultureInfo("en-US")) },
                      #endregion

                      #region Mulan
                      new Actor{Name = "Ming-Na", DateBirth =   DateTime.Parse("20/11/1963",new CultureInfo("en-US")) },
                      new Actor{Name = "Eddie Murphy", DateBirth =   DateTime.Parse("03/4/1961",new CultureInfo("en-US")) },
                      new Actor{Name = "B.D. Wong", DateBirth =   DateTime.Parse("24/10/1960",new CultureInfo("en-US")) },
                      #endregion
            };

            //actors.ForEach(s => context.Actors.AddOrUpdate(a => a.Name, s));
            context.Actors.AddRange(actors);
            context.SaveChanges();

            var actorCharacters = new List<ActorMovie>() {

                   #region Jurassic Park
                  new ActorMovie { ActorId = actors.Single(g => g.Name == "Jeff Goldblum").ActorId,
                                                          Character =  "Ian Malcolm",
                                                          MovieId = movies.Single(g => g.Title == "Jurassic Park").ID },
                  new ActorMovie { ActorId = actors.Single( g => g.Name == "Sam Neill").ActorId,  Character =  "Alan Grant", MovieId = movies.Single(g => g.Title == "Jurassic Park").ID },
                 new ActorMovie { ActorId = actors.Single( g => g.Name == "Laura Dern").ActorId,  Character =  "Ellie Sattler", MovieId = movies.Single(g => g.Title == "Jurassic Park").ID },
                 new ActorMovie { ActorId = actors.Single( g => g.Name == "Richard Attenborough").ActorId,  Character =  "John Hammond", MovieId = movies.Single(g => g.Title == "Jurassic Park").ID },
                 new ActorMovie { ActorId = actors.Single( g => g.Name == "Samuel L. Jackson").ActorId,  Character =  "Ray Arnold", MovieId = movies.Single(g => g.Title == "Jurassic Park").ID },
                 #endregion

                   #region SW
                  new ActorMovie { ActorId = actors.Single(g => g.Name == "Mark Hamill").ActorId, Character =  "Luke Skywalker", MovieId = movies.Single(g => g.Title == "Star Wars").ID },
                  new ActorMovie { ActorId = actors.Single( g => g.Name == "Carrie Fisher").ActorId,  Character =  "Leia Organa", MovieId = movies.Single(g => g.Title == "Star Wars").ID },
                  new ActorMovie { ActorId = actors.Single( g => g.Name == "Harrison Ford").ActorId,  Character =  "Han Solo", MovieId = movies.Single(g => g.Title == "Star Wars").ID },
                  new ActorMovie { ActorId = actors.Single( g => g.Name == "David Prowse").ActorId,  Character =  "Darth Vader", MovieId = movies.Single(g => g.Title == "Star Wars").ID },
                  #endregion


                  #region Forrest Gump
                  new ActorMovie { ActorId = actors.Single(g => g.Name == "Tom Hanks").ActorId, Character =  "Forrest Gump", MovieId = movies.Single(g => g.Title == "Forrest Gump").ID },
                      new ActorMovie { ActorId = actors.Single(g => g.Name == "Robin Wright").ActorId, Character =  "Jenny Curran", MovieId = movies.Single(g => g.Title == "Forrest Gump").ID },
                      new ActorMovie { ActorId = actors.Single(g => g.Name == "Gary Sinise").ActorId, Character =  "Lieutenant Dan Taylor", MovieId = movies.Single(g => g.Title == "Forrest Gump").ID },
                      #endregion


                     #region Empire
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Mark Hamill").ActorId, Character =  "Luke Skywalker", MovieId = movies.Single(g => g.Title == "The Empire Strikes Back").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Carrie Fisher").ActorId, Character =  "Leia Organa", MovieId = movies.Single(g => g.Title == "The Empire Strikes Back").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Harrison Ford").ActorId, Character =  "Han Solo", MovieId = movies.Single(g => g.Title == "The Empire Strikes Back").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "David Prowse").ActorId, Character =  "Darth Vader", MovieId = movies.Single(g => g.Title == "The Empire Strikes Back").ID },
                     #endregion

                     #region Jedi
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Mark Hamill").ActorId, Character =  "Luke Skywalker", MovieId = movies.Single(g => g.Title == "Return of the Jedi").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Carrie Fisher").ActorId, Character =  "Leia Organa", MovieId = movies.Single(g => g.Title == "Return of the Jedi").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Harrison Ford").ActorId, Character =  "Han Solo", MovieId = movies.Single(g => g.Title == "Return of the Jedi").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "David Prowse").ActorId, Character =  "Darth Vader", MovieId = movies.Single(g => g.Title == "Return of the Jedi").ID },
                     #endregion

                     #region GoldenEye
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Pierce Brosnan").ActorId, Character =  "James Bond", MovieId = movies.Single(g => g.Title == "GoldenEye").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Famke Janssen").ActorId, Character =  "Xenia Onatopp", MovieId = movies.Single(g => g.Title == "GoldenEye").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Judi Dench").ActorId, Character =  "M", MovieId = movies.Single(g => g.Title == "GoldenEye").ID },
                     #endregion

                     #region The World Is Not Enough
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Pierce Brosnan").ActorId, Character =  "James Bond", MovieId = movies.Single(g => g.Title == "The World Is Not Enough").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Sophie Marceau").ActorId, Character =  "Elektra King", MovieId = movies.Single(g => g.Title == "The World Is Not Enough").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Judi Dench").ActorId, Character =  "M", MovieId = movies.Single(g => g.Title == "The World Is Not Enough").ID },
                     #endregion

                     #region Die Another Day
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Pierce Brosnan").ActorId, Character =  "James Bond", MovieId = movies.Single(g => g.Title == "Die Another Day").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Halle Berry").ActorId, Character =  "Giacinta 'Jinx' Johnson", MovieId = movies.Single(g => g.Title == "Die Another Day").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Judi Dench").ActorId, Character =  "M", MovieId = movies.Single(g => g.Title == "Die Another Day").ID },
                     #endregion

                     #region Tomorrow Never Dies
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Pierce Brosnan").ActorId, Character =  "James Bond", MovieId = movies.Single(g => g.Title == "Tomorrow Never Dies").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Michelle Yeoh").ActorId, Character =  "Wai Lin", MovieId = movies.Single(g => g.Title == "Tomorrow Never Dies").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Judi Dench").ActorId, Character =  "M", MovieId = movies.Single(g => g.Title == "Tomorrow Never Dies").ID },
                      #endregion

                     #region Skyfall
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Daniel Craig").ActorId, Character =  "James Bond", MovieId = movies.Single(g => g.Title == "Skyfall").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Javier Bardem").ActorId, Character =  "Raoul Silva", MovieId = movies.Single(g => g.Title == "Skyfall").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Judi Dench").ActorId, Character =  "M", MovieId = movies.Single(g => g.Title == "Skyfall").ID },
                     #endregion

                       #region Casino Royale
                      new ActorMovie { ActorId = actors.Single(g => g.Name == "Daniel Craig").ActorId, Character =  "James Bond", MovieId = movies.Single(g => g.Title == "Casino Royale").ID },
                      new ActorMovie { ActorId = actors.Single(g => g.Name == "Eva Green").ActorId, Character =  "Vesper Lynd", MovieId = movies.Single(g => g.Title == "Casino Royale").ID },
                      new ActorMovie { ActorId = actors.Single(g => g.Name == "Mads Mikkelsen").ActorId, Character =  "Le Chiffre", MovieId = movies.Single(g => g.Title == "Casino Royale").ID },
                      new ActorMovie { ActorId = actors.Single(g => g.Name == "Judi Dench").ActorId, Character =  "M", MovieId = movies.Single(g => g.Title == "Casino Royale").ID },

                     #endregion
                    
                      //Exercicio 11
                      #region Pedra Filosofal
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Daniel Radcliffe").ActorId, Character =  "Harry Potter", MovieId = movies.Single(g => g.Title == "Harry Potter and the Philosopher's Stone").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Rupert Grint").ActorId, Character =  "Ron Weasley", MovieId = movies.Single(g => g.Title == "Harry Potter and the Philosopher's Stone").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Emma Watson").ActorId, Character =  "Hermione Granger", MovieId = movies.Single(g => g.Title == "Harry Potter and the Philosopher's Stone").ID },
                     #endregion

                     #region Camara Secreta
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Daniel Radcliffe").ActorId, Character =  "Harry Potter", MovieId = movies.Single(g => g.Title == "Harry Potter and the Chamber of Secrets").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Rupert Grint").ActorId, Character =  "Ron Weasley", MovieId = movies.Single(g => g.Title == "Harry Potter and the Chamber of Secrets").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Emma Watson").ActorId, Character =  "Hermione Granger", MovieId = movies.Single(g => g.Title == "Harry Potter and the Chamber of Secrets").ID },
                     #endregion

                     #region Pisioneiro de Azkaban
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Daniel Radcliffe").ActorId, Character =  "Harry Potter", MovieId = movies.Single(g => g.Title == "Harry Potter and the Prisoner of Azkaban").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Rupert Grint").ActorId, Character =  "Ron Weasley", MovieId = movies.Single(g => g.Title == "Harry Potter and the Prisoner of Azkaban").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Emma Watson").ActorId, Character =  "Hermione Granger", MovieId = movies.Single(g => g.Title == "Harry Potter and the Prisoner of Azkaban").ID },
                     #endregion

                     #region Calice de Fogo
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Daniel Radcliffe").ActorId, Character =  "Harry Potter", MovieId = movies.Single(g => g.Title == "Harry Potter and the Goblet of Fire").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Rupert Grint").ActorId, Character =  "Ron Weasley", MovieId = movies.Single(g => g.Title == "Harry Potter and the Goblet of Fire").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Emma Watson").ActorId, Character =  "Hermione Granger", MovieId = movies.Single(g => g.Title == "Harry Potter and the Goblet of Fire").ID },
                     #endregion

                     #region Inside Llewyn Davis
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Oscar Isaac").ActorId, Character =  "Llewyn Davis", MovieId = movies.Single(g => g.Title == "Inside Llewyn Davis").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Carey Mulligan").ActorId, Character =  "Jane", MovieId = movies.Single(g => g.Title == "Inside Llewyn Davis").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Justin Timberlake").ActorId, Character =  "Jim", MovieId = movies.Single(g => g.Title == "Inside Llewyn Davis").ID },
                     #endregion

                     #region Ex_Machina
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Domhnall Gleeson").ActorId, Character =  "Caleb Smith", MovieId = movies.Single(g => g.Title == "Ex_Machina").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Alicia Vikander").ActorId, Character =  "Ava", MovieId = movies.Single(g => g.Title == "Ex_Machina").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Oscar Isaac").ActorId, Character =  "Nathan Bateman", MovieId = movies.Single(g => g.Title == "Ex_Machina").ID },
                     #endregion

                     #region Mulan
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Ming-Na").ActorId, Character =  "Mulan", MovieId = movies.Single(g => g.Title == "Mulan").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "Eddie Murphy ").ActorId, Character =  "Mushu", MovieId = movies.Single(g => g.Title == "Mulan").ID },
                     new ActorMovie { ActorId = actors.Single(g => g.Name == "B.D. Wong").ActorId, Character =  "Shang", MovieId = movies.Single(g => g.Title == "Mulan").ID },
                     #endregion
            };

            //actorCharacters.ForEach(s => context.Characters.AddOrUpdate(ac => new { ac.ActorId, ac.MovieId }, s));
            context.Characters.AddRange(actorCharacters);
            context.SaveChanges();
            #endregion

        }
    }
}