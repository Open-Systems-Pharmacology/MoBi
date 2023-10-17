using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using Container = OSPSuite.Core.Domain.Container;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTaskForContainer : ContextSpecification<EditTaskForContainer>
   {
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IInteractionTask _interactionTask;
      protected IObjectPathFactory _objectPathFactory;
      private IMoBiApplicationController _applicationController;
      protected ISelectFolderAndIndividualFromProjectPresenter _selectIndividualFromProjectPresenter;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IIndividualParameterToParameterMapper _individualParameterToParameterMapper;

      protected override void Context()
      {
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _selectIndividualFromProjectPresenter = A.Fake<ISelectFolderAndIndividualFromProjectPresenter>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _individualParameterToParameterMapper = A.Fake<IIndividualParameterToParameterMapper>();
         A.CallTo(() => _applicationController.Start<ISelectFolderAndIndividualFromProjectPresenter>()).Returns(_selectIndividualFromProjectPresenter);
         A.CallTo(() => _interactionTaskContext.ApplicationController).Returns(_applicationController);
         _interactionTask = A.Fake<IInteractionTask>();
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         A.CallTo(() => _interactionTaskContext.InteractionTask).Returns(_interactionTask);
         sut = new EditTaskForContainer(_interactionTaskContext, _spatialStructureFactory, _objectPathFactory, _cloneManager, _individualParameterToParameterMapper);
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

   public class When_saving_a_container_with_individual_to_pkml : concern_for_EditTaskForContainer
   {
      private IContainer _clonedContainer;
      private Container _parentContainer;
      private MoBiSpatialStructure _tmpSpatialStructure;
      private IndividualBuildingBlock _individual;
      private IContainer _containerToSave;
      private Parameter _replacedParameter;

      protected override void Context()
      {
         base.Context();
         _parentContainer = new Container().WithName("Parent");
         _individual = new IndividualBuildingBlock().WithName("Individual");
         _individual.Add(new IndividualParameter {ContainerPath = new ObjectPath("Parent", "Container1")}.WithName("parameter1"));
         _individual.Add(new IndividualParameter {ContainerPath = new ObjectPath("Parent", "Container1")}.WithName("ShouldBeReplaced"));
         _individual.Add(new IndividualParameter {ContainerPath = new ObjectPath("Parent", "Container2")}.WithName("parameter2"));
         _containerToSave = new Container().WithName("Container1").WithMode(ContainerMode.Physical).Under(_parentContainer);
         _clonedContainer = new Container().WithName("Container1").WithMode(ContainerMode.Physical);
         _replacedParameter = new Parameter().WithName("ShouldBeReplaced");
         _clonedContainer.Add(_replacedParameter);
         
         _tmpSpatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };

         A.CallTo(() => _selectIndividualFromProjectPresenter.SelectedFilePath).Returns("FilePath");
         A.CallTo(() => _selectIndividualFromProjectPresenter.SelectedIndividual).Returns(_individual);
         A.CallTo(() => _spatialStructureFactory.Create()).Returns(_tmpSpatialStructure);
         A.CallTo(() => _cloneManager.Clone(_containerToSave, _tmpSpatialStructure.FormulaCache)).Returns(_clonedContainer);
         A.CallTo(() => _individualParameterToParameterMapper.MapFrom(A<IndividualParameter>._)).ReturnsLazily(x => new Parameter().WithName(x.Arguments.Get<IndividualParameter>(0).Name));
      }

      protected override void Because()
      {
         sut.SaveWithIndividual(_containerToSave);
      }

      [Observation]
      public void the_exported_structure_should_have_parameter_overrides_when_parameter_is_in_both_individual_and_spatial_structure()
      {
         _tmpSpatialStructure.TopContainers.Single(x => x.Name.Equals("Container1")).GetSingleChildByName("ShouldBeReplaced").ShouldNotBeEqualTo(_replacedParameter);
      }

      [Observation]
      public void the_exported_structure_should_have_parameters_from_the_individual_if_they_match_the_container_path()
      {
         _tmpSpatialStructure.TopContainers.Single(x => x.Name.Equals("Container1")).ContainsName("parameter1").ShouldBeTrue();
         _tmpSpatialStructure.TopContainers.Single(x => x.Name.Equals("Container1")).ContainsName("ShouldBeReplaced").ShouldBeTrue();
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