using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ClearOriginalQuantitiesTrackerCommand : MoBiCommand
   {
      private IMoBiSimulation _simulation;

      public ClearOriginalQuantitiesTrackerCommand(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         ObjectType = ObjectTypes.Simulation;
         Description = AppConstants.Commands.RemoveTrackedQuantityChanges;
         CommandType = AppConstants.Commands.DeleteCommand;
         
         Visible = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _simulation.ClearOriginalQuantities();
      }

      protected override void ClearReferences()
      {
         _simulation = null;
      }
   }
}