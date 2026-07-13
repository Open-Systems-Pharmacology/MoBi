using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Services
{
   public interface IExpressionProfileRenamingTask
   {
      /// <summary>
      ///    Renames the <paramref name="expressionProfile" /> to <paramref name="newName" /> and updates the molecule name in the
      ///    paths of all expression parameters and initial conditions accordingly.
      /// </summary>
      void Rename(ExpressionProfileBuildingBlock expressionProfile, string newName);
   }

   public class ExpressionProfileRenamingTask : IExpressionProfileRenamingTask
   {
      public void Rename(ExpressionProfileBuildingBlock expressionProfile, string newName)
      {
         var oldMoleculeName = expressionProfile.MoleculeName;
         expressionProfile.Name = newName;
         var newMoleculeName = expressionProfile.MoleculeName;

         expressionProfile.ExpressionParameters.Each(x => renameMoleculeInPath(x, oldMoleculeName, newMoleculeName));
         expressionProfile.InitialConditions.Each(x => renameMoleculeInPath(x, oldMoleculeName, newMoleculeName));
      }

      private static void renameMoleculeInPath(PathAndValueEntity entity, string oldMoleculeName, string newMoleculeName)
      {
         var objectPath = entity.Path;
         objectPath.Replace(oldMoleculeName, newMoleculeName);
         entity.Path = objectPath;
      }
   }
}
