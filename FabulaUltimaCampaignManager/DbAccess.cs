using FabulaUltimaDatabase;
using FabulaUltimaDatabase.Configuration;
using FirstProject.Beastiary;
using Godot;

namespace FirstProject
{
    public partial class DbAccess : Node
    {   
        public BeastiaryRepository Repository { get; set; }

        public DbAccess() 
        {
            var configuration = ResourceExtensions.Load<Configuration>("res://configuration.tres");
            var databaseConfiguration = new DatabaseConfiguration
            {
                FileName = configuration.DatabaseFilePath,
                Mode = Microsoft.Data.Sqlite.SqliteOpenMode.ReadWrite
            };
            var instance = new Instance(databaseConfiguration);
            Repository = new BeastiaryRepository(instance);
        }
    }
}
