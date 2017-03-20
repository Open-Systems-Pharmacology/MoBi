using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Model
{
   public interface IAffectedBuildingBlockRetriever
   {
      IBuildingBlockInfo RetrieveFor(object changedObject, IMoBiSimulation affectedSimulation);
   }

   internal class AffectedBuildingBlockRetriever : IAffectedBuildingBlockRetriever
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public AffectedBuildingBlockRetriever(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public IBuildingBlockInfo RetrieveFor(object changedObject, IMoBiSimulation affectedSimulation)
      {
         var affectedBuildConfiguration = affectedSimulation.MoBiBuildConfiguration;

         var changedQuantity = changedObject as IQuantity;
         if (changedQuantity != null)
            return retrieveForQuantity(affectedBuildConfiguration, changedQuantity);

         //special case that do not fall into the generic quantity pattern.
         if (belongsToSimulationSettings(changedObject))
            return affectedBuildConfiguration.SimulationSettingsInfo;

         if (belongsToMoleculeStartValues(changedObject))
            return affectedBuildConfiguration.MoleculeStartValuesInfo;

         return null;
      }

      private static bool belongsToMoleculeStartValues(object changedObject)
      {
         var possibleType = new[] {typeof (ScaleDivisor), typeof (IEnumerable<ScaleDivisor>)};
         return possibleType.Any(changedObject.IsAnImplementationOf);
      }

      private static bool belongsToSimulationSettings(object changedObject)
      {
         var possibleType = new[]
         {
            typeof (OutputSelections), typeof (CurveChartTemplate), typeof (SolverSettings),
            typeof (OutputSchema), typeof (OutputInterval)
         };
         return possibleType.Any(changedObject.IsAnImplementationOf);
      }

      private IBuildingBlockInfo retrieveForQuantity(IMoBiBuildConfiguration buildConfiguration, IQuantity changedQuantity)
      {
         if (isMoleculeStartValue(changedQuantity, buildConfiguration))
            return buildConfiguration.MoleculeStartValuesInfo;

         if (isObserver(changedQuantity, buildConfiguration))
            return buildConfiguration.ObserversInfo;

         var parameter = changedQuantity as IParameter;
         if (parameter == null)
            return null;

         if (isInSimulationSettings(parameter, buildConfiguration))
            return buildConfiguration.SimulationSettingsInfo;

         if (isParameterStartValue(parameter, buildConfiguration))
            return buildConfiguration.ParameterStartValuesInfo;

         if (isInReaction(parameter, buildConfiguration))
            return modeDependentInfo(parameter, buildConfiguration.ReactionsInfo, buildConfiguration.ParameterStartValuesInfo);

         if (isInPassiveTransport(parameter, buildConfiguration))
            return buildConfiguration.ParameterStartValuesInfo;

         //this step needs to be performed AFTER isParameterStartValue
         if (isInMolecule(parameter, buildConfiguration))
            return modeDependentInfo(parameter, buildConfiguration.MoleculesInfo, buildConfiguration.ParameterStartValuesInfo);

         if (isInMoleculeProperties(parameter, buildConfiguration))
            return buildConfiguration.ParameterStartValuesInfo;

         //this needs to be performed AFTER isMolecule
         if (isInEventGroup(parameter, buildConfiguration))
            return buildConfiguration.ParameterStartValuesInfo;

         //this should always be last
         return buildConfiguration.SpatialStructureInfo;
      }

      private IBuildingBlockInfo modeDependentInfo(IParameter parameter, IBuildingBlockInfo defautBuildingBlockInfo, IBuildingBlockInfo parameterStartValuesInfo)
      {
         return parameter.BuildMode == ParameterBuildMode.Local ? parameterStartValuesInfo : defautBuildingBlockInfo;
      }

      private bool isInEventGroup(IQuantity changedQuantity, IMoBiBuildConfiguration buildConfiguration)
      {
         IEntity entity = changedQuantity;
         while (entity != null)
         {
            if (entity.IsAnImplementationOf<IEvent>() || entity.IsAnImplementationOf<IEventGroup>())
               return true;

            entity = entity.ParentContainer;
         }
         return false;
      }

      private bool isObserver(IQuantity changedQuantity, IMoBiBuildConfiguration buildConfiguration)
      {
         return changedQuantity.IsAnImplementationOf<IObserver>();
      }

      private bool isInSimulationSettings(IParameter changedParameter, IMoBiBuildConfiguration buildConfiguration)
      {
         return changedParameter.ParentContainer.IsAnImplementationOf<OutputInterval>();
      }

      private bool isMoleculeStartValue(IQuantity changedQuantity, IMoBiBuildConfiguration buildConfiguration)
      {
         return changedQuantity.IsAnImplementationOf<IMoleculeAmount>() ||
                (changedQuantity.ParentContainer.IsAnImplementationOf<IMoleculeAmount>() && changedQuantity.IsNamed(Constants.Parameters.START_VALUE));
      }

      private bool isParameterStartValue(IQuantity changedQuantity, IMoBiBuildConfiguration buildConfiguration)
      {
         if (!changedQuantity.IsAnImplementationOf<IParameter>())
            return false;

         var parameterStartValues = buildConfiguration.ParameterStartValues;
         var path = _entityPathResolver.ObjectPathFor(changedQuantity);
         return parameterStartValues.Any(psv => Equals(psv.Path, path));
      }

      private bool isInPassiveTransport(IParameter changedParameter, IMoBiBuildConfiguration buildConfiguration)
      {
         return quantityIsIn<ITransport, ITransportBuilder>(changedParameter, buildConfiguration.PassiveTransports);
      }

      private bool isInReaction(IParameter changedParameter, IMoBiBuildConfiguration buildConfiguration)
      {
         return quantityIsIn<IReaction, IReactionBuilder>(changedParameter, buildConfiguration.Reactions);
      }

      private bool quantityIsIn<TParentType, TBuilderType>(IQuantity changedQuantity, IEnumerable<TBuilderType> buildingBlock)
         where TParentType : class, IObjectBase where TBuilderType : IObjectBase

      {
         if (buildingBlock == null)
            return false;

         var parentContainer = changedQuantity.ParentContainer;
         if (parentContainer == null)
            return false;

         //either under the expected type or it is a global parameter
         return parentContainer.IsAnImplementationOf<TParentType>() || buildingBlock.AllNames().Contains(parentContainer.Name);
      }

      private bool isInMolecule(IParameter changedParameter, IMoBiBuildConfiguration buildConfiguration)
      {
         if (changedParameter.ParentContainer == null)
            return false;

         var molecules = buildConfiguration.Molecules;

         var transporterMoleculeContainers = molecules.SelectMany(m => m.TransporterMoleculeContainerCollection).ToList();
         if (transporterMoleculeContainers.Any())
         {
            var allTransportName = transporterMoleculeContainers.Select(t => t.TransportName);
            if (allTransportName.Contains(changedParameter.ParentContainer.Name))
               return true;
         }

         var moleculeBuilder = molecules[changedParameter.ParentContainer.Name];
         if (moleculeBuilder == null)
            return false;

         //Parameter may be defined in the spatial structure. Hence we need to ensure that
         //the molecule builder has a parameter with the same name
         return moleculeBuilder.Parameters.ExistsByName(changedParameter.Name);
      }

      private bool isInMoleculeProperties(IParameter changedParameter, IMoBiBuildConfiguration buildConfiguration)
      {
         if (changedParameter.ParentContainer == null)
            return false;

         var molecules = buildConfiguration.Molecules;
         var moleculeBuilder = molecules[changedParameter.ParentContainer.Name];

         if (moleculeBuilder == null)
            return false;

         //Parameter should not be defined in molecule builder 
         return !moleculeBuilder.Parameters.ExistsByName(changedParameter.Name);
      }
   }
}