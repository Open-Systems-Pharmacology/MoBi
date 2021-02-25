using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ChangePathElementAtContainerPathCommand:BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _newElement;
      private readonly IApplicationMoleculeBuilder _applicationMoleculeBuilder;
      private readonly string _oldElement;

      public ChangePathElementAtContainerPathCommand(string newElement, IApplicationMoleculeBuilder applicationMoleculeBuilder, string oldElement,IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _newElement = newElement;
         _applicationMoleculeBuilder = applicationMoleculeBuilder;
         _oldElement = oldElement;
         Description = AppConstants.Commands.ChangeContainerPathElementDescription(_applicationMoleculeBuilder.Name, _newElement, _oldElement);
         ObjectType = ObjectTypes.FormulaUsablePath;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _applicationMoleculeBuilder.RelativeContainerPath.Replace(_oldElement, _newElement);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangePathElementAtContainerPathCommand(_oldElement, _applicationMoleculeBuilder, _newElement,_buildingBlock)
            .AsInverseFor(this);
      }
   }
}