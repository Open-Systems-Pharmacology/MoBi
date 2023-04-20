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
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Service
{
   public abstract class concern_for_SimulationUpdateTask : ContextSpecification<SimulationUpdateTask>
   {
      
      protected IObjectPathFactory _objectPathFactory;
      private IMoBiContext _context;
      private IMoBiApplicationController _applicationController;

      protected IEntityPathResolver _entityPathResolver;
      protected ICreateSimulationConfigurationPresenter _configurePresenter;
      protected ISimulationFactory _simulationFactory;

      protected override void Context()
      {
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _context = A.Fake<IMoBiContext>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _entityPathResolver = new EntityPathResolverForSpecs();
         _configurePresenter = A.Fake<ICreateSimulationConfigurationPresenter>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).Returns(_configurePresenter);

         sut = new SimulationUpdateTask(_context, _applicationController, _entityPathResolver, _simulationFactory);
      }
   }

   internal class When_updating_an_changed_simulation_where_changed_object_is_removed_by_update : concern_for_SimulationUpdateTask
   {
      private ICommand _resultCommand;
      private IMoBiSimulation _simulationToUpdate;
      private SpatialStructure _templateBuildingBlock;
      private SimulationConfiguration _updatedBuildConfiguration;

      protected override void Context()
      {
         base.Context();

         _simulationToUpdate = A.Fake<IMoBiSimulation>();
         _simulationToUpdate.Model.Name = "XX";
         _templateBuildingBlock = A.Fake<SpatialStructure>();
         _updatedBuildConfiguration = new SimulationConfiguration();
         A.CallTo(() => _configurePresenter.CreateBasedOn(_simulationToUpdate, false)).Returns(_updatedBuildConfiguration);
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
         allCommands.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_create_model_from_updated_build_configuration()
      {
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(_simulationToUpdate.Model.Name, _updatedBuildConfiguration)).MustHaveHappened();
      }
   }

   public class When_updating_an_unchanged_simulation_with_a_changed_buildingBlock : concern_for_SimulationUpdateTask
   {
      private ICommand _resultCommand;
      private IMoBiSimulation _simulationToUpdate;
      private IBuildingBlock _templateBuildingBlock;
      private SimulationConfiguration _updatedBuildConfiguration;

      protected override void Context()
      {
         base.Context();
         _simulationToUpdate = A.Fake<IMoBiSimulation>();
         _simulationToUpdate.Model.Name = "XX";
         _templateBuildingBlock = A.Fake<IBuildingBlock>();
         _updatedBuildConfiguration = new SimulationConfiguration();
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
      
   }

   public class When_updating_a_simulation_with_fixed_value_with_a_template_building_block_that_allows_quick_update : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToUpdate;
      private IBuildingBlock _templateBuildingBlock;
      private IParameter _originalParameterFixedNotBelongingToTemplate;
      private IParameter _originalParameterFixedUpdatedByTemplate;
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



         //simulates the update command
         var container = createRootContainer();

         _updatedParameterFixedNotBelongingToTemplate = container.Parameter(_originalParameterFixedNotBelongingToTemplate.Name);
         _updatedParameterFixedUpdatedByTemplate = container.Parameter(_originalParameterFixedUpdatedByTemplate.Name);


         A.CallTo(() => _simulationToUpdate.Update(A<SimulationConfiguration>._, A<IModel>._)).Invokes(x => { _simulationToUpdate.Model.Root = container; });
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
      public void should_update_all_quantity_affected_by_the_updated_()
      {
         _updatedParameterFixedUpdatedByTemplate.IsFixedValue.ShouldBeFalse();
      }
   }

   public class When_configuration_a_simulation : concern_for_SimulationUpdateTask
   {
      private IMoBiSimulation _simulationToConfigure;
      private IModel _model;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _simulationToConfigure = new MoBiSimulation { Model = new Model { Root = new Container() }.WithName("OLD_MODEL") };
         _simulationToConfigure.Configuration = new SimulationConfiguration();
         _model = new Model().WithName("NEW MODEL");
         _model.Root = new Container();
         _creationResult = new CreationResult(_model, new SimulationBuilder(_simulationToConfigure.Configuration));
         A.CallTo(() => _simulationFactory.CreateModelAndValidate(A<string>._, A<SimulationConfiguration>._)).Returns(_model);
      }

      protected override void Because()
      {
         sut.ConfigureSimulation(_simulationToConfigure);
      }

      [Observation]
      public void should_start_the_configure_workflow_for_the_user()
      {
         A.CallTo(() => _configurePresenter.CreateBasedOn(_simulationToConfigure, false)).MustHaveHappened();
      }

      [Observation]
      public void should_create_a_new_simulation_using_the_build_configuration_setup_by_the_user()
      {
         _simulationToConfigure.Model.ShouldBeEqualTo(_model);
      }
   }
}