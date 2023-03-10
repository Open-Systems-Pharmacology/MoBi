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

      public AddBuildingBlocksToModuleCommand(Module existingModule, Module moduleWithNewBuildingBlocks)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<Module>();
         CommandType = AppConstants.Commands.AddCommand;
         _existingModule = existingModule;
         _moduleWithNewBuildingBlocks = moduleWithNewBuildingBlocks;
         Silent = false; //not sure
         //Description = AppConstants.Commands.AddToProjectDescription(ObjectType, module.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
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

            if (!Silent)
               context.PublishEvent(new AddedEvent<IEventGroupBuildingBlock>(_moduleWithNewBuildingBlocks.EventGroup, _existingModule));
         }


         if (_moduleWithNewBuildingBlocks.PassiveTransport != null)
            _existingModule.PassiveTransport = _moduleWithNewBuildingBlocks.PassiveTransport; 

         if (_moduleWithNewBuildingBlocks.Reaction != null)
            _existingModule.Reaction = _moduleWithNewBuildingBlocks.Reaction;

         if (_moduleWithNewBuildingBlocks.SpatialStructure != null)
            _existingModule.SpatialStructure = _moduleWithNewBuildingBlocks.SpatialStructure;

         //should work with empty, but let's test
         _moduleWithNewBuildingBlocks.ParameterStartValuesCollection.Each(x => _existingModule.AddParameterStartValueBlock(x));
         _moduleWithNewBuildingBlocks.MoleculeStartValuesCollection.Each(x => _existingModule.AddMoleculeStartValueBlock(x));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         //  _module = context.Get<Module>(ModuleId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveModuleCommand(_existingModule).AsInverseFor(this); //this is wrong
      }

      protected override void ClearReferences()
      {
         //_module = null;
      }

      public bool Silent { get; set; }
   }
}