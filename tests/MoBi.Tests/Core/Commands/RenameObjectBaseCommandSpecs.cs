using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RenameObjectBaseCommand : ContextSpecification<RenameObjectBaseCommand>
   {
      protected IMoBiContext _context;
      protected ISimulationEntitySourceUpdater _simulationEntitySourceUpdater;
      protected IRenameInSimulationTask _renameInSimulationTask;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _simulationEntitySourceUpdater = A.Fake<ISimulationEntitySourceUpdater>();
         _renameInSimulationTask = A.Fake<IRenameInSimulationTask>();

         A.CallTo(() => _context.Resolve<ISimulationEntitySourceUpdater>()).Returns(_simulationEntitySourceUpdater);
         A.CallTo(() => _context.Resolve<IRenameInSimulationTask>()).Returns(_renameInSimulationTask);

         sut = new RenameObjectBaseCommand(GetObject(), "newName", GetBuildingBlock());
      }

      protected abstract IBuildingBlock GetBuildingBlock();

      protected abstract IObjectBase GetObject();
   }

   public class When_renaming_a_simulation : concern_for_RenameObjectBaseCommand
   {
      private IObjectBase _simulation;

      protected override void Context()
      {
         _simulation = new MoBiSimulation().WithName("originalName");
         base.Context();
         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(new EntityPathResolverForSpecs());
      }

      protected override void Because() => sut.Execute(_context);

      [Observation]
      public void the_rename_in_simulation_task_is_used_to_update_source_paths()
      {
         _simulation.Name.ShouldBeEqualTo("newName");
      }

      protected override IBuildingBlock GetBuildingBlock() => null;

      protected override IObjectBase GetObject() => _simulation;
   }

   public class When_renaming_a_parameter : concern_for_RenameObjectBaseCommand
   {
      private IBuildingBlock _spatialStructure;
      private IObjectBase _parameter;

      protected override void Context()
      {
         _spatialStructure = new SpatialStructure();
         _parameter = new Parameter().WithName("originalName");
         base.Context();


         A.CallTo(() => _context.Resolve<IEntityPathResolver>()).Returns(new EntityPathResolverForSpecs());
      }

      protected override void Because() => sut.Execute(_context);

      [Observation]
      public void the_rename_in_simulation_task_is_used_to_update_source_paths()
      {
         A.CallTo(() => _simulationEntitySourceUpdater.UpdateEntitySourcesForEntityRename(A<ObjectPath>.That.Matches(x => x.PathAsString.Equals("newName")), A<ObjectPath>.That.Matches(x => x.PathAsString.Equals("originalName")), _spatialStructure)).MustHaveHappened();
      }

      protected override IBuildingBlock GetBuildingBlock() => _spatialStructure;

      protected override IObjectBase GetObject() => _parameter;
   }

   public class When_renaming_a_building_block : concern_for_RenameObjectBaseCommand
   {
      private SpatialStructure _buildingBlock;

      protected override void Context()
      {
         _buildingBlock = new SpatialStructure().WithName("oldName").WithId("id");
         base.Context();
      }

      protected override void Because() => sut.Execute(_context);

      protected override IBuildingBlock GetBuildingBlock() => null;

      protected override IObjectBase GetObject() => _buildingBlock;

      [Observation]
      public void the_renaming_task_is_used_to_rename_the_module()
      {
         A.CallTo(() => _renameInSimulationTask.RenameInSimulationUsingTemplateBuildingBlock("oldName", _buildingBlock)).MustHaveHappened();
      }
   }

   public class When_renaming_a_module : concern_for_RenameObjectBaseCommand
   {
      private Module _module;

      protected override void Context()
      {
         _module = new Module().WithName("oldName").WithId("id");
         base.Context();
      }

      protected override void Because() => sut.Execute(_context);

      protected override IBuildingBlock GetBuildingBlock() => null;

      protected override IObjectBase GetObject() => _module;

      [Observation]
      public void the_renaming_task_is_used_to_rename_the_module()
      {
         A.CallTo(() => _renameInSimulationTask.RenameInSimulationUsingTemplateModule("oldName", _module)).MustHaveHappened();
      }
   }

   public class When_renaming_an_expression_profile : concern_for_RenameObjectBaseCommand
   {
      private ExpressionProfileBuildingBlock _expressionProfile;
      private IExpressionProfileRenamingTask _expressionProfileRenamingTask;

      protected override void Context()
      {
         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("OldMolecule|Species|Category").WithId("id");
         base.Context();
         _expressionProfileRenamingTask = A.Fake<IExpressionProfileRenamingTask>();
         A.CallTo(() => _context.Resolve<IExpressionProfileRenamingTask>()).Returns(_expressionProfileRenamingTask);
      }

      protected override void Because() => sut.Execute(_context);

      protected override IBuildingBlock GetBuildingBlock() => null;

      protected override IObjectBase GetObject() => _expressionProfile;

      [Observation]
      public void the_expression_profile_renaming_task_renames_the_building_block_and_updates_its_paths()
      {
         A.CallTo(() => _expressionProfileRenamingTask.Rename(_expressionProfile, "newName")).MustHaveHappened();
      }

      [Observation]
      public void the_rename_is_propagated_to_simulations()
      {
         A.CallTo(() => _renameInSimulationTask.RenameInSimulationUsingTemplateBuildingBlock("OldMolecule|Species|Category", _expressionProfile)).MustHaveHappened();
      }
   }
}