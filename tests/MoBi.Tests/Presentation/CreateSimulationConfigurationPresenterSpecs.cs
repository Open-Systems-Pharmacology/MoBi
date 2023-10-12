using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.IntegrationTests;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreateSimulationConfigurationPresenter : ContextForIntegration<CreateSimulationConfigurationPresenter>
   {
      protected ICreateSimulationConfigurationView _view;
      protected IModelConstructor _modelConstructor;
      protected IDimensionValidator _validationVisitor;
      protected IUserSettings _userSettings;
      protected ISimulationFactory _simulationFactory;
      protected IMoBiApplicationController _applicationController;
      protected IHeavyWorkManager _heavyWorkManager;
      private ISubPresenterItemManager<ISimulationConfigurationItemPresenter> _subPresenterManager;
      private IDialogCreator _dialogCreator;
      protected IForbiddenNamesRetriever _forbiddenNameRetriever;
      protected IMoBiSimulation _simulation;
      protected SimulationConfiguration _simulationConfiguration;
      protected SimulationSettings _clonedSimulationSettings;
      private IModuleConfigurationDTOToModuleConfigurationMapper _moduleConfigurationMapper;
      protected ISimulationConfigurationFactory _simulationConfigurationFactory;

      protected override void Context()
      {
         _view = A.Fake<ICreateSimulationConfigurationView>();
         _subPresenterManager = A.Fake<ISubPresenterItemManager<ISimulationConfigurationItemPresenter>>();
         _clonedSimulationSettings = new SimulationSettings();
         _moduleConfigurationMapper = A.Fake<IModuleConfigurationDTOToModuleConfigurationMapper>();

         _modelConstructor = A.Fake<IModelConstructor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _modelConstructor.CreateModelFrom(A<SimulationConfiguration>._, A<string>._)).Returns(A.Fake<CreationResult>());

         _simulationConfigurationFactory = A.Fake<ISimulationConfigurationFactory>();
         _validationVisitor = A.Fake<IDimensionValidator>();
         _userSettings = A.Fake<IUserSettings>();
         _userSettings.CheckCircularReference = true;
         _simulationFactory = A.Fake<ISimulationFactory>();
         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         _forbiddenNameRetriever = A.Fake<IForbiddenNamesRetriever>();
         sut = new CreateSimulationConfigurationPresenter(_view, _subPresenterManager, _dialogCreator,
            _forbiddenNameRetriever, _userSettings, _moduleConfigurationMapper, _simulationConfigurationFactory);

         _simulation = new MoBiSimulation();
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         _simulationConfiguration = createBuildConfiguration();
         _simulation.Configuration = _simulationConfiguration;
      }

      private SimulationConfiguration createBuildConfiguration()
      {
         var simulationConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
         return simulationConfiguration;
      }
   }

   internal class When_cancelling_the_create_of_a_new_configuration : concern_for_CreateSimulationConfigurationPresenter
   {
      private SimulationConfiguration _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void should_revert_commands()
      {
         _result.ShouldBeNull();
      }
   }

   public class configuring_an_existing_simulation_configuration : concern_for_CreateSimulationConfigurationPresenter
   {
      private SimulationConfiguration _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _simulationConfigurationFactory.Create(_simulation.Settings)).Returns(new SimulationConfiguration { SimulationSettings = _simulation.Settings });
      }

      protected override void Because()
      {
         _result = sut.CreateBasedOn(_simulation, false);
      }
      
      [Observation]
      public void the_forbidden_names_must_not_be_initialized()
      {
         A.CallTo(() => _forbiddenNameRetriever.For(_simulation)).MustNotHaveHappened();
      }

      [Observation]
      public void the_view_must_disable_naming()
      {
         A.CallTo(() => _view.DisableNaming()).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_check_circular_reference_according_to_value_in_user_settings()
      {
         _simulationConfiguration.PerformCircularReferenceCheck.ShouldBeEqualTo(_userSettings.CheckCircularReference);
      }

      [Observation]
      public void the_simulation_settings_should_not_be_replaced()
      {
         _result.SimulationSettings.ShouldBeEqualTo(_simulation.Settings);
      }
   }

   public class creating_a_new_simulation_configuration : concern_for_CreateSimulationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void the_forbidden_names_must_be_initialized()
      {
         A.CallTo(() => _forbiddenNameRetriever.For(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_check_circular_reference_according_to_value_in_user_settings()
      {
         _simulationConfiguration.PerformCircularReferenceCheck.ShouldBeEqualTo(_userSettings.CheckCircularReference);
      }
   }
}