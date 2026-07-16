using Microsoft.Data.Sqlite;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Serialization.Extensions;
using System.Data.Common;

namespace MoBi.Core.Services
{
   public interface IProjectFileCompressor
   {
      void Compress(string projectFile);
   }

   public class ProjectFileCompressor : IProjectFileCompressor
   {
      public void Compress(string projectFile)
      {
         var path = projectFile.ToUNCPath();
         using (var sqlLite = new SqliteConnection(ConnectionStringHelper.ConnectionStringFor(path)))
         {
            sqlLite.Open();
            vacuum(sqlLite);
         }
      }

      private void vacuum(SqliteConnection sqlLite)
      {
         ExecuteNonQuery(sqlLite, "vacuum;");
      }

      public int ExecuteNonQuery(DbConnection connection, string query)
      {
         using (var command = connection.CreateCommand())
         {
            command.CommandText = query;
            return command.ExecuteNonQuery();
         }
      }
   }
}