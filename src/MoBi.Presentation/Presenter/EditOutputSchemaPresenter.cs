using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditOutputSchemaPresenter : ISimulationSettingsItemPresenter
   {
      void AddOutputInterval();
      void RemoveOutputInterval(OutputIntervalDTO outputIntervalDTO);
      void SetParameterUnit(IParameterDTO parameterDTO, Unit newUnit, OutputIntervalDTO outputIntervalDTO);
      void SetParameterValue(IParameterDTO parameterDTO, double valuevalueInDisplayUnit);
      bool ShowGroupCaption { set; }
      void Edit(IMoBiSimulation simulation);
   }

   internal class EditOutputSchemaPresenter : AbstractSubPresenter<IEditOutputSchemaView, IEditOutputSchemaPresenter>, 
      IEditOutputSchemaPresenter,
      IListener<OutputSchemaChangedEvent>
   {
      private readonly IOutputIntervalToOutputIntervalDTOMapper _intervalMapper;
      private readonly IQuantityTask _quantityTask;
      private readonly IOutputSchemaTask _outputSchemaTask;
      private readonly IDialogCreator _dialogCreator;
      private ICache<OutputIntervalDTO, OutputInterval> _allIntervals;
      private SimulationSettings _simulationSettings;
      private IMoBiSimulation _simulation;

      public EditOutputSchemaPresenter(IEditOutputSchemaView view, IOutputIntervalToOutputIntervalDTOMapper intervalMapper,
         IQuantityTask quantityTask, IOutputSchemaTask outputSchemaTask, IDialogCreator dialogCreator)
         : base(view)
      {
         _intervalMapper = intervalMapper;
         _quantityTask = quantityTask;
         _outputSchemaTask = outputSchemaTask;
         _dialogCreator = dialogCreator;
      }

      private void bindToView()
      {
         var schema = _simulationSettings.OutputSchema;
         _allIntervals = new Cache<OutputIntervalDTO, OutputInterval>(_intervalMapper.MapFrom);
         schema.Intervals.Each(_allIntervals.Add);
         _view.Show(_allIntervals.Keys);
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         Edit(_simulation.Settings);
      }

      public void Edit(SimulationSettings simulationSettings)
      {
         _simulationSettings = simulationSettings;
         bindToView();
      }

      public void AddOutputInterval()
      {
         AddCommand(_outputSchemaTask.AddOutputIntervalTo(_simulationSettings));
         bindToView();
      }

      public void RemoveOutputInterval(OutputIntervalDTO outputIntervalDTO)
      {
         AddCommand(_outputSchemaTask.RemoveOutputInterval(getIntervalFromDto(outputIntervalDTO), _simulationSettings));
         bindToView();
      }

      public void SetParameterUnit(IParameterDTO parameterDTO, Unit newUnit, OutputIntervalDTO outputIntervalDTO)
      {
         // The UI validation of interval start and end does not evaluate for unit changes.
         // This is a programmatic validation of the proposed new unit
         if (!intervalIsValidInNewUnit(parameterDTO, newUnit, outputIntervalDTO))
         {
            displayIntervalErrorMessage();
            return;
         }

         var parameter = parameterDTO.Parameter;
         AddCommand(_simulation != null
            ? _quantityTask.SetQuantityDisplayUnit(parameter, newUnit, _simulation)
            : _quantityTask.SetQuantityDisplayUnit(parameter, newUnit, _simulationSettings));
      }

      private bool intervalIsValidInNewUnit(IParameterDTO parameterDTO, Unit newUnit, OutputIntervalDTO outputIntervalDTO)
      {
         if (parameterDTO == outputIntervalDTO.StartTimeParameter && !checkStartTimeInNewUnit(newUnit, outputIntervalDTO))
            return false;

         if (parameterDTO == outputIntervalDTO.EndTimeParameter && !checkEndTimeInNewUnit(newUnit, outputIntervalDTO))
            return false;

         return true;
      }

      private void displayIntervalErrorMessage()
      {
         _dialogCreator.MessageBoxError(AppConstants.Exceptions.TheStartTimeMustBeEarlierThanTheEndTimeOfTheInterval);
      }

      private bool checkEndTimeInNewUnit(Unit newUnit, OutputIntervalDTO outputIntervalDTO)
      {
         var endValueInCurrentDisplayUnit = displayValueInCurrentDisplayUnit(outputIntervalDTO.EndTimeParameter);
         var endTimeInBaseUnit = newValueInBaseUnit(endValueInCurrentDisplayUnit, newUnit);
         return endTimeInBaseUnit > outputIntervalDTO.StartTimeParameter.Parameter.Value;
      }

      private bool checkStartTimeInNewUnit(Unit newUnit, OutputIntervalDTO outputIntervalDTO)
      {
         var startValueInCurrentDisplayUnit = displayValueInCurrentDisplayUnit(outputIntervalDTO.StartTimeParameter);
         var startTimeInBaseUnit = newValueInBaseUnit(startValueInCurrentDisplayUnit, newUnit);
         return startTimeInBaseUnit < outputIntervalDTO.EndTimeParameter.Parameter.Value;
      }

      private double newValueInBaseUnit(double valueInCurrentDisplayUnit, Unit newUnit)
      {
         return newUnit.UnitValueToBaseUnitValue(valueInCurrentDisplayUnit);
      }

      private double displayValueInCurrentDisplayUnit(IParameterDTO parameterDTO)
      {
         return parameterDTO.Parameter.ValueInDisplayUnit;
      }
      
      public void SetParameterValue(IParameterDTO parameterDTO, double valuevalueInDisplayUnit)
      {
         var parameter = parameterDTO.Parameter;
         AddCommand(_simulation != null
            ? _quantityTask.SetQuantityDisplayValue(parameter, valuevalueInDisplayUnit, _simulation.Settings)
            : _quantityTask.SetQuantityDisplayValue(parameter, valuevalueInDisplayUnit, _simulationSettings));
      }

      private OutputInterval getIntervalFromDto(OutputIntervalDTO outputIntervalDTO)
      {
         return _allIntervals[outputIntervalDTO];
      }

      public override ApplicationIcon Icon => ApplicationIcons.OutputInterval;

      public bool ShowGroupCaption
      {
         set => View.ShowGroupCaption = value;
      }

      public void Handle(OutputSchemaChangedEvent eventToHandle)
      {
         if (eventToHandle.OutputSchema.Equals(_simulationSettings.OutputSchema))
            bindToView();
      }
   }
}