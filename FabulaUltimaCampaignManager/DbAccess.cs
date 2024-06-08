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
            if(!FileAccess.FileExists(configuration.DatabaseFilePath))
            {
                const string packedBeastiaryDBPath = "res://Database/BeastiaryDB.sqlite";
                using var directory = DirAccess.Open("res://Database");
                directory.Copy(packedBeastiaryDBPath, configuration.DatabaseFilePath);
            }

            using var dbfile = FileAccess.Open(configuration.DatabaseFilePath, FileAccess.ModeFlags.Read);            
            var databaseFilePath = dbfile.GetPathAbsolute();

            var databaseConfiguration = new DatabaseConfiguration
            {
                FileName = databaseFilePath,
                Mode = Microsoft.Data.Sqlite.SqliteOpenMode.ReadWrite
            };
            try
            {
                var instance = new Instance(databaseConfiguration);
                Repository = new BeastiaryRepository(instance);
            }
            catch(NoDatabaseFileException)
            {
                throw new System.Exception($"can't find {configuration.DatabaseFilePath} {dbfile.GetPath()}");
            }            
        }
    }
}
