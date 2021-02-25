using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditIsAdvancesParameterPropertyCommand : EditQuantityInBuildingBlockCommand<IParameter>
   {
      private readonly bool _newValue;
      private readonly bool _oldValue;

      public EditIsAdvancesParameterPropertyCommand(IParameter parameter, bool newValue, IBuildingBlock buildingBlock)
         : base(parameter, buildingBlock)
      {
         _newValue = newValue;
         _oldValue = parameter.Visible;
         Description = AppConstants.Commands.EditIsAdvancedParameterCommandDescription(parameter, _newValue);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditIsAdvancesParameterPropertyCommand(_quantity, _oldValue, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _quantity.Visible = !_newValue;
      }
   }
}