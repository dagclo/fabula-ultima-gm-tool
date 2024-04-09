using Microsoft.Data.Sqlite;

namespace FabulaUltimaDatabase.Configuration
{
    public static class DatabaseConfigurationExtensions
    {
        public static SqliteConnection GetConnection(this DatabaseConfiguration configuration) 
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = configuration.FileName,
                Mode = configuration.Mode,
            };

            return new SqliteConnection(connectionStringBuilder.ConnectionString);
        }

        public static void InitializeDatabase(this DatabaseConfiguration configuration)
        {
            if (File.Exists(configuration.FileName)) return; //todo: verify db has tables in it

            if (configuration.Mode == SqliteOpenMode.ReadWriteCreate)
            {
                using (var connection = configuration.GetConnection())
                {
                    connection.Open();
                    var buildDirectory = AppContext.BaseDirectory;
                    var path = Path.Combine(buildDirectory, "InitializationScripts");
                    foreach (var file in SortInDeployOrder(Directory.GetFiles(path)))
                    {
                        var sql = File.ReadAllText(file);
                        var command = connection.CreateCommand();
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                throw new NoDatabaseFileException(configuration.FileName);
            }
        }

        private static IEnumerable<string> SortInDeployOrder(string[] filePaths)
        {
            return filePaths.OrderBy(path =>
            {
                var filename = Path.GetFileName(path);
                var split = filename.Split("_");
                if (!int.TryParse(split[0] /* ordinal */, out int ordinal))
                {
                    throw new Exception($"unable to parse ordinal for path {path}");
                }
                return ordinal;
            });
        }        
    }
}
