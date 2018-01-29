namespace Dominio
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class DataContext : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'DataContext' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'Dominio.DataContext' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'DataContext'  en el archivo de configuración de la aplicación.
        public DataContext(): base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<Dominio.League> Leagues { get; set; }

        public System.Data.Entity.DbSet<Dominio.Team> Teams { get; set; }

        public System.Data.Entity.DbSet<Dominio.Tournament> Tournaments { get; set; }

        public System.Data.Entity.DbSet<Dominio.TournamentGroup> TournamentGroups { get; set; }

        public System.Data.Entity.DbSet<Dominio.Date> Dates { get; set; }

        public System.Data.Entity.DbSet<Dominio.TournamentTeam> TournamentTeams { get; set; }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}