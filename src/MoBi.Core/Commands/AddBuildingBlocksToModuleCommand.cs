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
   public class AddBuildingBlocksToModuleCommand : MoBiReversibleCommand, ISilentCommand
   {
      protected Module _existingModule;
      protected Module _moduleWithNewBuildingBlocks;
      public string ExistingModuleId { get; private set; }
      public string ModuleWithNewBuildingBlocksId { get; private set; }

      public AddBuildingBlocksToModuleCommand(Module existingModule, Module moduleWithNewBuildingBlocks)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<Module>();
         CommandType = AppConstants.Commands.AddCommand;
         _existingModule = existingModule;
         _moduleWithNewBuildingBlocks = moduleWithNewBuildingBlocks;
         ExistingModuleId = existingModule.Id;
         ModuleWithNewBuildingBlocksId = moduleWithNewBuildingBlocks.Id;
         Silent = false;
         Description = AppConstants.Commands.AddBuildingBlocksToModule(_existingModule.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         context.Register(_existingModule);
         context.Register(_moduleWithNewBuildingBlocks);

         if (_moduleWithNewBuildingBlocks.Molecule != null)
         {
            _existingModule.Molecule = _moduleWithNewBuildingBlocks.Molecule;

            context.Register(_existingModule.Molecule);

            if (!Silent)
               context.PublishEvent(new AddedEvent<IMoleculeBuildingBlock>(_existingModule.Molecule, _existingModule));
         }


         if (_moduleWithNewBuildingBlocks.Observer != null)
         {
            _existingModule.Observer = _moduleWithNewBuildingBlocks.Observer;

            context.Register(_existingModule.Observer);

            if (!Silent)
               context.PublishEvent(new AddedEvent<IObserverBuildingBlock>(_existingModule.Observer, _existingModule));
         }

         if (_moduleWithNewBuildingBlocks.EventGroup != null)
         {
            _existingModule.EventGroup = _moduleWithNewBuildingBlocks.EventGroup;

            context.Register(_existingModule.Observer); 

            if (!Silent)
               context.PublishEvent(new AddedEvent<IEventGroupBuildingBlock>(_moduleWithNewBuildingBlocks.EventGroup, _existingModule));
         }


         if (_moduleWithNewBuildingBlocks.PassiveTransport != null)
         {
            _existingModule.PassiveTransport = _moduleWithNewBuildingBlocks.PassiveTransport;

            context.Register(_existingModule.PassiveTransport);

            if (!Silent)
               context.PublishEvent(new AddedEvent<IPassiveTransportBuildingBlock>(_moduleWithNewBuildingBlocks.PassiveTransport, _existingModule));
         }


         if (_moduleWithNewBuildingBlocks.Reaction != null)
         {
            _existingModule.Reaction = _moduleWithNewBuildingBlocks.Reaction;

            if (!Silent)
               context.PublishEvent(new AddedEvent<IReactionBuildingBlock>(_moduleWithNewBuildingBlocks.Reaction, _existingModule));
         }


         if (_moduleWithNewBuildingBlocks.SpatialStructure != null)
         {
            _existingModule.SpatialStructure = _moduleWithNewBuildingBlocks.SpatialStructure;

            if (!Silent)
               context.PublishEvent(new AddedEvent<ISpatialStructure>(_moduleWithNewBuildingBlocks.SpatialStructure, _existingModule));
         }

         _moduleWithNewBuildingBlocks.ParameterStartValuesCollection.Each(x => _existingModule.AddParameterStartValueBlock(x));
         _moduleWithNewBuildingBlocks.MoleculeStartValuesCollection.Each(x => _existingModule.AddMoleculeStartValueBlock(x));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _existingModule = context.Get<Module>(ExistingModuleId);
         _moduleWithNewBuildingBlocks = context.Get<Module>(ModuleWithNewBuildingBlocksId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveBuildingBlocksFromModuleCommand(_existingModule, _moduleWithNewBuildingBlocks).AsInverseFor(this); 
      }

      protected override void ClearReferences()
      {
         _existingModule = null;
         _moduleWithNewBuildingBlocks = null;
      }

      public bool Silent { get; set; }
   }
}