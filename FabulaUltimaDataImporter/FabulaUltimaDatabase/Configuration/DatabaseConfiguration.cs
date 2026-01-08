namespace FabulaUltimaDatabase.Configuration
{
    public class DatabaseConfiguration
    {
        public string? FileName { get; set; }

        public Microsoft.Data.Sqlite.SqliteOpenMode Mode { get; set; }
    }
}
