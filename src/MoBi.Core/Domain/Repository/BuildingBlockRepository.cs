using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Repository
{
   public interface IBuildingBlockRepository : OSPSuite.Core.Domain.Builder.IBuildingBlockRepository
   {
      IReadOnlyList<MoleculeBuildingBlock> MoleculeBlockCollection { get; }
      IReadOnlyList<MoBiReactionBuildingBlock> ReactionBlockCollection { get; }
      IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfileCollection { get; }
      IReadOnlyList<IndividualBuildingBlock> IndividualsCollection { get; }
      IReadOnlyList<PassiveTransportBuildingBlock> PassiveTransportCollection { get; }
      IReadOnlyList<MoBiSpatialStructure> SpatialStructureCollection { get; }
      IReadOnlyList<ObserverBuildingBlock> ObserverBlockCollection { get; }
      IReadOnlyList<EventGroupBuildingBlock> EventBlockCollection { get; }
      IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionBlockCollection { get; }
      IReadOnlyList<ParameterValuesBuildingBlock> ParametersValueBlockCollection { get; }
      IndividualBuildingBlock IndividualByName(string buildingBlockName);
      ExpressionProfileBuildingBlock ExpressionProfileByName(string buildingBlockName);
   }

   public class BuildingBlockRepository : IBuildingBlockRepository
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public BuildingBlockRepository(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IReadOnlyList<IBuildingBlock> All()
      {
         return IndividualsCollection
            .Concat<IBuildingBlock>(ExpressionProfileCollection)
            .Concat(moduleBuildingBlocks()).ToList();
      }

      public IReadOnlyList<T> All<T>() where T : IBuildingBlock => All().OfType<T>().ToList();

      private IEnumerable<IBuildingBlock> moduleBuildingBlocks() => _projectRetriever.Current.Modules.SelectMany(x => x.BuildingBlocks);

      private IReadOnlyList<T> get<T>(Func<Module, T> getter) => _projectRetriever.Current.Modules.Select(getter).Where(x => x != null).ToList();

      private IReadOnlyList<T> getMany<T>(Func<Module, IEnumerable<T>> getter) => _projectRetriever.Current.Modules.SelectMany(getter).Where(x => x != null).ToList();

      public IReadOnlyList<MoleculeBuildingBlock> MoleculeBlockCollection => get(x => x.Molecules);

      public IReadOnlyList<MoBiReactionBuildingBlock> ReactionBlockCollection => get(x => x.Reactions as MoBiReactionBuildingBlock);

      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfileCollection => _projectRetriever.Current.ExpressionProfileCollection;

      public IReadOnlyList<IndividualBuildingBlock> IndividualsCollection => _projectRetriever.Current.IndividualsCollection;

      public IReadOnlyList<PassiveTransportBuildingBlock> PassiveTransportCollection => get(x => x.PassiveTransports);

      public IReadOnlyList<MoBiSpatialStructure> SpatialStructureCollection => get(x => x.SpatialStructure as MoBiSpatialStructure);

      public IReadOnlyList<ObserverBuildingBlock> ObserverBlockCollection => get(x => x.Observers);

      public IReadOnlyList<EventGroupBuildingBlock> EventBlockCollection => get(x => x.EventGroups);

      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionBlockCollection => getMany(x => x.InitialConditionsCollection);

      public IReadOnlyList<ParameterValuesBuildingBlock> ParametersValueBlockCollection => getMany(x => x.ParameterValuesCollection);

      public IndividualBuildingBlock IndividualByName(string buildingBlockName) => IndividualsCollection.FindByName(buildingBlockName);

      public ExpressionProfileBuildingBlock ExpressionProfileByName(string buildingBlockName) => ExpressionProfileCollection.FindByName(buildingBlockName);
   }
}