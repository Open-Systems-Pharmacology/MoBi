using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IApplicationMoleculeBuilder _applicationMoleculeBuilder;
      private readonly IObjectPath _newPath;
      private readonly IObjectPath _oldPath;
      private readonly string _applicationMoleculeBuilderId;

      public EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand(IApplicationMoleculeBuilder applicationMoleculeBuilder, IObjectPath newPath, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _applicationMoleculeBuilder = applicationMoleculeBuilder;
         _applicationMoleculeBuilderId = _applicationMoleculeBuilder.Id;
         _newPath = newPath;
         _oldPath = applicationMoleculeBuilder.RelativeContainerPath;
         ObjectType = ObjectTypes.ApplicationMoleculeBuilder;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.RelativeContainerPath, _oldPath.PathAsString, newPath.PathAsString, applicationMoleculeBuilder.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditRelativeContainerPathPropertyAtApplicationMoleculeBuilderCommand(_applicationMoleculeBuilder, _oldPath, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _applicationMoleculeBuilder = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _applicationMoleculeBuilder.RelativeContainerPath = _newPath;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _applicationMoleculeBuilder = context.Get<IApplicationMoleculeBuilder>(_applicationMoleculeBuilderId);
      }
   }
}