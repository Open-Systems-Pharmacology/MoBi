using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterCanBeVariedCommand : EditQuantityInBuildingBlockCommand<IParameter>
   {
      private readonly bool _newValue;
      private readonly bool _oldValue;

      public EditParameterCanBeVariedCommand(IParameter parameter, bool newValue, IBuildingBlock buildingBlock) :
         base(parameter, buildingBlock)
      {
         _newValue = newValue;
         _oldValue = parameter.CanBeVaried;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.CanBeVaried, _oldValue.ToString(), newValue.ToString(), parameter.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterCanBeVariedCommand(_quantity, _oldValue, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _quantity.CanBeVaried = _newValue;
         context.PublishEvent(new ParameterChangedEvent(_quantity));
      }
   }
}