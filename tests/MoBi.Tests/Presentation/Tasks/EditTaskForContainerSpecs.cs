using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTaskForContainer : ContextSpecification<EditTaskForContainer>
   {
      protected ISpatialStructureContentExporter _spatialStructureContentExporter;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IInteractionTask _interactionTask;

      protected override void Context()
      {
         base.Context();
         _spatialStructureContentExporter = A.Fake<ISpatialStructureContentExporter>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _interactionTask = _interactionTaskContext.InteractionTask;
         sut = new EditTaskForContainer(_interactionTaskContext, _spatialStructureContentExporter);
      }

      protected bool containsEvents(List<string> prohibitedNames)
      {
         return prohibitedNames.Any(x => string.Compare(x, "Events", StringComparison.InvariantCultureIgnoreCase) == 0);
      }
   }

   public class When_renaming_a_container_that_is_not_in_a_spatial_structure : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private EventGroupBuildingBlock _eventBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _eventBuildingBlock = new EventGroupBuildingBlock();
         A.CallTo(_interactionTaskContext.NamingTask).WithReturnType<string>().Returns("NEW");

         A.CallTo(() => _interactionTaskContext.Active<SpatialStructure>()).Returns(null);
      }

      protected override void Because()
      {
         sut.Rename(_container, _eventBuildingBlock);
      }

      [Observation]
      public void the_forbidden_names_should_include_events()
      {
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(_container, A<IReadOnlyList<string>>._)).WhenArgumentsMatch(x => containsEvents(x.Get<IReadOnlyList<string>>(1).ToList())).MustHaveHappened();
      }

      [Observation]
      public void should_be_able_to_rename_the_container()
      {
         _container.Name.ShouldBeEqualTo("NEW");
      }
   }

   public class When_exporting_container_only : concern_for_EditTaskForContainer
   {
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
      }

      protected override void Because()
      {
         sut.Save(_container);
      }

      [Observation]
      public void the_export_task_should_be_used()
      {
         A.CallTo(() => _spatialStructureContentExporter.Save(_container)).MustHaveHappened();
      }
   }

   public class When_exporting_container_with_individual_and_expression : concern_for_EditTaskForContainer
   {
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
      }

      protected override void Because()
      {
         sut.SaveWithIndividualAndExpression(_container);
      }

      [Observation]
      public void the_export_task_should_be_used()
      {
         A.CallTo(() => _spatialStructureContentExporter.SaveWithIndividualAndExpression(_container)).MustHaveHappened();
      }
   }

   public class When_renaming_a_container_to_events_that_is_a_top_container : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private SpatialStructure _spatialStructure;
      private Module _module;
      private IBuildingBlock _moleculesBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _spatialStructure = new SpatialStructure();
         _module = new Module().WithName("Module");
         _moleculesBuildingBlock = new MoleculeBuildingBlock();
         _module.Add(_spatialStructure);
         _module.Add(_moleculesBuildingBlock);
         _spatialStructure.AddTopContainer(_container);

         A.CallTo(() => _interactionTaskContext.Active<SpatialStructure>()).Returns(_spatialStructure);
      }

      protected override void Because()
      {
         sut.Rename(_container, _spatialStructure);
      }

      [Observation]
      public void the_forbidden_names_should_not_include_events()
      {
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(_container, A<IReadOnlyList<string>>._)).WhenArgumentsMatch(x => !containsEvents(x.Get<IReadOnlyList<string>>(1).ToList())).MustHaveHappened();
      }
   }

   public class When_renaming_a_container_to_events_that_is_not_a_top_container : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private SpatialStructure _spatialStructure;
      private Module _module;
      private IBuildingBlock _moleculesBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _spatialStructure = new SpatialStructure();
         _module = new Module().WithName("Module");
         _moleculesBuildingBlock = new MoleculeBuildingBlock();
         _module.Add(_spatialStructure);
         _module.Add(_moleculesBuildingBlock);
         _spatialStructure.Add(new Container { _container });

         A.CallTo(() => _interactionTaskContext.Active<SpatialStructure>()).Returns(_spatialStructure);
      }

      protected override void Because()
      {
         sut.Rename(_container, _spatialStructure);
      }

      [Observation]
      public void the_forbidden_names_should_include_events()
      {
         A.CallTo(() => _interactionTaskContext.NamingTask.RenameFor(_container, A<IReadOnlyList<string>>._)).WhenArgumentsMatch(x => containsEvents(x.Get<IReadOnlyList<string>>(1).ToList())).MustHaveHappened();
      }

      private bool containsEvents(List<string> prohibitedNames)
      {
         // check for instances of "Events" with any case
         return prohibitedNames.Any(x => string.Compare(x, "Events", StringComparison.InvariantCultureIgnoreCase) == 0);
      }
   }

   public class When_renaming_a_container_that_is_in_a_spatial_structure : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private SpatialStructure _spatialStructure;
      private IMoBiCommand _renameCommand;
      private Module _module;
      private IBuildingBlock _moleculesBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _spatialStructure = new SpatialStructure();
         _module = new Module().WithName("Module");
         _moleculesBuildingBlock = new MoleculeBuildingBlock();
         _module.Add(_spatialStructure);
         _module.Add(_moleculesBuildingBlock);
         A.CallTo(_interactionTaskContext.NamingTask).WithReturnType<string>().Returns("NEW");

         A.CallTo(() => _interactionTaskContext.Context.AddToHistory(A<IMoBiCommand>._))
            .Invokes(x => _renameCommand = x.GetArgument<IMoBiCommand>(0));
      }

      protected override void Because()
      {
         sut.Rename(_container, _spatialStructure);
      }

      [Observation]
      public void the_check_name_visitor_should_be_used_to_find_related_renames_from_the_module()
      {
         A.CallTo(() => _interactionTaskContext.CheckNamesVisitor.GetPossibleChangesFrom(_container, "NEW", _spatialStructure, "OLD")).MustHaveHappened();
         A.CallTo(() => _interactionTaskContext.CheckNamesVisitor.GetPossibleChangesFrom(_container, "NEW", _moleculesBuildingBlock, "OLD")).MustHaveHappened();
      }

      [Observation]
      public void should_be_able_to_rename_the_container()
      {
         _container.Name.ShouldBeEqualTo("NEW");
      }

      [Observation]
      public void should_have_used_a_rename_container_command_specifically()
      {
         //at least one command with the special case command
         _renameCommand.DowncastTo<MoBiMacroCommand>().All().Any(x => x.IsAnImplementationOf<RenameContainerCommand>()).ShouldBeTrue();
      }
   }

   public class When_saving_multiple_container_to_pkml : concern_for_EditTaskForContainer
   {
      private List<IContainer> _entitiesToSave;

      protected override void Context()
      {
         base.Context();
         _entitiesToSave = new List<IContainer> { new Container().WithName("Container1") };
         A.CallTo(_interactionTask).WithReturnType<string>().Returns("FilePath");
      }

      protected override void Because()
      {
         sut.SaveMultiple(_entitiesToSave);
      }

      [Observation]
      public void should_have_called_the_save_multiple_on_interactionTask()
      {
         A.CallTo(() => _interactionTask.Save(_entitiesToSave)).MustHaveHappened();
      }
   }
}