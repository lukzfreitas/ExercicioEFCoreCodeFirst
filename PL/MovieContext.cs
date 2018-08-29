using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using T1.ExercicioEFCoreCodeFirst.PL;

namespace ExercicioEFCoreCodeFirst
{
    public class MovieContext : DbContext
    {
        public static readonly LoggerFactory FabricaLogger = new LoggerFactory(new[] { new
ConsoleLoggerProvider((_, __) => true, true) });
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             //.UseLoggerFactory(FabricaLogger)
            .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=DBMovieCF;Trusted_Connection=True;");
        }
    }
}