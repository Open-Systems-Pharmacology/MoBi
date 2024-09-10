using System;
using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IPathAndValueEntitiesView : IView
   {
      void AddDistributedParameterView(IView view);
      void RefreshForUpdatedEntity();
   }

   public interface IPathAndValueEntitiesView<TStartValueDTO> : IPathAndValueEntitiesView
   {
      void BindTo(IEnumerable<TStartValueDTO> startValueDTOs);

      /// <summary>
      ///    Initializes the columns associated with paths in the start value.
      /// </summary>
      void InitializePathColumns();

      /// <summary>
      ///    Add items to the list of suggested values for path elements
      /// </summary>
      /// <param name="pathValues">The list of suggested path values</param>
      void AddPathItems(IEnumerable<string> pathValues);

      void ClearPathItems();
      IReadOnlyList<TStartValueDTO> SelectedStartValues { get; }
      IReadOnlyList<TStartValueDTO> VisibleStartValues { get; }
      void HideDeleteButton();
      void HideDeleteColumn();
      void HideIsPresentButton();
      void HideRefreshButton();
      void HideNegativeValuesAllowedButton();
      void HideValueOriginColumn();
      void DisablePathColumns();
      void HideNegativeValuesNotAllowedButton();
      void HideIsNotPresentButton();
      void RefreshData();
      TStartValueDTO FocusedStartValue { get; set; }
      bool CanCreateNewFormula { set; }

      event Action IsPresentAction;
      event Action IsNotPresentAction;
      event Action NegativeValuesAllowedAction;
      event Action NegativeValuesNotAllowedAction;
      event Action RefreshAction;
      event Action DeleteAction;
   }
}