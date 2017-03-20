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
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditOutputSchemaPresenter : ISimulationSettingsItemPresenter
   {
      void AddOutputInterval();
      void RemoveOutputInterval(OutputIntervalDTO outputIntervalDTO);
      void SetParameterUnit(IParameterDTO parameterDTO, Unit newUnit);
      void SetParameterValue(IParameterDTO parameterDTO, double valuevalueInDisplayUnit);
      bool ShowGroupCaption { set; }
      void Edit(IMoBiSimulation simulation);
   }

   internal class EditOutputSchemaPresenter : AbstractSubPresenter<IEditOutputSchemaView, IEditOutputSchemaPresenter>, IEditOutputSchemaPresenter
   {
      private readonly IOutputIntervalToOutputIntervalDTOMapper _intervalMapper;
      private readonly IQuantityTask _quantityTask;
      private readonly IOutputSchemaTask _outputSchemaTask;
      private ICache<OutputIntervalDTO, OutputInterval> _allIntervals;
      private ISimulationSettings _simulationSettings;
      private IMoBiSimulation _simulation;

      public EditOutputSchemaPresenter(IEditOutputSchemaView view, IOutputIntervalToOutputIntervalDTOMapper intervalMapper,
         IQuantityTask quantityTask, IOutputSchemaTask outputSchemaTask)
         : base(view)
      {
         _intervalMapper = intervalMapper;
         _quantityTask = quantityTask;
         _outputSchemaTask = outputSchemaTask;
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

      public void Edit(ISimulationSettings simulationSettings)
      {
         _simulationSettings = simulationSettings;
         bindToView();
      }

      public void AddOutputInterval()
      {
         AddCommand(_outputSchemaTask.AddOuputIntervalTo(_simulationSettings, _simulation));
         bindToView();
      }

      public void RemoveOutputInterval(OutputIntervalDTO outputIntervalDTO)
      {
         AddCommand(_outputSchemaTask.RemoveOuputInterval(getIntervalFromDto(outputIntervalDTO), _simulationSettings, _simulation));
         bindToView();
      }

      public void SetParameterUnit(IParameterDTO parameterDTO, Unit newUnit)
      {
         var parameter = parameterDTO.Parameter;
         AddCommand(_simulation != null
            ? _quantityTask.SetQuantityDisplayUnit(parameter, newUnit, _simulation)
            : _quantityTask.SetQuantityDisplayUnit(parameter, newUnit, _simulationSettings));
      }

      public void SetParameterValue(IParameterDTO parameterDTO, double valuevalueInDisplayUnit)
      {
         var parameter = parameterDTO.Parameter;
         AddCommand(_simulation != null
            ? _quantityTask.SetQuantityDisplayValue(parameter, valuevalueInDisplayUnit, _simulation)
            : _quantityTask.SetQuantityDisplayValue(parameter, valuevalueInDisplayUnit, _simulationSettings));
      }

      private OutputInterval getIntervalFromDto(OutputIntervalDTO outputIntervalDTO)
      {
         return _allIntervals[outputIntervalDTO];
      }

      public override ApplicationIcon Icon
      {
         get { return ApplicationIcons.OutputInterval; }
      }

      public bool ShowGroupCaption
      {
         set { View.ShowGroupCaption = value; }
      }
   }
}