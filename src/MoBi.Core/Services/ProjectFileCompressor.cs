using System.Data.Common;
using Microsoft.Data.Sqlite;
using OSPSuite.Core.Extensions;

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
         using (var sqlLite = new SqliteConnection(string.Format("Data Source={0}", path)))
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