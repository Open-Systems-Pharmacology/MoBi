using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand<T> : SwapBuildingBlockCommand<T> where T : class, IBuildingBlock
   {
      private IMoBiSimulation _simulation;

      /// <summary>
      ///    Will create a command that will swap out the template building block <paramref name="templateBuildingBlock" /> with
      ///    the <paramref name="simulationBuildingBlockClone" />
      /// </summary>
      /// <param name="templateBuildingBlock">Template building block to remove</param>
      /// <param name="simulationBuildingBlockClone">Clone of simulation building block to use to replace the template</param>
      /// <param name="simulation">
      ///    Simulation containing the building block that was cloned and that will be replacing the
      ///    template building block
      /// </param>
      public UpdateTemplateBuildingBlockFromSimulationBuildingBlockCommand(T templateBuildingBlock, T simulationBuildingBlockClone, IMoBiSimulation simulation)
         : base(templateBuildingBlock, simulationBuildingBlockClone)
      {
         _simulation = simulation;
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         Description = AppConstants.Commands.UpdateTemplateBuildingCommandDescription(ObjectType, templateBuildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _simulation = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var buildingBlockInfo = _simulation.MoBiBuildConfiguration.BuildingInfoForTemplate(oldTemplateBuildingBlock);
         var buildingBlockUsedInSimulation = buildingBlockInfo.UntypedBuildingBlock;

         //Update simulation info and save the old one for undo
         buildingBlockInfo.SimulationChanges = 0;

         base.ExecuteWith(context);

         var bbInfoUpdater = context.Resolve<IBuildingBlockReferenceUpdater>();
         bbInfoUpdater.UpdateTemplateReference(context.CurrentProject, newTemplateBuildingBlock);

         //increment the version of the template building block to be the version of the swapped building block +1
         newTemplateBuildingBlock.Version = oldTemplateBuildingBlock.Version + 1;
         //make sure building block used in Simulation and new template have the same version
         buildingBlockUsedInSimulation.Version = newTemplateBuildingBlock.Version;

         //notify version changed
         var buildingBlockVersionUpdater = context.Resolve<IBuildingBlockVersionUpdater>();
         buildingBlockVersionUpdater.UpdateBuildingBlockVersion(newTemplateBuildingBlock, newTemplateBuildingBlock.Version);
      }

      private T oldTemplateBuildingBlock
      {
         get { return _oldBuildingBlock; }
      }

      private T newTemplateBuildingBlock
      {
         get { return _newBuildingBlock; }
      }
   }
}