using System.Data.Common;
using System.Data.SQLite;
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
         using (var sqlLite = new SQLiteConnection(string.Format("Data Source={0}", path)))
         {
            sqlLite.Open();
            vacuum(sqlLite);
         }
      }

      private void vacuum(SQLiteConnection sqlLite)
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