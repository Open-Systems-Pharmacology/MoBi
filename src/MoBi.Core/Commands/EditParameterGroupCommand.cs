using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterGroupCommand : EditQuantityInBuildingBlockCommand<IParameter>
   {
      private readonly IGroup _newGroup;
      private IGroup _oldGroup;

      public EditParameterGroupCommand(IParameter parameter, IGroup newGroup, IBuildingBlock buildingBlock)
         : base(parameter, buildingBlock)
      {
         _newGroup = newGroup;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var groupRepository = context.Resolve<IGroupRepository>();
         _oldGroup = groupRepository.GroupByName(_quantity.GroupName);
         _quantity.GroupName = _newGroup.Name;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.Group, _oldGroup.DisplayName, _newGroup.DisplayName, _quantity.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterGroupCommand(_quantity, _oldGroup, _buildingBlock).AsInverseFor(this);
      }
   }
}