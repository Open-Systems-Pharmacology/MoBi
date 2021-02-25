using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

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
         addContainerVolumeParameter(context);
         Description = AppConstants.Commands.EditDescription(ObjectTypes.Container, AppConstants.Captions.ContainerMode, _oldContainerMode.ToString(), _newContainerMode.ToString(), _container.Name);
      }

      private void addContainerVolumeParameter(IMoBiContext context)
      {
         var volume = _container.GetSingleChildByName<IParameter>(Constants.Parameters.VOLUME);

         //switch from physical to logical. If volume was created here, this command is an inverse and volume should be removed
         if (_newContainerMode == ContainerMode.Logical)
         {
            if (_volumeParameterCreatedHere && volume != null)
            {
               _container.RemoveChild(volume);
               context.Unregister(volume);
               context.PublishEvent(new RemovedEvent(volume, _container));
            }
         }
         //we switched from Logical to physical. Add volume parameter if not available
         else
         {
            if (volume != null) return;
            var parameterFactory = context.Resolve<IParameterFactory>();
            volume = parameterFactory.CreateVolumeParameter();

            _container.Add(volume);
            context.Register(volume);
            _volumeParameterCreatedHere = true;
            context.PublishEvent(new AddedEvent<IParameter>(volume, _container));
         }
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