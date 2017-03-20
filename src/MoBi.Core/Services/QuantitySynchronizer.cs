using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IQuantitySynchronizer
   {
      /// <summary>
      ///    Returns the unexecuted command corresponding to the synchronize action of the <paramref name="quantity" /> with the
      ///    building block used in the <paramref name="simulation" />
      /// </summary>
      /// <remarks>The returned command is not executed</remarks>
      IMoBiCommand SynchronizeCommand(IQuantity quantity, IMoBiSimulation simulation);

      /// <summary>
      ///    Synchronizes the <paramref name="quantity" /> with the building block used in the <paramref name="simulation" /> and
      ///    returns the executed and hidden command corresponding to the changes performed
      /// </summary>
      IMoBiCommand Synchronize(IQuantity quantity, IMoBiSimulation simulation);

      /// <summary>
      ///    Synchronizes the value <paramref name="quantity" /> with the corresponding entries defined in
      ///    <paramref name="moleculeStartValues" /> if available
      /// </summary>
      void SynchronizeMoleculeStartValues(IQuantity quantity, IMoleculeStartValuesBuildingBlock moleculeStartValues);
   }

   public class QuantitySynchronizer : IQuantitySynchronizer,
      IVisitor<IReactionBuildingBlock>,
      IVisitor<ISpatialStructure>,
      IVisitor<IMoleculeBuildingBlock>
   {
      private readonly IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IMoBiContext _context;
      private IParameter _parameter;
      private IMoBiCommand _command;

      public QuantitySynchronizer(IAffectedBuildingBlockRetriever affectedBuildingBlockRetriever, IEntityPathResolver entityPathResolver, IMoBiContext context)
      {
         _affectedBuildingBlockRetriever = affectedBuildingBlockRetriever;
         _entityPathResolver = entityPathResolver;
         _context = context;
      }

      public IMoBiCommand SynchronizeCommand(IQuantity quantity, IMoBiSimulation simulation)
      {
         var affectedBuildingBlockInfo = _affectedBuildingBlockRetriever.RetrieveFor(quantity, simulation);
         if (affectedBuildingBlockInfo == null)
            return new MoBiEmptyCommand();

         var affectedBuildingBlock = affectedBuildingBlockInfo.UntypedBuildingBlock;
         if (affectedBuildingBlock.IsAnImplementationOf<IParameterStartValuesBuildingBlock>())
            return synchronizeParameterStartValueCommand(quantity as IParameter, affectedBuildingBlock.DowncastTo<IParameterStartValuesBuildingBlock>());

         if (affectedBuildingBlock.IsAnImplementationOf<IMoleculeStartValuesBuildingBlock>())
            return synchronizeMoleculeStartValueCommand(quantity, affectedBuildingBlock.DowncastTo<IMoleculeStartValuesBuildingBlock>());

         return synchronizeStructuralBuildingBlockCommand(quantity as IParameter, affectedBuildingBlock);
      }

      public IMoBiCommand Synchronize(IQuantity quantity, IMoBiSimulation simulation)
      {
         return SynchronizeCommand(quantity, simulation).AsHidden().Run(_context);
      }

      public void SynchronizeMoleculeStartValues(IQuantity quantity, IMoleculeStartValuesBuildingBlock moleculeStartValues)
      {
         synchronizeMoleculeStartValueCommand(quantity, moleculeStartValues, allowCreation: false).Run(_context);
      }

      private IMoBiCommand synchronizeStructuralBuildingBlockCommand(IParameter parameter, IBuildingBlock buildingBlock)
      {
         _command = new MoBiEmptyCommand();

         if (parameter == null)
            return _command;

         try
         {
            _parameter = parameter;
            this.Visit(buildingBlock);
            return _command;
         }
         finally
         {
            _parameter = null;
            _command = new MoBiEmptyCommand();
         }
      }

      private IMoBiCommand synchronizeMoleculeStartValueCommand(IQuantity quantity, IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock, bool allowCreation = true)
      {
         var moleculeAmount = quantity as IMoleculeAmount ?? quantity.ParentContainer as IMoleculeAmount;
         return synchronizeStartValueCommand(moleculeAmount, moleculeStartValuesBuildingBlock, allowCreation,
            msv => new SynchronizeMoleculeStartValueCommand(quantity, msv),
            () => new AddMoleculeStartValueFromQuantityInSimulationCommand(moleculeAmount, moleculeStartValuesBuildingBlock));
      }

      private IMoBiCommand synchronizeParameterStartValueCommand(IParameter parameter, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock, bool allowCreation = true)
      {
         return synchronizeStartValueCommand(parameter, parameterStartValuesBuildingBlock, allowCreation,
            psv => new SynchronizeParameterStartValueCommand(parameter, psv),
            () => new AddParameterStartValueFromQuantityInSimulationCommand(parameter, parameterStartValuesBuildingBlock));
      }

      private IMoBiCommand synchronizeStartValueCommand<TStartValue>(IQuantity quantity, IStartValuesBuildingBlock<TStartValue> startValuesBuildingBlock, bool allowCreation,
         Func<TStartValue, IMoBiCommand> synchronizeStartValueCommandFunc, Func<IMoBiCommand> createStartValueCommandFunc) where TStartValue : class, IStartValue
      {
         if (quantity == null)
            return new MoBiEmptyCommand();

         var objectPath = _entityPathResolver.ObjectPathFor(quantity);
         var startValue = startValuesBuildingBlock[objectPath];
         if (startValue != null)
            return synchronizeStartValueCommandFunc(startValue);

         if (allowCreation)
            return createStartValueCommandFunc();

         return new MoBiEmptyCommand();
      }

      public void Visit(IReactionBuildingBlock reactionBuildingBlock)
      {
         synchronizeInParameters(reactionBuildingBlock);
      }

      private ICommand synchronizeInParameters<TBuilder>(IEnumerable<TBuilder> builders) where TBuilder : IContainer
      {
         var buildingBlockParameters = new PathCache<IParameter>(_entityPathResolver);
         buildingBlockParameters.AddRange(builders.SelectMany(x => x.GetAllChildren<IParameter>()));
         var parameterPath = _entityPathResolver.PathFor(_parameter);
         var buildingBlockParameter = buildingBlockParameters[parameterPath];
         return setSynchronizeCommand(buildingBlockParameter);
      }

      public void Visit(ISpatialStructure spatialStructure)
      {
         synchronizeInParameters(spatialStructure);
      }

      public void Visit(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var command = synchronizeInParameters(moleculeBuildingBlock);
         if (!command.IsEmpty())
            return;

         var transporterMoleculeContainers = moleculeBuildingBlock.SelectMany(m => m.TransporterMoleculeContainerCollection).ToList();
         if (!transporterMoleculeContainers.Any())
            return;

         var parameterTransporterName = _parameter.ParentContainer.Name;
         var parameterMoleculeName = _parameter.ParentContainer.ParentContainer?.Name;

         var transporterMoleculeContainer = findTransporterMoleculeContainerFor(transporterMoleculeContainers, parameterTransporterName, parameterMoleculeName);

         if (transporterMoleculeContainer == null)
            return;

         var buildingBlockParameter = transporterMoleculeContainer.Parameter(_parameter.Name);
         setSynchronizeCommand(buildingBlockParameter);
      }

      private static TransporterMoleculeContainer findTransporterMoleculeContainerFor(List<TransporterMoleculeContainer> transporterMoleculeContainers, string parameterTransporterName, string parameterMoleculeName)
      {
         return transporterMoleculeContainers.FirstOrDefault(x => string.Equals(x.TransportName, parameterTransporterName) && string.Equals(x.ParentContainer.Name, parameterMoleculeName));
      }

      private ICommand setSynchronizeCommand(IParameter buildingBlockParameter)
      {
         _command = buildingBlockParameter != null ? new SynchronizeParameterValueCommand(_parameter, buildingBlockParameter) : (IMoBiCommand)new MoBiEmptyCommand();
         return _command;
      }
   }
}