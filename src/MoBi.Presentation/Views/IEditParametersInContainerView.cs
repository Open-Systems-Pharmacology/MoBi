using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditParametersInContainerView :  IView<IEditParametersInContainerPresenter>
   {
      void SetNameForContainer(IContainer container);
      void BindTo(IEnumerable<ParameterDTO> dtos);
      EditParameterMode EditMode { get; set; }
      bool ShowBuildMode { get; set; }
      string ParentName {  set; }
      void SetEditParameterView(IView view);
      void RefreshList();
      void Select(ParameterDTO parameterToSelect);
   }
}