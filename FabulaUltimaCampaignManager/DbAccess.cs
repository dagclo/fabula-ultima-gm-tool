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
            using var dbfile = FileAccess.Open(configuration.DatabaseFilePath, FileAccess.ModeFlags.Read);            
            var databaseFilePath = dbfile.GetPathAbsolute();
            var databaseConfiguration = new DatabaseConfiguration
            {
                FileName = databaseFilePath,
                Mode = Microsoft.Data.Sqlite.SqliteOpenMode.ReadWrite
            };
            var instance = new Instance(databaseConfiguration);
            Repository = new BeastiaryRepository(instance);
        }
    }
}
