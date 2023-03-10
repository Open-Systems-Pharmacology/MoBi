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
         addToCache<IMoleculeBuildingBlock>(ObjectTypes.MoleculeBuildingBlock);
         addToCache<IReactionBuildingBlock>(ObjectTypes.ReactionBuildingBlock);
         addToCache<INeighborhoodBuilder>(ObjectTypes.NeighborhoodBuilder);
         addToCache<INeighborhood>(ObjectTypes.NeighborhoodBuilder);
         addToCache<ISpatialStructure>(ObjectTypes.SpatialStructure);
         addToCache<IPassiveTransportBuildingBlock>(ObjectTypes.PassiveTransportBuildingBlock);
         addToCache<IObserverBuildingBlock>(ObjectTypes.ObserverBuildingBlock);
         addToCache<IEventGroupBuildingBlock>(ObjectTypes.EventGroupBuildingBlock);
         addToCache<IMoleculeStartValuesBuildingBlock>(ObjectTypes.MoleculeStartValuesBuildingBlock);
         addToCache<IParameterStartValuesBuildingBlock>(ObjectTypes.ParameterStartValuesBuildingBlock);
         addToCache<IMoleculeBuilder>(ObjectTypes.Molecule);
         addToCache<TransporterMoleculeContainer>(ObjectTypes.TransporterMoleculeContainer);
         addToCache<ITransportBuilder>(ObjectTypes.TransportBuilder);
         addToCache<ITransport>(ObjectTypes.Transport);
         addToCache<IDistributedParameter>(ObjectTypes.DistributedParameter);
         addToCache<IParameter>(ObjectTypes.Parameter);
         addToCache<IReactionBuilder>(ObjectTypes.Reaction);
         addToCache<IAmountObserverBuilder>(ObjectTypes.AmountObserverBuilder);
         addToCache<IContainerObserverBuilder>(ObjectTypes.ContainerObserverBuilder);
         addToCache<IApplicationBuilder>(ObjectTypes.Application);
         addToCache<IEventGroupBuilder>(ObjectTypes.EventGroupBuilder);
         addToCache<IEventBuilder>(ObjectTypes.EventBuilder);
         addToCache<IEventAssignmentBuilder>(ObjectTypes.EventAssignmentBuilder);
         addToCache<IApplicationMoleculeBuilder>(ObjectTypes.ApplicationMoleculeBuilder);
         addToCache<IFormula>(ObjectTypes.Formula);
         addToCache<IReactionPartnerBuilder>(ObjectTypes.ReactionPartnerBuilder);
         addToCache<ConstantFormula>(ObjectTypes.ConstantFormula);
         addToCache<ExplicitFormula>(ObjectTypes.ExplicitFormula);
         addToCache<BlackBoxFormula>(ObjectTypes.BlackBoxFormula);
         addToCache<IMoBiSimulation>(ObjectTypes.Simulation);
         addToCache<DataRepository>(ObjectTypes.ObservedData);
         addToCache<ISimulationSettings>(ObjectTypes.SimulationSettings);
         addToCache<CurveChartTemplate>(ObjectTypes.ChartTemplate);
         addToCache<MoleculeStartValue>(ObjectTypes.MoleculeStartValue);
         addToCache<ParameterStartValue>(ObjectTypes.ParameterStartValue);
         addToCache<IObserverBuilder>(ObjectTypes.ObserverBuilder);
         addToCache<TimePath>(ObjectTypes.Reference);
         addToCache<ObjectPath>(ObjectTypes.Reference);
         addToCache<ObjectReference>(ObjectTypes.Reference);
         addToCache<IMoBiHistoryManager>(ObjectTypes.History);
         addToCache<IMoBiProject>(ObjectTypes.Project);
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