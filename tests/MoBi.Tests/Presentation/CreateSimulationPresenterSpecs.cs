using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using MoBi.IntegrationTests;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreateSimulationPresenter : ContextForIntegration<ICreateSimulationPresenter>
   {
      protected ICreateSimulationView _view;
      protected IMoBiContext _context;
      protected IModelConstructor _modelConstructor;
      protected IDimensionValidator _validationVisitor;
      protected IUserSettings _userSettings;
      protected ISimulationFactory _simulationFactory;
      protected IMoBiApplicationController _applicationController;
      protected IHeavyWorkManager _heavyWorkManager;
      private ISubPresenterItemManager<ISimulationItemPresenter> _subPresenterManager;
      private IDialogCreator _dialogCreator;
      private IForbiddenNamesRetriever _forbiddenNameRetriever;
      protected IMoBiSimulation _simulation;
      protected string _templateId = "Template";
      protected SimulationConfiguration _simulationConfiguration;
      private ICloneManagerForBuildingBlock _cloneManager;
      protected SimulationSettings _clonedSimulationSettings;
      protected const string _useId = "ToUse";

      protected override void Context()
      {
         _view = A.Fake<ICreateSimulationView>();
         _subPresenterManager = A.Fake<ISubPresenterItemManager<ISimulationItemPresenter>>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _clonedSimulationSettings = new SimulationSettings();

         _context = A.Fake<IMoBiContext>();
         _modelConstructor = A.Fake<IModelConstructor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _modelConstructor.CreateModelFrom(A<SimulationConfiguration>._, A<string>._)).Returns(A.Fake<CreationResult>());


         _validationVisitor = A.Fake<IDimensionValidator>();
         _userSettings = A.Fake<IUserSettings>();
         _userSettings.CheckCircularReference = true;
         _simulationFactory = A.Fake<ISimulationFactory>();
         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         _forbiddenNameRetriever = A.Fake<IForbiddenNamesRetriever>();
         sut = new CreateSimulationPresenter(_view, _context, _modelConstructor, _validationVisitor,
            _simulationFactory, _heavyWorkManager, _subPresenterManager, _dialogCreator,
            _forbiddenNameRetriever, _userSettings, _cloneManager);

         _simulation = new MoBiSimulation();
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         _simulationConfiguration = createBuildConfiguration();
         _simulation.Configuration = _simulationConfiguration;

         A.CallTo(() => _cloneManager.CloneBuildingBlock(_context.CurrentProject.SimulationSettings)).Returns(_clonedSimulationSettings);
      }

      private SimulationConfiguration createBuildConfiguration()
      {
         var simulationConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
         return simulationConfiguration;
      }
   }

   internal class When_finishing_creation_of_a_simulation : concern_for_CreateSimulationPresenter
   {
      private IMoBiSimulation _result;

      protected override void Because()
      {
         _result = sut.Create();
      }

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      [Observation]
      public void should_revert_commands()
      {
         _result.ShouldBeNull();
      }
   }

   internal class When_creating_a_new_simulation : concern_for_CreateSimulationPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.Create();
      }

      [Observation]
      public void the_cloned_simulation_settings_should_be_used_in_the_configuration()
      {
         _simulation.Configuration.SimulationSettings.ShouldBeEqualTo(_clonedSimulationSettings);
      }

      [Observation]
      public void should_ask_simulation_factory_for_new_simulation()
      {
         A.CallTo(() => _simulationFactory.Create()).MustHaveHappened();
      }

      [Observation]
      public void should_create_model()
      {
         A.CallTo(() => _modelConstructor.CreateModelFrom(_simulationConfiguration, A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_check_circular_reference_according_to_value_in_user_settings()
      {
         _simulationConfiguration.PerformCircularReferenceCheck.ShouldBeEqualTo(_userSettings.CheckCircularReference);
      }
   }
}