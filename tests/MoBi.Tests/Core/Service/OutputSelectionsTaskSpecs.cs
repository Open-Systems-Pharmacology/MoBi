using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Services;

namespace MoBi.Core.Service
{
   public class concern_for_OutputSelectionsTask : ContextSpecification<OutputSelectionsTask>
   {
      protected ISimulationRepository _simulationRepository;
      protected IMoBiApplicationController _applicationController;
      protected IHierarchicalQuantitySelectionPresenter _quantitySelectionPresenter;
      protected IModalPresenter _modalPresenter;
      protected SimulationSettings _simulationSettings;
      protected IDialogCreator _dialogCreator;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _simulationRepository = A.Fake<ISimulationRepository>();
         _modalPresenter = A.Fake<IModalPresenter>();
         _quantitySelectionPresenter = A.Fake<IHierarchicalQuantitySelectionPresenter>();
         _simulationSettings = new SimulationSettings();

         sut = new OutputSelectionsTask(_context, _applicationController, _simulationRepository, _dialogCreator);

         A.CallTo(() => _applicationController.Start<IModalPresenter>()).Returns(_modalPresenter);
         A.CallTo(() => _applicationController.Start<IHierarchicalQuantitySelectionPresenter>()).Returns(_quantitySelectionPresenter);

         SetupFakePresenterResponses();
      }

      protected virtual void SetupFakePresenterResponses()
      {
         A.CallTo(() => _modalPresenter.Show()).Returns(true);
         A.CallTo(() => _quantitySelectionPresenter.SelectedPath).Returns(new ObjectPath("another|path"));
      }
   }

   public class When_removing_an_output_selection_from_a_simulation_settings : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("test", QuantityType.Undefined);
      }

      protected override void Because()
      {
         sut.RemoveOutputSelection(_simulationSettings, _selection);
      }

      [Observation]
      public void should_remove_the_selection_from_the_simulation_settings()
      {
         _simulationSettings.OutputSelections.AllOutputs.ShouldNotContain(_selection);
      }

      [Observation]
      public void should_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustHaveHappened();
      }
   }

   public class When_canceling_the_edit_dialog_when_editing_a_output_selection : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();
         _selection = new QuantitySelection("test", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
      }

      protected override void SetupFakePresenterResponses()
      {
         A.CallTo(() => _modalPresenter.Show()).Returns(false);
      }

      protected override void Because()
      {
         sut.EditOutputSelection(_simulationSettings, _selection);
      }

      [Observation]
      public void should_not_update_the_selection_in_the_simulation_settings()
      {
         _selection.Path.ShouldBeEqualTo("test");
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_canceling_the_edit_portion_of_adding_a_new_output_selection : concern_for_OutputSelectionsTask
   {
      protected override void SetupFakePresenterResponses()
      {
         A.CallTo(() => _modalPresenter.Show()).Returns(false);
      }

      protected override void Because()
      {
         sut.AddOutputSelection(_simulationSettings, null);
      }

      [Observation]
      public void should_not_add_a_new_selection_to_the_simulation_settings()
      {
         _simulationSettings.OutputSelections.AllOutputs.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_adding_an_output_selection_to_a_simulation_settings_and_the_path_already_exists_in_the_output_selections : concern_for_OutputSelectionsTask
   {
      protected override void Context()
      {
         base.Context();
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("another|path", QuantityType.Undefined));
      }

      protected override void Because()
      {
         sut.AddOutputSelection(_simulationSettings);
      }

      [Observation]
      public void the_user_should_be_warned()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_add_a_new_selection_to_the_simulation_settings()
      {
         _simulationSettings.OutputSelections.AllOutputs.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_adding_an_output_selection_to_a_simulation_settings : concern_for_OutputSelectionsTask
   {
      protected override void Because()
      {
         sut.AddOutputSelection(_simulationSettings);
      }

      [Observation]
      public void should_add_a_new_selection_to_the_simulation_settings()
      {
         _simulationSettings.OutputSelections.AllOutputs.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustHaveHappened();
      }
   }

   public class When_editing_an_output_selection_with_tree_browser_in_a_simulation_settings_and_the_path_is_already_selected : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("test", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("another|path", QuantityType.Undefined));
      }

      protected override void Because()
      {
         sut.EditOutputSelection(_simulationSettings, _selection);
      }

      [Observation]
      public void the_user_should_be_warned()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_quantity_path_should_not_be_updated()
      {
         _selection.Path.ShouldBeEqualTo("test");
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_editing_an_output_selection_with_tree_browser_in_a_simulation_settings_and_the_same_quantity_is_selected_and_not_changed : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("another|path", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("test", QuantityType.Undefined));
      }

      protected override void Because()
      {
         sut.EditOutputSelection(_simulationSettings, _selection);
      }

      [Observation]
      public void the_user_should_not_be_warned()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_quantity_path_should_not_be_updated()
      {
         _selection.Path.ShouldBeEqualTo("another|path");
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_editing_an_output_selection_with_tree_browser_in_a_simulation_settings : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("test", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
      }

      protected override void Because()
      {
         sut.EditOutputSelection(_simulationSettings, _selection);
      }

      [Observation]
      public void the_quantity_path_should_be_updated()
      {
         _selection.Path.ShouldBeEqualTo("another|path");
      }

      [Observation]
      public void should_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustHaveHappened();
      }
   }

   public class When_editing_an_output_selection_string_path_in_a_simulation_settings_where_there_is_no_change_made_to_the_path : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("test", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("another|path", QuantityType.Undefined));
      }

      protected override void Because()
      {
         sut.UpdateOutputSelection(_simulationSettings, _selection, "test");
      }

      [Observation]
      public void the_user_should_not_be_warned()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_quantity_path_should_not_be_updated()
      {
         _selection.Path.ShouldBeEqualTo("test");
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_editing_an_output_selection_string_path_in_a_simulation_settings_where_there_is_a_duplicate_output_already_selected : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("test", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
         _simulationSettings.OutputSelections.AddOutput(new QuantitySelection("another|path", QuantityType.Undefined));
      }

      protected override void Because()
      {
         sut.UpdateOutputSelection(_simulationSettings, _selection, "another|path");
      }

      [Observation]
      public void the_user_should_be_warned()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_quantity_path_should_not_be_updated()
      {
         _selection.Path.ShouldBeEqualTo("test");
      }

      [Observation]
      public void should_not_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustNotHaveHappened();
      }
   }

   public class When_editing_an_output_selection_string_path_in_a_simulation_settings : concern_for_OutputSelectionsTask
   {
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();

         _selection = new QuantitySelection("test", QuantityType.Undefined);
         _simulationSettings.OutputSelections.AddOutput(_selection);
      }

      protected override void Because()
      {
         sut.UpdateOutputSelection(_simulationSettings, _selection, "another|path");
      }

      [Observation]
      public void the_quantity_path_should_be_updated()
      {
         _selection.Path.ShouldBeEqualTo("another|path");
      }

      [Observation]
      public void should_update_the_building_block_version()
      {
         A.CallTo(() => _context.ProjectChanged()).MustHaveHappened();
      }
   }
}