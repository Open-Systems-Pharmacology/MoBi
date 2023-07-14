using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RenameObjectBaseCommand : ContextSpecification<RenameObjectBaseCommand>
   {
      protected IMoBiContext _context;
      protected IRenameInSimulationTask _renamingTask;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _renamingTask = A.Fake<IRenameInSimulationTask>();
         A.CallTo(() => _context.Resolve<IRenameInSimulationTask>()).Returns(_renamingTask);
         sut = new RenameObjectBaseCommand(GetObject(), "newName", GetBuildingBlock());
      }

      protected abstract IBuildingBlock GetBuildingBlock();

      protected abstract IObjectBase GetObject();
   }

   public class When_renaming_a_building_block : concern_for_RenameObjectBaseCommand
   {
      private SpatialStructure _buildingBlock;

      protected override void Context()
      {
         _buildingBlock = new SpatialStructure().WithName("oldName").WithId("id");
         base.Context();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      protected override IBuildingBlock GetBuildingBlock()
      {
         return null;
      }

      protected override IObjectBase GetObject()
      {
         return _buildingBlock;
      }

      [Observation]
      public void the_renaming_task_is_used_to_rename_the_module()
      {
         A.CallTo(() => _renamingTask.RenameInSimulationUsingTemplateBuildingBlock("oldName", _buildingBlock)).MustHaveHappened();
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

      protected override void Because()
      {
         sut.Execute(_context);
      }

      protected override IBuildingBlock GetBuildingBlock()
      {
         return null;
      }

      protected override IObjectBase GetObject()
      {
         return _module;
      }

      [Observation]
      public void the_renaming_task_is_used_to_rename_the_module()
      {
         A.CallTo(() => _renamingTask.RenameInSimulationUsingTemplateModule("oldName", _module)).MustHaveHappened();
      }
   }
}
