using System;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IDefaultOutputSelectionsButtonsView : IView
   {
      Action MakeProjectDefaultsClicked { get; set; }
      Action LoadProjectDefaultsClicked { get; set; }
   }

   public interface IOutputSelectionsView : IModalView<IOutputSelectionsPresenter>
   {
      void AddSettingsView(IView view);
      void AddDefaultButtonsView(IView view);
   }
}