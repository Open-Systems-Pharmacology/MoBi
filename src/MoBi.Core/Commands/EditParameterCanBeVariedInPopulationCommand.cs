using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterCanBeVariedInPopulationCommand : EditQuantityInBuildingBlockCommand<IParameter>
   {
      private readonly bool _newValue;
      private readonly bool _oldValue;

      public EditParameterCanBeVariedInPopulationCommand(IParameter parameter, bool newValue, IBuildingBlock buildingBlock) :
         base(parameter, buildingBlock)
      {
         _newValue = newValue;
         _oldValue = parameter.CanBeVariedInPopulation;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.CanBeVariedInPopulation, _oldValue.ToString(), newValue.ToString(), parameter.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterCanBeVariedInPopulationCommand(_quantity, _oldValue, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _quantity.CanBeVariedInPopulation = _newValue;
      }
   }
}