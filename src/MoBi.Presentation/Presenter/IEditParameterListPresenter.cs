using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterListPresenter : IParameterPresenter, IPresenterWithContextMenu<IViewItem>, IPresenter<IEditParameterListView>
   {
      void GoTo(ParameterDTO parameterDTO);
      void Edit(IEnumerable<IParameter> parameters);
      void SetVisibility(PathElementId pathElement, bool isVisible);
      IEnumerable<IParameter> EditedParameters { get; }
      IReadOnlyList<IParameter> SelectedParameters { get; set; }
      void EnableSimulationTracking(TrackableSimulation simulation);
      bool HasModules(IReadOnlyList<ParameterDTO> parameters);
      bool HasBuildingBlocks(IReadOnlyList<ParameterDTO> parameters);
      void NavigateToParameter(ParameterDTO parameterDTO);
   }
}