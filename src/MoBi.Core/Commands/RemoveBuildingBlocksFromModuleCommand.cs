using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class RemoveBuildingBlocksFromModuleCommand : MoBiReversibleCommand
   {
      protected Module _existingModule;
      protected Module _moduleWithAddedBuildingBlocks;
      public string ExistingModuleId { get; private set; }
      public string ModuleWithAddedBuildingBlocksId { get; private set; }
      public bool Silent { get; set; }

      public RemoveBuildingBlocksFromModuleCommand(Module existingModule, Module moduleWithAddedBuildingBlocks)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<Module>();
         CommandType = AppConstants.Commands.AddCommand;
         _existingModule = existingModule;
         _moduleWithAddedBuildingBlocks = moduleWithAddedBuildingBlocks;
         ExistingModuleId = existingModule.Id;
         ModuleWithAddedBuildingBlocksId = moduleWithAddedBuildingBlocks.Id;
         Silent = false;
         Description = AppConstants.Commands.AddBuildingBlocksToModule(_existingModule.Name);
      }


      //OK, this needs some work more
      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_moduleWithAddedBuildingBlocks.Molecule != null)
         {
            _existingModule.Molecule = null;

            context.Unregister(_existingModule.Molecule);

            if (!Silent)
               context.PublishEvent(new RemovedEvent(_existingModule.Molecule, _existingModule));
         }


         if (_moduleWithAddedBuildingBlocks.Observer != null)
         {
            _existingModule.Observer = null;

            context.Unregister(_existingModule.Observer);

            if (!Silent)
               context.PublishEvent(new RemovedEvent(_existingModule.Observer, _existingModule));
         }

         if (_moduleWithAddedBuildingBlocks.EventGroup != null)
         {
            _existingModule.EventGroup = _moduleWithAddedBuildingBlocks.EventGroup;

            if (!Silent)
               context.PublishEvent(new AddedEvent<IEventGroupBuildingBlock>(_moduleWithAddedBuildingBlocks.EventGroup, _existingModule));
         }


         if (_moduleWithAddedBuildingBlocks.PassiveTransport != null)
         {
            _existingModule.PassiveTransport = _moduleWithAddedBuildingBlocks.PassiveTransport;

            if (!Silent)
               context.PublishEvent(new AddedEvent<IPassiveTransportBuildingBlock>(_moduleWithAddedBuildingBlocks.PassiveTransport, _existingModule));
         }


         if (_moduleWithAddedBuildingBlocks.Reaction != null)
         {
            _existingModule.Reaction = _moduleWithAddedBuildingBlocks.Reaction;

            if (!Silent)
               context.PublishEvent(new AddedEvent<IReactionBuildingBlock>(_moduleWithAddedBuildingBlocks.Reaction, _existingModule));
         }


         if (_moduleWithAddedBuildingBlocks.SpatialStructure != null)
         {
            _existingModule.SpatialStructure = _moduleWithAddedBuildingBlocks.SpatialStructure;

            if (!Silent)
               context.PublishEvent(new AddedEvent<ISpatialStructure>(_moduleWithAddedBuildingBlocks.SpatialStructure, _existingModule));
         }

         _moduleWithAddedBuildingBlocks.ParameterStartValuesCollection.Each(x => _existingModule.AddParameterStartValueBlock(x));
         _moduleWithAddedBuildingBlocks.MoleculeStartValuesCollection.Each(x => _existingModule.AddMoleculeStartValueBlock(x));

      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _existingModule = context.Get<Module>(ExistingModuleId);
         _moduleWithAddedBuildingBlocks = context.Get<Module>(ModuleWithAddedBuildingBlocksId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddBuildingBlocksToModuleCommand(_existingModule, _moduleWithAddedBuildingBlocks).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _existingModule = null;
         _moduleWithAddedBuildingBlocks = null;
      }
   }
}