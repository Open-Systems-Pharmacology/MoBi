using System;
using Microsoft.Data.Sqlite;

namespace TestSqliteMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Microsoft.Data.Sqlite migration...");
            
            // Test basic connection
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();
                Console.WriteLine("✓ Connection opened successfully");
                
                // Test basic query
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1 as test_value";
                    var result = command.ExecuteScalar();
                    Console.WriteLine($"✓ Query executed successfully: {result}");
                }
                
                // Test VACUUM command (used in ProjectFileCompressor)
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "VACUUM";
                    command.ExecuteNonQuery();
                    Console.WriteLine("✓ VACUUM command executed successfully");
                }
            }
            
            Console.WriteLine("✓ All Microsoft.Data.Sqlite tests passed!");
        }
    }
}