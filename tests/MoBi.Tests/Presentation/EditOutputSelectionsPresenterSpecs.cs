using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_EditOutputSelectionsPresenter : ContextSpecification<EditOutputSelectionsPresenter>
   {
      protected IEditOutputSelectionsView _view;
      protected IOutputSelectionsTask _outputSelectionsTask;

      protected override void Context()
      {
         _view = A.Fake<IEditOutputSelectionsView>();
         _outputSelectionsTask = A.Fake<IOutputSelectionsTask>();
         sut = new EditOutputSelectionsPresenter(_view, _outputSelectionsTask);
      }
   }

   public class When_editing_output_selections : concern_for_EditOutputSelectionsPresenter
   {
      private SimulationSettings _simulationSettings;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
      }

      protected override void Because()
      {
         sut.Edit(_simulationSettings);
      }

      [Observation]
      public void should_bind_to_output_selections()
      {
         A.CallTo(() => _view.BindTo(_simulationSettings.OutputSelections.AllOutputs)).MustHaveHappened();
      }
   }

   public class When_editing_output_selections_with_new_path_and_selection : concern_for_EditOutputSelectionsPresenter
   {
      private SimulationSettings _simulationSettings;
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _selection = new QuantitySelection("path", QuantityType.Undefined);
         sut.Edit(_simulationSettings);
      }

      protected override void Because()
      {
         sut.EditOutputSelection(_selection);
      }

      [Observation]
      public void should_edit_output_selection()
      {
         A.CallTo(() => _outputSelectionsTask.EditOutputSelection(_simulationSettings, _selection)).MustHaveHappened();
      }
   }

   public class When_editing_output_selections_with_new_path : concern_for_EditOutputSelectionsPresenter
   {
      private SimulationSettings _simulationSettings;
      private QuantitySelection _selection;
      private string _newPath;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _selection = new QuantitySelection("path", QuantityType.Undefined);
         _newPath = "newPath";
         sut.Edit(_simulationSettings);
      }

      protected override void Because()
      {
         sut.UpdateOutputSelection(_selection, _newPath);
      }

      [Observation]
      public void should_edit_output_selection()
      {
         A.CallTo(() => _outputSelectionsTask.UpdateOutputSelection(_simulationSettings, _selection, _newPath)).MustHaveHappened();
      }
   }

   public class When_adding_output_selections_to_simulation_settings : concern_for_EditOutputSelectionsPresenter
   {
      private SimulationSettings _simulationSettings;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         sut.Edit(_simulationSettings);
      }

      protected override void Because()
      {
         sut.AddOutputSelection(null);
      }

      [Observation]
      public void should_add_output_selection_to_simulation_settings()
      {
         A.CallTo(() => _outputSelectionsTask.AddOutputSelection(_simulationSettings, null)).MustHaveHappened();
      }
   }

   public class When_removing_output_selections_from_simulation_settings : concern_for_EditOutputSelectionsPresenter
   {
      private SimulationSettings _simulationSettings;
      private QuantitySelection _selection;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _selection = new QuantitySelection("path", QuantityType.Undefined);
         sut.Edit(_simulationSettings);
      }

      protected override void Because()
      {
         sut.RemoveOutputSelection(_selection);
      }

      [Observation]
      public void should_remove_output_selection_from_simulation_settings()
      {
         A.CallTo(() => _outputSelectionsTask.RemoveOutputSelection(_simulationSettings, _selection)).MustHaveHappened();
      }
   }
}
