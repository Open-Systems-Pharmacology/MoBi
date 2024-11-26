using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SetContainerModeCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IContainer _container;
      private readonly ContainerMode _newContainerMode;
      private readonly ContainerMode _oldContainerMode;
      private readonly string _containerId;
      private bool _volumeParameterCreatedHere;

      public SetContainerModeCommand(IBuildingBlock buildingBlock, IContainer container, ContainerMode newContainerMode)
         : base(buildingBlock)
      {
         _container = container;
         _containerId = container.Id;
         _newContainerMode = newContainerMode;
         _oldContainerMode = _container.Mode;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.Container;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _container.Mode = _newContainerMode;
         Description = AppConstants.Commands.EditDescription(ObjectTypes.Container, AppConstants.Captions.ContainerMode, _oldContainerMode.ToString(), _newContainerMode.ToString(), _container.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _container = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetContainerModeCommand(_buildingBlock, _container, _oldContainerMode)
         {
            _volumeParameterCreatedHere = _volumeParameterCreatedHere
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _container = context.Get<IContainer>(_containerId);
      }
   }
}