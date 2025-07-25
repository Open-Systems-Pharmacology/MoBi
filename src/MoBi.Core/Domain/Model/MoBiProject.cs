using System.Collections.Generic;
using System.Linq;
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

      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfileCollection => get<ExpressionProfileBuildingBlock>();

      public IReadOnlyList<IndividualBuildingBlock> IndividualsCollection => get<IndividualBuildingBlock>();

      public Module ModuleByName(string moduleName)
      {
         return Modules.FindByName(moduleName);
      }

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

      public void AddIndividualBuildingBlock(IndividualBuildingBlock individualBuildingBlock)
      {
         addBuildingBlock(individualBuildingBlock);
      }

      public void AddExpressionProfileBuildingBlock(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         addBuildingBlock(expressionProfileBuildingBlock);
      }

      private void addBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlocks.Add(buildingBlock);
      }

      public void RemoveIndividualBuildingBlock(IndividualBuildingBlock buildingBlockToRemove)
      {
         removeBuildingBlock(buildingBlockToRemove);
      }

      public void RemoveExpressionProfileBuildingBlock(ExpressionProfileBuildingBlock buildingBlockToRemove)
      {
         removeBuildingBlock(buildingBlockToRemove);
      }

      private void removeBuildingBlock(IBuildingBlock buildingBlockToRemove)
      {
         _buildingBlocks.Remove(buildingBlockToRemove);
         RemoveClassifiableForWrappedObject(buildingBlockToRemove);
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _modules.Each(x => x.AcceptVisitor(visitor));
         _buildingBlocks.Each(x => x.AcceptVisitor(visitor));
         _allSimulations.Each(x => x.AcceptVisitor(visitor));
         _charts.Each(x => x.AcceptVisitor(visitor));
      }

      public IReadOnlyList<IMoBiSimulation> SimulationsUsing(IBuildingBlock templateBuildingBlock)
      {
         return Simulations.Where(simulation => simulation.Uses(templateBuildingBlock)).ToList();
      }

      public IEnumerable<IObjectBase> All()
      {
         return All<IObjectBase>().Union(Simulations);
      }

      /// <summary>
      /// Returns a list of simulations that have a module where the name matches <paramref name="module"/>
      /// This indicates that the <paramref name="module"/> was used as a template for the simulation
      /// </summary>
      public IReadOnlyList<IMoBiSimulation> SimulationsUsing(Module module)
      {
         return Simulations.Where(simulation => simulation.Uses(module)).ToList();
      }
   }
}