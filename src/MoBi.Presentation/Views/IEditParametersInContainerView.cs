using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditParametersInContainerView : IView<IEditParametersInContainerPresenter>
   {
      void BindTo(IReadOnlyList<ParameterDTO> parameterDTOs);
      EditParameterMode EditMode { get; set; }
      bool ShowBuildMode { get; set; }
      string ContainerPath { set; }
      void SetEditParameterView(IView view);
      void RefreshList();
      void Select(ParameterDTO parameterToSelect);
      void CopyToClipBoard(string text);
      void ShowIndividualSelection(bool show);
   }
}