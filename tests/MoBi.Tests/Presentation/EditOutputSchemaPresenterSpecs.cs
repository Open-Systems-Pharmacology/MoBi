using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation
{
   internal class concern_for_EditOutputSchemaPresenter : ContextSpecification<EditOutputSchemaPresenter>
   {
      protected IDialogCreator _dialogCreator;
      private IOutputSchemaTask _outputSchemaTask;
      protected IQuantityTask _quantityTask;
      private IOutputIntervalToOutputIntervalDTOMapper _intervalMapper;
      private IEditOutputSchemaView _view;
      protected SimulationSettings _simulationSettings;
      protected OutputIntervalDTO _outputInterval;

      public override void GlobalContext()
      {
         base.GlobalContext();
         DimensionFactoryForSpecs.TimeDimension.AddUnit("minutes", 60, 0, isDefault: false);
         DimensionFactoryForSpecs.TimeDimension.AddUnit("years", 31556952, 0, isDefault: false);
      }

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _outputSchemaTask = A.Fake<IOutputSchemaTask>();
         _quantityTask = A.Fake<IQuantityTask>();
         _intervalMapper = A.Fake<IOutputIntervalToOutputIntervalDTOMapper>();
         _view = A.Fake<IEditOutputSchemaView>();
         _simulationSettings = new SimulationSettings
         {
            OutputSchema = new OutputSchema()
         };

         sut = new EditOutputSchemaPresenter(_view, _intervalMapper, _quantityTask, _outputSchemaTask, _dialogCreator);

         var timeDimension = DimensionFactoryForSpecs.TimeDimension;
         var minutes = timeDimension.Unit("minutes");
         _outputInterval = new OutputIntervalDTO
         {
            StartTimeParameter = new ParameterDTO(new Parameter().WithDimension(timeDimension).WithDisplayUnit(minutes)),
            EndTimeParameter = new ParameterDTO(new Parameter().WithDimension(timeDimension).WithDisplayUnit(minutes)),
            ResolutionParameter = new ParameterDTO(new Parameter().WithDimension(timeDimension)),
         };

         // start = 10 minutes, end = 20 minutes
         _outputInterval.StartTimeParameter.Parameter.Value = _outputInterval.StartTimeParameter.Parameter.ConvertToBaseUnit(10, minutes);
         _outputInterval.EndTimeParameter.Parameter.Value = _outputInterval.EndTimeParameter.Parameter.ConvertToBaseUnit(20, minutes);

         sut.InitializeWith(A.Fake<ICommandCollector>());
         sut.Edit(_simulationSettings);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         DimensionFactoryForSpecs.TimeDimension.RemoveUnit("minutes");
         DimensionFactoryForSpecs.TimeDimension.RemoveUnit("years");
      }
   }

   internal class When_the_output_interval_start_time_becomes_later_than_the_end_time_through_setting_the_unit : concern_for_EditOutputSchemaPresenter
   {
      private Unit _newUnit;

      protected override void Context()
      {
         base.Context();
         _newUnit = DimensionFactoryForSpecs.TimeDimension.Unit("years");
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_outputInterval.StartTimeParameter, _newUnit, _outputInterval);
      }

      [Observation]
      public void the_dialog_should_show_the_error_message()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(AppConstants.Exceptions.TheStartTimeMustBeEarlierThanTheEndTimeOfTheInterval)).MustHaveHappened();
      }

      [Observation]
      public void the_quantity_task_is_not_used_to_change_the_unit()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_outputInterval.StartTimeParameter.Parameter, _newUnit, _simulationSettings)).MustNotHaveHappened();
      }
   }

   internal class When_the_output_interval_end_time_becomes_less_than_start_time_through_setting_the_unit : concern_for_EditOutputSchemaPresenter
   {
      private Unit _newUnit;

      protected override void Context()
      {
         base.Context();
         _newUnit = DimensionFactoryForSpecs.TimeDimension.Unit("s");
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_outputInterval.EndTimeParameter, _newUnit, _outputInterval);
      }

      [Observation]
      public void the_dialog_should_show_the_error_message()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(AppConstants.Exceptions.TheStartTimeMustBeEarlierThanTheEndTimeOfTheInterval)).MustHaveHappened();
      }

      [Observation]
      public void the_quantity_task_is_not_used_to_change_the_unit()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_outputInterval.EndTimeParameter.Parameter, _newUnit, _simulationSettings)).MustNotHaveHappened();
      }
   }

   internal class When_changing_the_start_time_unit : concern_for_EditOutputSchemaPresenter
   {
      private Unit _newUnit;

      protected override void Context()
      {
         base.Context();
         _newUnit = DimensionFactoryForSpecs.TimeDimension.Unit("s");
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_outputInterval.StartTimeParameter, _newUnit, _outputInterval);
      }

      [Observation]
      public void the_dialog_should_show_the_error_message()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(AppConstants.Exceptions.TheStartTimeMustBeEarlierThanTheEndTimeOfTheInterval)).MustNotHaveHappened();
      }

      [Observation]
      public void the_quantity_task_is_used_to_change_the_unit()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_outputInterval.StartTimeParameter.Parameter, _newUnit, _simulationSettings)).MustHaveHappened();
      }
   }

   internal class When_changing_the_end_time_unit : concern_for_EditOutputSchemaPresenter
   {
      private Unit _newUnit;

      protected override void Context()
      {
         base.Context();
         _newUnit = DimensionFactoryForSpecs.TimeDimension.Unit("years");
      }

      protected override void Because()
      {
         sut.SetParameterUnit(_outputInterval.EndTimeParameter, _newUnit, _outputInterval);
      }

      [Observation]
      public void the_dialog_should_show_the_error_message()
      {
         A.CallTo(() => _dialogCreator.MessageBoxError(AppConstants.Exceptions.TheStartTimeMustBeEarlierThanTheEndTimeOfTheInterval)).MustNotHaveHappened();
      }

      [Observation]
      public void the_quantity_task_is_used_to_change_the_unit()
      {
         A.CallTo(() => _quantityTask.SetQuantityDisplayUnit(_outputInterval.EndTimeParameter.Parameter, _newUnit, _simulationSettings)).MustHaveHappened();
      }
   }
}