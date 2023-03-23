using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditSolverPropertyCommand : EditObjectPropertyInBuildingBlockCommand<SimulationSettings>
   {
      public EditSolverPropertyCommand(string propertyName, object newValue, object oldValue, SimulationSettings simulationSettings)
         : base(propertyName, newValue, oldValue, simulationSettings.Solver, simulationSettings, simulationSettings.Name)
      {
         ObjectType = ObjectTypes.SolverProperty;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         Description = AppConstants.Commands.EditSolverPropertyDescription(PropertyName, NewValueSerializationString, _buildingBlock.Name);
      }

      protected override object RestoreObjectToUpdate(IMoBiContext context)
      {
         return _buildingBlock.Solver;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditSolverPropertyCommand(PropertyName, _oldValue, _newValue, _buildingBlock).AsInverseFor(this);
      }
   }
}