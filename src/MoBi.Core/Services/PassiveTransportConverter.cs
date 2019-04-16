using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Assets;

namespace MoBi.Core.Services
{
   public interface IPassiveTransportConverter
   {
      void Convert(object objectToConvert);
   }

   public class PassiveTransportConverter : IPassiveTransportConverter, IVisitor<IMoleculeBuildingBlock>, IVisitor<IMoleculeBuilder>, IVisitor<IBuildConfiguration>, IVisitor<SimulationTransfer>, IVisitor<IModelCoreSimulation>, IVisitor<IMoBiProject>

   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IContainerTask _containerTask;
      private readonly IEventPublisher _eventPublisher;
      private readonly ICloneManagerForModel _cloneManager;
      private readonly IFormulaTask _formulaTask;

      public PassiveTransportConverter(IObjectBaseFactory objectBaseFactory, IMoBiProjectRetriever projectRetriever, IContainerTask containerTask,
         IEventPublisher eventPublisher, ICloneManagerForModel cloneManager, IFormulaTask formulaTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _projectRetriever = projectRetriever;
         _containerTask = containerTask;
         _eventPublisher = eventPublisher;
         _cloneManager = cloneManager;
         _formulaTask = formulaTask;
      }

      public void Convert(object objectToConvert)
      {
         this.Visit(objectToConvert);
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var passiveTransportBuildingBlock = createPassiveTransportBuildingBlockForMolecule(moleculeBuildingBlock.Name);
         convert(moleculeBuildingBlock, passiveTransportBuildingBlock);
         addPassiveTransportToProject(passiveTransportBuildingBlock);
      }

      private void convert(IMoleculeBuildingBlock moleculeBuildingBlock, IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         convert(moleculeBuildingBlock, new[] {passiveTransportBuildingBlock});
      }

      private void convert(IMoleculeBuildingBlock moleculeBuildingBlock, IReadOnlyList<IPassiveTransportBuildingBlock> passiveTransportBuildingBlocks)
      {
         moleculeBuildingBlock.Each(m => convert(m, moleculeBuildingBlock, passiveTransportBuildingBlocks));
      }

      private IPassiveTransportBuildingBlock createPassiveTransportBuildingBlockForMolecule(string name)
      {
         return _objectBaseFactory.Create<IPassiveTransportBuildingBlock>()
            .WithName(_containerTask.CreateUniqueName(_projectRetriever.Current.PassiveTransportCollection, name, canUseBaseName: true));
      }

      public void Visit(IMoleculeBuilder moleculeBuilder)
      {
         var passiveTransportBuildingBlock = createPassiveTransportBuildingBlockForMolecule(moleculeBuilder.Name);
         convert(moleculeBuilder, null, passiveTransportBuildingBlock);
         addPassiveTransportToProject(passiveTransportBuildingBlock);
      }

      private void convert(IMoleculeBuilder moleculeBuilder, IMoleculeBuildingBlock moleculeBuildingBlock, IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         convert(moleculeBuilder, moleculeBuildingBlock, new[] {passiveTransportBuildingBlock});
      }

      private void convert(IMoleculeBuilder moleculeBuilder, IMoleculeBuildingBlock moleculeBuildingBlock, IReadOnlyList<IPassiveTransportBuildingBlock> passiveTransportBuildingBlocks)
      {
         var allPassiveTransports = moleculeBuilder.GetChildren<ITransportBuilder>().ToList();
         if (!allPassiveTransports.Any()) return;

         foreach (var passiveTransport in allPassiveTransports)
         {
            moleculeBuilder.RemoveChild(passiveTransport);
            var defaultName = AppConstants.CompositeNameFor(passiveTransport.Name, moleculeBuilder.Name);

            foreach (var passiveTransportBuildingBlock in passiveTransportBuildingBlocks)
            {
               if (similarPassiveTransportAlreadyExists(passiveTransportBuildingBlock, passiveTransport, defaultName))
               {
                  var existingPassiveTransport = passiveTransportBuildingBlock.FindByName(defaultName);
                  existingPassiveTransport.AddMoleculeName(moleculeBuilder.Name);
                  continue;
               }

               var passiveTransportClone = _cloneManager.Clone(passiveTransport);
               passiveTransportClone.Name = _containerTask.CreateUniqueName(passiveTransportBuildingBlock, defaultName, canUseBaseName: true);
               passiveTransportClone.ForAll = false;
               passiveTransportClone.AddMoleculeName(moleculeBuilder.Name);
               passiveTransportClone.Parameters.Each(x => x.BuildMode = ParameterBuildMode.Local);
               passiveTransportBuildingBlock.Add(passiveTransportClone);
               passiveTransportBuildingBlock.AddFormula(passiveTransportClone.Formula);
            }

            if (moleculeBuildingBlock != null)
               moleculeBuildingBlock.FormulaCache.Remove(passiveTransport.Formula);
         }
      }

      private bool similarPassiveTransportAlreadyExists(IPassiveTransportBuildingBlock passiveTransportBuildingBlock, ITransportBuilder passiveTransport, string passiveTransportName)
      {
         var existingPassiveTransport = passiveTransportBuildingBlock.FindByName(passiveTransportName);
         if (existingPassiveTransport == null) return false;

         return _formulaTask.FormulasAreTheSame(existingPassiveTransport.Formula, passiveTransport.Formula);
      }

      private void convert(IMoBiProject project)
      {
         //we should only convert the template building blocks by creating the cartesian product of all 
         foreach (var moleculeBuildingBlock in project.MoleculeBlockCollection)
         {
            convert(moleculeBuildingBlock, project.PassiveTransportCollection);
         }
      }

      private void addPassiveTransportToProject(IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         _projectRetriever.Current.AddBuildingBlock(passiveTransportBuildingBlock);
         var notification = new NotificationMessage(passiveTransportBuildingBlock, MessageOrigin.Simulation, passiveTransportBuildingBlock, NotificationType.Info)
         {
            Message = AppConstants.Warnings.PassiveTransportBuildingBlockCreatedAutomatically(passiveTransportBuildingBlock.Name),
            ObjectType = ObjectTypes.PassiveTransportBuildingBlock,
            BuildingBlockType = ObjectTypes.PassiveTransportBuildingBlock,
         };
         _eventPublisher.PublishEvent(new ShowNotificationsEvent(notification));
      }

      public void Visit(IBuildConfiguration buildConfiguration)
      {
         convert(buildConfiguration.Molecules, buildConfiguration.PassiveTransports);
      }

      public void Visit(SimulationTransfer simulationTransfer)
      {
         Visit(simulationTransfer.Simulation);
      }

      public void Visit(IModelCoreSimulation simulation)
      {
         Visit(simulation.BuildConfiguration);
      }

      public void Visit(IMoBiProject project)
      {
         convert(project);
         project.Simulations.Each(Visit);
      }
   }
}