using System;
using System.Collections.Generic;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IApplyToSelectionPresenter : IPresenter
   {
      /// <summary>
      ///    The current SelectOption
      /// </summary>
      SelectOption CurrentSelection { get; set; }

      /// <summary>
      ///    The list of valid SelectOptions
      /// </summary>
      IEnumerable<SelectOption> AvailableSelectOptions { get; }

      /// <summary>
      ///    Called when the action should be run using CurrentSelection
      /// </summary>
      void PerformSelectionHandler();

      /// <summary>
      ///    The action that should be executed in response to PerformSelectionHandler
      /// </summary>
      Action<SelectOption> ApplySelectionAction { get; set; }
   }

   public abstract class ApplyToSelectionPresenter : AbstractPresenter<IApplyToSelectionButtonView, IApplyToSelectionPresenter>, IApplyToSelectionPresenter
   {
      public Action<SelectOption> ApplySelectionAction { get; set; }
      public SelectOption CurrentSelection { get; set; }

      protected ApplyToSelectionPresenter(IApplyToSelectionButtonView view, SelectOption defaultSelection, string caption, ApplicationIcon icon = null) : base(view)
      {
         CurrentSelection = defaultSelection;
         view.Caption = caption;
         if(icon!=null)
            view.SetButonIcon(icon);

         view.BindToSelection();
      }

      public abstract IEnumerable<SelectOption> AvailableSelectOptions { get; }

      public void PerformSelectionHandler()
      {
         ApplySelectionAction(CurrentSelection);
      }
   }
}