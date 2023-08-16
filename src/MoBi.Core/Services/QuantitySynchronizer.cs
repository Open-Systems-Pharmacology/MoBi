using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Visitor;

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
      ///    <paramref name="initialConditions" /> if available
      /// </summary>
      void SynchronizeInitialConditions(IQuantity quantity, InitialConditionsBuildingBlock initialConditions);
   }

   public class QuantitySynchronizer : IQuantitySynchronizer,
      IVisitor<ReactionBuildingBlock>,
      IVisitor<SpatialStructure>,
      IVisitor<MoleculeBuildingBlock>
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IMoBiContext _context;
      private IParameter _parameter;
      private IMoBiCommand _command;

      public QuantitySynchronizer(IEntityPathResolver entityPathResolver, IMoBiContext context)
      {
         _entityPathResolver = entityPathResolver;
         _context = context;
      }

      public IMoBiCommand SynchronizeCommand(IQuantity quantity, IMoBiSimulation simulation)
      {
         //TODO SIMULATION_CONFIGURATION
         return new MoBiEmptyCommand();
         // var affectedBuildingBlockInfo = _affectedBuildingBlockRetriever.RetrieveFor(quantity, simulation);
         // if (affectedBuildingBlockInfo == null)
         //    return new MoBiEmptyCommand();
         //
         // var affectedBuildingBlock = affectedBuildingBlockInfo.UntypedBuildingBlock;
         // if (affectedBuildingBlock.IsAnImplementationOf<ParameterValuesBuildingBlock>())
         //    return synchronizeParameterStartValueCommand(quantity as IParameter, affectedBuildingBlock.DowncastTo<ParameterValuesBuildingBlock>());
         //
         // if (affectedBuildingBlock.IsAnImplementationOf<InitialConditionsBuildingBlock>())
         //    return synchronizeInitialConditionCommand(quantity, affectedBuildingBlock.DowncastTo<InitialConditionsBuildingBlock>());
         //
         // return synchronizeStructuralBuildingBlockCommand(quantity as IParameter, affectedBuildingBlock);
      }

      public IMoBiCommand Synchronize(IQuantity quantity, IMoBiSimulation simulation)
      {
         return SynchronizeCommand(quantity, simulation).AsHidden().Run(_context);
      }

      public void SynchronizeInitialConditions(IQuantity quantity, InitialConditionsBuildingBlock initialConditions)
      {
         synchronizeInitialConditionCommand(quantity, initialConditions, allowCreation: false).Run(_context);
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

      private IMoBiCommand synchronizeInitialConditionCommand(IQuantity quantity, InitialConditionsBuildingBlock initialConditionsBuildingBlock, bool allowCreation = true)
      {
         var moleculeAmount = quantity as MoleculeAmount ?? quantity.ParentContainer as MoleculeAmount;
         return synchronizePathAndValueEntityCommand(moleculeAmount, initialConditionsBuildingBlock, allowCreation,
            msv => new SynchronizeInitialConditionCommand(quantity, msv),
            () => new AddInitialConditionFromQuantityInSimulationCommand(moleculeAmount, initialConditionsBuildingBlock));
      }

      private IMoBiCommand synchronizeParameterValueCommand(IParameter parameter, ParameterValuesBuildingBlock parameterValuesBuildingBlock, bool allowCreation = true)
      {
         return synchronizePathAndValueEntityCommand(parameter, parameterValuesBuildingBlock, allowCreation,
            psv => new SynchronizeParameterValueCommand(parameter, psv),
            () => new AddParameterValueFromQuantityInSimulationCommand(parameter, parameterValuesBuildingBlock));
      }

      private IMoBiCommand synchronizePathAndValueEntityCommand<TPathAndValueEntity>(IQuantity quantity, PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock, bool allowCreation,
         Func<TPathAndValueEntity, IMoBiCommand> synchronizePathAndValueEntityCommandFunc, Func<IMoBiCommand> createPathAndValueEntityCommandFunc) where TPathAndValueEntity : PathAndValueEntity
      {
         if (quantity == null)
            return new MoBiEmptyCommand();

         var objectPath = _entityPathResolver.ObjectPathFor(quantity);
         var pathAndValueEntity = buildingBlock[objectPath];
         if (pathAndValueEntity != null)
            return synchronizePathAndValueEntityCommandFunc(pathAndValueEntity);

         if (allowCreation)
            return createPathAndValueEntityCommandFunc();

         return new MoBiEmptyCommand();
      }

      public void Visit(ReactionBuildingBlock reactionBuildingBlock)
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

      public void Visit(SpatialStructure spatialStructure)
      {
         synchronizeInParameters(spatialStructure);
      }

      public void Visit(MoleculeBuildingBlock moleculeBuildingBlock)
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
         _command = buildingBlockParameter != null ? new SynchronizeParameterValueToParameterValueCommand(_parameter, buildingBlockParameter) : (IMoBiCommand)new MoBiEmptyCommand();
         return _command;
      }
   }
}