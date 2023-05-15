using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;

namespace MoBi.Core.Domain.Repository
{
   public interface IBuildingBlockRepository : IRepository<IBuildingBlock>
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

      public IEnumerable<IBuildingBlock> All()
      {
         return IndividualsCollection
            .Concat<IBuildingBlock>(ExpressionProfileCollection)
            .Concat(moduleBuildingBlocks()).ToList();
      }

      private IEnumerable<IBuildingBlock> moduleBuildingBlocks()
      {
         return _projectRetriever.Current.Modules.SelectMany(x => x.BuildingBlocks);
      }

      private IReadOnlyList<T> get<T>()
      {
         return All().OfType<T>().ToList();
      }

      public IReadOnlyList<MoleculeBuildingBlock> MoleculeBlockCollection => get<MoleculeBuildingBlock>();

      public IReadOnlyList<MoBiReactionBuildingBlock> ReactionBlockCollection => get<MoBiReactionBuildingBlock>();

      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfileCollection => _projectRetriever.Current.ExpressionProfileCollection;

      public IReadOnlyList<IndividualBuildingBlock> IndividualsCollection => _projectRetriever.Current.IndividualsCollection;

      public IReadOnlyList<PassiveTransportBuildingBlock> PassiveTransportCollection => get<PassiveTransportBuildingBlock>();

      public IReadOnlyList<MoBiSpatialStructure> SpatialStructureCollection => get<MoBiSpatialStructure>();

      public IReadOnlyList<ObserverBuildingBlock> ObserverBlockCollection => get<ObserverBuildingBlock>();

      public IReadOnlyList<EventGroupBuildingBlock> EventBlockCollection => get<EventGroupBuildingBlock>();

      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionBlockCollection => get<InitialConditionsBuildingBlock>();

      public IReadOnlyList<ParameterValuesBuildingBlock> ParametersValueBlockCollection => get<ParameterValuesBuildingBlock>();

      public IndividualBuildingBlock IndividualByName(string buildingBlockName)
      {
         return IndividualsCollection.FindByName(buildingBlockName);
      }

      public ExpressionProfileBuildingBlock ExpressionProfileByName(string buildingBlockName)
      {
         return ExpressionProfileCollection.FindByName(buildingBlockName);
      }
   }
}