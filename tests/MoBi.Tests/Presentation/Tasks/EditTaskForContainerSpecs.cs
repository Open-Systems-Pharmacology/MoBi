using System.Linq;
using DevExpress.Utils.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.ToolTips;
using Container = OSPSuite.Core.Domain.Container;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTaskForContainer : ContextSpecification<EditTaskForContainer>
   {
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IInteractionTask _interactionTask;
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _interactionTask = A.Fake<IInteractionTask>();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         sut = new EditTaskForContainer(_interactionTaskContext, _spatialStructureFactory, _objectPathFactory);
      }
   }

   public class When_saving_an_entity_to_pkml_but_the_file_dialog_is_canceled : concern_for_EditTaskForContainer
   {
      private IContainer _entityToSave;

      protected override void Context()
      {
         base.Context();
         _entityToSave = new Container();
         A.CallTo(() => _interactionTask.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void A_call_to_interaction_task_saving_the_container_must_not_have_happened()
      {
         A.CallTo(() => _spatialStructureFactory.Create()).MustNotHaveHappened();
      }
   }

   public class When_saving_a_container_to_pkml : concern_for_EditTaskForContainer
   {
      private IContainer _entityToSave;
      private Container _parentContainer;
      private MoBiSpatialStructure _tmpSpatialStructure;
      private MoBiSpatialStructure _activeSpatialStructure;
      private NeighborhoodBuilder _neighborhood1;
      private NeighborhoodBuilder _neighborhood2;

      protected override void Context()
      {
         base.Context();
         _parentContainer = new Container().WithName("Parent");
         _entityToSave = new Container().WithName("Container").WithMode(ContainerMode.Physical).Under(_parentContainer);
         //this is a spatial structure that will be created on the fly to export the containers and neighborhoods to the DB
         _tmpSpatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };

         //this is the spatial structure that is active at the moment and that will be used to find all neighborhoods to export
         _activeSpatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };

         _neighborhood1 = new NeighborhoodBuilder().WithName("_neighborhood1");
         _neighborhood2 = new NeighborhoodBuilder().WithName("_neighborhood2");

         _neighborhood1.FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_entityToSave);

         _activeSpatialStructure.AddNeighborhood(_neighborhood1);
         _activeSpatialStructure.AddNeighborhood(_neighborhood2);
         A.CallTo(_interactionTask).WithReturnType<string>().Returns("FilePath");
         A.CallTo(() => _spatialStructureFactory.Create()).Returns(_tmpSpatialStructure);
         A.CallTo(() => _interactionTaskContext.Active<MoBiSpatialStructure>()).Returns(_activeSpatialStructure);
      }

      protected override void Because()
      {
         sut.Save(_entityToSave);
      }

      [Observation]
      public void should_have_kept_the_reference_to_the_parent_container()
      {
         _entityToSave.ParentContainer.ShouldBeEqualTo(_parentContainer);
      }

      [Observation]
      public void should_have_exported_all_neighborhoods_connected_to_the_container_or_one_of_its_sub_containers()
      {
         _tmpSpatialStructure.Neighborhoods.ShouldContain(_neighborhood1);
         _tmpSpatialStructure.Neighborhoods.ShouldNotContain(_neighborhood2);
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
      }

      protected override void Because()
      {
         sut.Rename(_container, _eventBuildingBlock);
      }

      [Observation]
      public void should_be_able_to_rename_the_container()
      {
         _container.Name.ShouldBeEqualTo("NEW");
      }
   }

   public class When_renaming_a_container_that_is_in_a_spatial_structure : concern_for_EditTaskForContainer
   {
      private IContainer _container;
      private SpatialStructure _spatialStructure;
      private IMoBiCommand _renameCommand;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("OLD");
         _spatialStructure = new SpatialStructure();
         A.CallTo(_interactionTaskContext.NamingTask).WithReturnType<string>().Returns("NEW");

         A.CallTo(() => _interactionTaskContext.Context.AddToHistory(A<IMoBiCommand>._))
            .Invokes(x => _renameCommand = x.GetArgument<IMoBiCommand>(0));
      }

      protected override void Because()
      {
         sut.Rename(_container, _spatialStructure);
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
}