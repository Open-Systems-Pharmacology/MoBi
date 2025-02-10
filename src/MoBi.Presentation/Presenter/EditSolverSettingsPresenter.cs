using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoBi.Assets;
using OSPSuite.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using ISolverOptionDTO = MoBi.Presentation.DTO.ISolverOptionDTO;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSolverSettingsPresenter : ISimulationSettingsItemPresenter
   {
      IEnumerable<string> GetSolverNames();
      void SetSolverPropertyValue(ISolverOptionDTO solverOptionDTO, string newValue, string oldValue);
      void SolverChanged(string solverName);
      bool ShowGroupCaption { set; }
      void Edit(IMoBiSimulation simulation);
   }

   public class EditSolverSettingsPresenter : AbstractSubPresenter<IEditSolverSettingsView, IEditSolverSettingsPresenter>, IEditSolverSettingsPresenter
   {
      private readonly ISolverSettingsToDTOSolverSettingsMapper _solverSettingsMapper;
      private readonly IMoBiContext _context;
      private SimulationSettings _simulationSettings;
      private IMoBiSimulation _simulation;


      public EditSolverSettingsPresenter(IEditSolverSettingsView view, ISolverSettingsToDTOSolverSettingsMapper solverSettingsMapper,
         IMoBiContext context) : base(view)
      {
         _context = context;
         _solverSettingsMapper = solverSettingsMapper;
      }

      public IEnumerable<string> GetSolverNames()
      {
         return new Collection<string> {AppConstants.CVODE_1002_2_SOLVER};
      }

      public void SetSolverPropertyValue(ISolverOptionDTO solverOptionDTO, string newValue, string oldValue)
      {
         //always convert the value to the actual property type
         var oldValueToUse = _context.DeserializeValueTo(solverOptionDTO.Type, oldValue);
         var newValueToUse = _context.DeserializeValueTo(solverOptionDTO.Type, newValue);
         AddCommand(createEditCommandFor(solverOptionDTO.Name, newValueToUse, oldValueToUse).RunCommand(_context));
      }

      private IMoBiCommand createEditCommandFor(string name, object newValue, object oldValue)
      {
         return new EditSolverPropertyCommand(name, newValue, oldValue, _simulationSettings);
      }

      public override ApplicationIcon Icon => ApplicationIcons.Solver;

      public void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         Edit(_simulation.Settings);
      }

      public void Edit(SimulationSettings simulationSettings)
      {
         _simulationSettings = simulationSettings;
         _view.Show(_solverSettingsMapper.MapFrom(_simulationSettings.Solver));
      }

      public void SolverChanged(string solverName)
      {
         throw new NotSupportedException("Only one solver supported at the time");
      }

      public bool ShowGroupCaption
      {
         set { View.ShowGroupCaption = value; }
      }
   }
}