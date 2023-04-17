using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Model
{
   public class MoBiProject : Project
   {
      private readonly List<IBuildingBlock> _buildingBlocks;
      private readonly List<CurveChart> _charts;
      private readonly List<IMoBiSimulation> _allSimulations;
      private readonly List<Module> _modules = new List<Module>();

      public IReadOnlyList<Module> Modules => _modules;

      public string ChartSettings { get; set; }

      public override bool HasChanged { get; set; }

      public override IEnumerable<IUsesObservedData> AllUsersOfObservedData => AllParameterIdentifications.Cast<IUsesObservedData>().Union(Simulations);

      public ReactionDimensionMode ReactionDimensionMode { get; set; }

      public MoBiProject()
      {
         _charts = new List<CurveChart>();
         _buildingBlocks = new List<IBuildingBlock>();
         _allSimulations = new List<IMoBiSimulation>();
         ReactionDimensionMode = ReactionDimensionMode.AmountBased;
      }

      public override void AddParameterIdentification(ParameterIdentification parameterIdentification)
      {
         base.AddParameterIdentification(parameterIdentification);
         parameterIdentification.IsLoaded = true;
      }

      public override IReadOnlyCollection<T> All<T>()
      {
         return get<T>();
      }

      public IEnumerable<CurveChart> Charts => _charts;

      public bool IsEmpty => !_buildingBlocks.Any() && !_allSimulations.Any();

      public SimulationSettings SimulationSettings { get; set; }

      public void AddModule(Module module)
      {
         _modules.Add(module);
      }

      public void RemoveModule(Module module)
      {
         _modules.Remove(module);
      }

      public IReadOnlyList<IMoBiSimulation> Simulations => _allSimulations;

      private IReadOnlyList<T> get<T>()
      {
         return _buildingBlocks.OfType<T>().ToList();
      }

      public IReadOnlyList<MoleculeBuildingBlock> MoleculeBlockCollection => get<MoleculeBuildingBlock>();

      public IReadOnlyList<MoBiReactionBuildingBlock> ReactionBlockCollection => get<MoBiReactionBuildingBlock>();

      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfileCollection => get<ExpressionProfileBuildingBlock>();

      public IReadOnlyList<IndividualBuildingBlock> IndividualsCollection => get<IndividualBuildingBlock>();

      public IReadOnlyList<PassiveTransportBuildingBlock> PassiveTransportCollection => get<PassiveTransportBuildingBlock>();

      public IReadOnlyList<MoBiSpatialStructure> SpatialStructureCollection => get<MoBiSpatialStructure>();

      public IReadOnlyList<ObserverBuildingBlock> ObserverBlockCollection => get<ObserverBuildingBlock>();

      public IReadOnlyList<EventGroupBuildingBlock> EventBlockCollection => get<EventGroupBuildingBlock>();

      public IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValueBlockCollection => get<MoleculeStartValuesBuildingBlock>();

      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParametersStartValueBlockCollection => get<ParameterStartValuesBuildingBlock>();

      public void AddSimulation(IMoBiSimulation newSimulation)
      {
         _allSimulations.Add(newSimulation);
      }

      public void RemoveSimulation(IMoBiSimulation simulationToRemove)
      {
         _allSimulations.Remove(simulationToRemove);
         RemoveClassifiableForWrappedObject(simulationToRemove);
      }

      public void AddChart(CurveChart chart)
      {
         _charts.Add(chart);
      }

      public void RemoveChart(CurveChart chartToRemove)
      {
         _charts.Remove(chartToRemove);
      }

      public IReadOnlyList<IBuildingBlock> AllBuildingBlocks()
      {
         return _buildingBlocks;
      }

      public IBuildingBlock TemplateById(string templateBuildingBlockId)
      {
         return AllBuildingBlocks().FindById(templateBuildingBlockId);
      }

      public void AddBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlocks.Add(buildingBlock);
      }

      public void RemoveBuildingBlock(IBuildingBlock buildingBlockToRemove)
      {
         _buildingBlocks.Remove(buildingBlockToRemove);
         RemoveClassifiableForWrappedObject(buildingBlockToRemove);
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _buildingBlocks.Each(x => x.AcceptVisitor(visitor));
         _allSimulations.Each(x => x.AcceptVisitor(visitor));
         _charts.Each(x => x.AcceptVisitor(visitor));
      }

      public IReadOnlyList<IBuildingBlock> ReferringStartValuesBuildingBlocks(IBuildingBlock buildingBlockToRemove)
      {
         return referringStartValuesBuildingBlocks(buildingBlockToRemove, MoleculeStartValueBlockCollection).ToList();
      }

      public IReadOnlyList<IMoBiSimulation> SimulationsCreatedUsing(IBuildingBlock templateBuildingBlock)
      {
         return Simulations.Where(simulation => simulation.IsCreatedBy(templateBuildingBlock)).ToList();
      }

      public IEnumerable<IObjectBase> All()
      {
         return All<IObjectBase>().Union(Simulations);
      }

      private IEnumerable<IBuildingBlock> referringStartValuesBuildingBlocks(IBuildingBlock buildingBlockToRemove, IReadOnlyList<MoleculeStartValuesBuildingBlock> buildingBlockCollection)
      {
         return buildingBlockCollection
            .Where(msvBB => msvBB.Uses(buildingBlockToRemove))
            .OfType<IBuildingBlock>()
            .ToList();
      }
   }
}