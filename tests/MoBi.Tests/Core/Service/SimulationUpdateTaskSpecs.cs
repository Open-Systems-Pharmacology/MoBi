using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationUpdateTask : ContextSpecification<ISimulationUpdateTask>
   {
      protected IModelConstructor _modelConstructor;
      protected IBuildConfigurationFactory _buildConfigurationFactory;
      protected IObjectPathFactory _objectPathFactory;
      protected IEventPublisher _eventPublisher;
      private IMoBiContext _context;
      private IMoBiApplicationController _applicationController;
      protected IConfigureSimulationPresenter _configurePresenter;
      private IDimensionValidator _validationVisitor;
      protected IEntityPathResolver _entityPathResolver;
      protected IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;

      protected override void Context()
      {
         _modelConstructor = A.Fake<IModelConstructor>();
         _buildConfigurationFactory = A.Fake<IBuildConfigurationFactory>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _context = A.Fake<IMoBiContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _configurePresenter = A.Fake<IConfigureSimulationPresenter>();
         _validationVisitor = A.Fake<IDimensionValidator>();
         _affectedBuildingBlockRetriever = A.Fake<IAffectedBuildingBlockRetriever>();
         _entityPathResolver = new EntityPathResolverForSpecs();
         A.CallTo(() => _applicationController.Start<IConfigureSimulationPresenter>()).Returns(_configurePresenter);

         sut = new SimulationUpdateTask(_modelConstructor, _buildConfigurationFactory, _context, _applicationController, _validationVisitor, _entityPathResolver, _affectedBuildingBlockRetriever);
      }
   }

   internal class When_updating_an_changed_simulation_where_changed_object_is_removed_by_update : concern_for_SimulationUpdateTask
   {
      private ICommand _resultCommand;
      private IMoBiSimulation _simulationToUpdate;
      private ISpatialStructure _templateBuildingBlock;
      private IMoBiBuildConfiguration _updatedBuildConfiguration;

      protected override void Context()
      {
         base.Context();

         _simulationToUpdate = A.Fake<IMoBiSimulation>();
         _simulationToUpdate.Model.Name = "XX";
         _templateBuildingBlock = A.Fake<ISpatialStructure>();
         _updatedBuildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         A.CallTo(() => _configurePresenter.BuildConfiguration).Returns(_updatedBuildConfiguration);
      }

      protected override void Because()
      {
         _resultCommand = sut.UpdateSimulationFrom(_simulationToUpdate, _templateBuildingBlock);
      }

      [Observation]
      public void should_return_an_update_simulation_command()
      {
         var allCommands = _resultCommand.DowncastTo<MoBiMacroCommand>();
         allCommands.ShouldNotBeNull();
         allCommands.All().SingleOrDefault(c => c.IsAnImplementationOf<UpdateSimulationCommand>()).ShouldNotBeNull();
         allCommands.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_create_model_from_updated_build_configuration()
      {
         A.CallTo(() => _modelConstructor.CreateModelFrom(_updatedBuildConfiguration, _simulationToUpdate.Model.Name)).MustHaveHappened();
      }
   }

   public class When_updating_an_unchanged_simulation_with_a_changed_buildingBlock : concern_for_SimulationUpdateTask
   {
      private ICommand _resultCommand;
      private IMoBiSimulation _simulationToUpdate;
      private IBuildingBlock _templateBuildingBlock;
      private IMoBiBuildConfiguration _updatedBuildConfiguration;

      protected override void Context()
      {
         base.Context();
         _simulationToUpdate = A.Fake<IMoBiSimulation>();
         _simulationToUpdate.Model.Name = "XX";
         _templateBuildingBlock = A.Fake<IBuildingBlock>();
         _updatedBuildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         A.CallTo(() => _buildConfigurationFactory.CreateFromReferencesUsedIn(_simulationToUpdate.MoBiBuildConfiguration, _templateBuildingBlock)).Returns(_updatedBuildConfiguration);
      }

      protected override void Because()
      {
         _resultCommand = sut.UpdateSimulationFrom(_simulationToUpdate, _templateBuildingBlock);
      }

      [Observation]
      public void should_return_an_update_simulation_command()
      {
         _resultCommand.ShouldNotBeNull();
         _resultCommand.ShouldBeAnInstanceOf<MoBiMacroCommand>();
      }

      [Observation]
      public void should_create_model_from_updated_build_configuration()
      {
         A.CallTo(() => _modelConstructor.CreateModelFrom(_updatedBuildConfiguration, _simulationToUpdate.Model.Name)).MustHaveHappened();
      }
   }

   public class When_updating_a_simulation_with_fixed_value_with_a_template_building_block_that_allows_quick_update : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToUpdate;
      private IBuildingBlock _templateBuildingBlock;
      private IParameter _originalParameterFixedNotBelongingToTemplate;
      private IParameter _originalParameterFixedUpdatedByTemplate;
      private IBuildingBlockInfo _buildingBlockInfoTemplate;
      private IParameter _updatedParameterFixedNotBelongingToTemplate;
      private IParameter _updatedParameterFixedUpdatedByTemplate;

      protected override void Context()
      {
         base.Context();
         _simulationToUpdate = A.Fake<IMoBiSimulation>();
         _templateBuildingBlock = A.Fake<IBuildingBlock>();

         var rootContainer = createRootContainer();
         _simulationToUpdate.Model.Root = rootContainer;

         _originalParameterFixedNotBelongingToTemplate = rootContainer.Parameter("P1");
         _originalParameterFixedNotBelongingToTemplate.Value = 5;

         _originalParameterFixedUpdatedByTemplate = rootContainer.Parameter("P2");
         _originalParameterFixedUpdatedByTemplate.Value = 8;


         _buildingBlockInfoTemplate = A.Fake<IBuildingBlockInfo>();
         A.CallTo(() => _buildingBlockInfoTemplate.UntypedTemplateBuildingBlock).Returns(_templateBuildingBlock);

         //simulates the update command
         var container = createRootContainer();

         _updatedParameterFixedNotBelongingToTemplate = container.Parameter(_originalParameterFixedNotBelongingToTemplate.Name);
         _updatedParameterFixedUpdatedByTemplate = container.Parameter(_originalParameterFixedUpdatedByTemplate.Name);

         A.CallTo(() => _affectedBuildingBlockRetriever.RetrieveFor(_updatedParameterFixedUpdatedByTemplate, _simulationToUpdate)).Returns(_buildingBlockInfoTemplate);
         A.CallTo(() => _affectedBuildingBlockRetriever.RetrieveFor(_updatedParameterFixedNotBelongingToTemplate, _simulationToUpdate)).Returns(A.Fake<IBuildingBlockInfo>());

         A.CallTo(() => _simulationToUpdate.Update(A<IMoBiBuildConfiguration>._, A<IModel>._)).Invokes(x => { _simulationToUpdate.Model.Root = container; });
      }

      private IContainer createRootContainer()
      {
         return new Container
         {
            new Parameter().WithName("P1").WithId("P1").WithFormula(new ExplicitFormula("1+2")),
            new Parameter().WithName("P2").WithId("P2").WithFormula(new ExplicitFormula("2+3")),
            new Parameter().WithName("P3").WithId("P3").WithFormula(new ExplicitFormula("5+7"))
         }.WithName("ROOT");
      }

      protected override void Because()
      {
         sut.UpdateSimulationFrom(_simulationToUpdate, _templateBuildingBlock);
      }

      [Observation]
      public void should_keep_the_quantity_not_affected_by_the_updated_as_marked_with_a_fixed_value()
      {
         _updatedParameterFixedNotBelongingToTemplate.Value.ShouldBeEqualTo(_originalParameterFixedNotBelongingToTemplate.Value);
         _updatedParameterFixedNotBelongingToTemplate.IsFixedValue.ShouldBeTrue();
      }

      [Observation]
      public void should_update_all_quantity_affected_by_the_updated_()
      {
         _updatedParameterFixedUpdatedByTemplate.IsFixedValue.ShouldBeFalse();
      }
   }

   public class When_configuration_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToConfigure;
      private IMoBiCommand _command;
      private CreationResult _creationResult;
      private IModel _model;

      protected override void Context()
      {
         base.Context();
         _simulationToConfigure = new MoBiSimulation {Model = new Model {Root = new Container()}.WithName("OLD_MODEL")};
         _model = new Model().WithName("NEW MODEL");
         _model.Root = new Container();
         _creationResult = new CreationResult(_model);
         _command = new MoBiMacroCommand();
         A.CallTo(() => _configurePresenter.CreateBuildConfiguration(_simulationToConfigure)).Returns(_command);
         A.CallTo(() => _modelConstructor.CreateModelFrom(_configurePresenter.BuildConfiguration, _simulationToConfigure.Model.Name)).Returns(_creationResult);
      }

      protected override void Because()
      {
         sut.ConfigureSimulation(_simulationToConfigure);
      }

      [Observation]
      public void should_start_the_configure_workflow_for_the_user()
      {
         A.CallTo(() => _configurePresenter.CreateBuildConfiguration(_simulationToConfigure)).MustHaveHappened();
      }

      [Observation]
      public void should_create_a_new_simulation_using_the_build_configuration_setup_by_the_user()
      {
         _simulationToConfigure.Model.ShouldBeEqualTo(_model);
      }
   }
}