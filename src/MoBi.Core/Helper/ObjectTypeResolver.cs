using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Assets;
using OSPSuite.Assets.Extensions;

namespace MoBi.Core.Helper
{
   public class ObjectTypeResolver : IObjectTypeResolver
   {
      private readonly Cache<Type, string> _typeCache;

      public ObjectTypeResolver()
      {
         _typeCache = new Cache<Type, string>();
         addToCache<MoleculeBuildingBlock>(ObjectTypes.MoleculeBuildingBlock);
         addToCache<ReactionBuildingBlock>(ObjectTypes.ReactionBuildingBlock);
         addToCache<NeighborhoodBuilder>(ObjectTypes.Neighborhood);
         addToCache<Neighborhood>(ObjectTypes.Neighborhood);
         addToCache<SpatialStructure>(ObjectTypes.SpatialStructure);
         addToCache<PassiveTransportBuildingBlock>(ObjectTypes.PassiveTransportBuildingBlock);
         addToCache<ObserverBuildingBlock>(ObjectTypes.ObserverBuildingBlock);
         addToCache<EventGroupBuildingBlock>(ObjectTypes.EventGroupBuildingBlock);
         addToCache<InitialConditionsBuildingBlock>(ObjectTypes.InitialConditionsBuildingBlock);
         addToCache<ParameterValuesBuildingBlock>(ObjectTypes.ParameterValuesBuildingBlock);
         addToCache<MoleculeBuilder>(ObjectTypes.Molecule);
         addToCache<TransporterMoleculeContainer>(ObjectTypes.TransporterMoleculeContainer);
         addToCache<TransportBuilder>(ObjectTypes.TransportBuilder);
         addToCache<Transport>(ObjectTypes.Transport);
         addToCache<IDistributedParameter>(ObjectTypes.DistributedParameter);
         addToCache<IParameter>(ObjectTypes.Parameter);
         addToCache<ReactionBuilder>(ObjectTypes.Reaction);
         addToCache<AmountObserverBuilder>(ObjectTypes.AmountObserverBuilder);
         addToCache<ContainerObserverBuilder>(ObjectTypes.ContainerObserverBuilder);
         addToCache<ApplicationBuilder>(ObjectTypes.Application);
         addToCache<EventGroupBuilder>(ObjectTypes.EventGroupBuilder);
         addToCache<EventBuilder>(ObjectTypes.EventBuilder);
         addToCache<EventAssignmentBuilder>(ObjectTypes.EventAssignmentBuilder);
         addToCache<ApplicationMoleculeBuilder>(ObjectTypes.ApplicationMoleculeBuilder);
         addToCache<IFormula>(ObjectTypes.Formula);
         addToCache<ReactionPartnerBuilder>(ObjectTypes.ReactionPartnerBuilder);
         addToCache<ConstantFormula>(ObjectTypes.ConstantFormula);
         addToCache<ExplicitFormula>(ObjectTypes.ExplicitFormula);
         addToCache<BlackBoxFormula>(ObjectTypes.BlackBoxFormula);
         addToCache<IMoBiSimulation>(ObjectTypes.Simulation);
         addToCache<DataRepository>(ObjectTypes.ObservedData);
         addToCache<SimulationSettings>(ObjectTypes.SimulationSettings);
         addToCache<CurveChartTemplate>(ObjectTypes.ChartTemplate);
         addToCache<InitialCondition>(ObjectTypes.InitialCondition);
         addToCache<ParameterValue>(ObjectTypes.ParameterValue);
         addToCache<ObserverBuilder>(ObjectTypes.ObserverBuilder);
         addToCache<TimePath>(ObjectTypes.Reference);
         addToCache<ObjectPath>(ObjectTypes.Reference);
         addToCache<ObjectReference>(ObjectTypes.Reference);
         addToCache<IMoBiHistoryManager>(ObjectTypes.History);
         addToCache<MoBiProject>(ObjectTypes.Project);
         addToCache<IDiagramModel>(ObjectTypes.DiagramModel);
         addToCache<IContainer>(ObjectTypes.Container);
         addToCache<FormulaUsablePath>(ObjectTypes.FormulaUsablePath);
         addToCache<ParameterIdentificationCovarianceMatrix>(Captions.ParameterIdentification.CovarianceMatrix);
         addToCache<ParameterIdentificationCorrelationMatrix>(Captions.ParameterIdentification.CorrelationMatrix);
         addToCache<ParameterIdentificationResidualHistogram>(Captions.ParameterIdentification.ResidualHistogramAnalysis);
         addToCache<ParameterIdentificationResidualVsTimeChart>(Captions.ParameterIdentification.ResidualsVsTimeAnalysis);
         addToCache<ParameterIdentificationTimeProfileConfidenceIntervalChart>(Captions.ParameterIdentification.TimeProfileConfidenceIntervalAnalysis);
         addToCache<ParameterIdentificationTimeProfilePredictionIntervalChart>(Captions.ParameterIdentification.TimeProfilePredictionIntervalAnalysis);
         addToCache<ParameterIdentificationTimeProfileVPCIntervalChart>(Captions.ParameterIdentification.TimeProfileVPCIntervalAnalysis);
         addToCache<ParameterIdentificationTimeProfileChart>(Captions.ParameterIdentification.TimeProfileAnalysis);
         addToCache<ParameterIdentificationPredictedVsObservedChart>(Captions.ParameterIdentification.PredictedVsObservedAnalysis);
         addToCache<ParameterIdentification>(ObjectTypes.ParameterIdentification);
      }

      private void addToCache<T>(string value)
      {
         _typeCache.Add(typeof(T), value);
      }

      public string TypeFor<T>(T item) where T : class
      {
         if (item == null)
            return TypeFor<T>();

         if (item.IsAnImplementationOf<IEnumerable<DataRepository>>())
            return AppConstants.Captions.ListOf(ObjectTypes.ObservedData);

         if (item.IsAnImplementationOf<IEnumerable<IBuildingBlock>>())
         {
            var col = item.DowncastTo<IEnumerable<IBuildingBlock>>().ToList();
            if (col.Any())
               return AppConstants.Captions.ListOf(TypeFor(col.First()).Pluralize());
         }


         return typeFor(item.GetType());
      }

      public string TypeFor<T>()
      {
         return typeFor(typeof (T));
      }

      private string typeFor(Type type)
      {
         if (_typeCache.Contains(type))
            return _typeCache[type];

         var firstPossibleType = _typeCache.Keys.FirstOrDefault(type.IsAnImplementationOf);
         if (firstPossibleType != null)
            _typeCache[type] = _typeCache[firstPossibleType];
         else
            _typeCache[type] = type.Name.SplitToUpperCase().Replace("Mo Bi", "MoBi");

         return _typeCache[type];
      }
   }
}