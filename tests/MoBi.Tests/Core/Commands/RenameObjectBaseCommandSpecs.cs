using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
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
}