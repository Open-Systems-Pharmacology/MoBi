﻿using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IApplyToSelectionButtonView : IView<IApplyToSelectionPresenter>
   {
      void BindToSelection();
      void SetButonIcon(ApplicationIcon icon);
   }
}  