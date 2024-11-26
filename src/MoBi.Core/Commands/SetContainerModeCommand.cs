using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
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
         if (_container.Mode == ContainerMode.Logical)
         {
            var moleculeProperties = _container.Children
               .OfType<IContainer>()
               .Where(child => child.IsMoleculeProperties())
               .ToList();

            foreach (var item in moleculeProperties)
            {
               _container.RemoveChild(item);
            }
         }
         else
         {
            var moleculeProperties = context.Create<IContainer>()
               .WithName(Constants.MOLECULE_PROPERTIES)
               .WithMode(ContainerMode.Logical);
            _container.Add(moleculeProperties);
         }
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