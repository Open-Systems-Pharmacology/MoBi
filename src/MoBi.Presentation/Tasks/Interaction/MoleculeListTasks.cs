using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IMoleculeListTasks
   {
      IMoBiCommand MergeMoleculeLists(IMoleculeDependentBuilder builder, IMoleculeDependentBuilder targetBuilder, IBuildingBlock targetBuildingBlock);
   }

   public class MoleculeListTasks : IMoleculeListTasks
   {
      private readonly IMoBiContext _context;

      public MoleculeListTasks(IMoBiContext context)
      {
         _context = context;
      }

      public IMoBiCommand MergeMoleculeLists(IMoleculeDependentBuilder builder, IMoleculeDependentBuilder targetBuilder, IBuildingBlock targetBuildingBlock)
      {
         var newMoleculeList = createUnionMoleculeList(builder, targetBuilder);
         return new SetMoleculeListsCommand(targetBuilder, newMoleculeList, targetBuildingBlock).Run(_context);
      }

      private MoleculeList createUnionMoleculeList(IMoleculeDependentBuilder builder, IMoleculeDependentBuilder targetBuilder)
      {
         var newMoleculeList = targetBuilder.MoleculeList.Clone();
         builder.MoleculeList.MoleculeNames
            .Where(moleculeName => !newMoleculeList.MoleculeNames.Contains(moleculeName))
            .Each(newMoleculeList.AddMoleculeName);

         builder.MoleculeList.MoleculeNamesToExclude.Where(
            moleculeName => !newMoleculeList.MoleculeNamesToExclude.Contains(moleculeName))
            .Each(newMoleculeList.AddMoleculeNameToExclude);
         return newMoleculeList;
      }
   }
}