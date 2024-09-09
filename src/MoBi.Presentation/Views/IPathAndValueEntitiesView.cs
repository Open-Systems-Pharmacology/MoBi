using System.Collections.Generic;
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
      void AddDeleteStartValuesView(IView view);
      void HideDeleteView();
      void HideDeleteColumn();
      void HideIsPresentView();
      void HideRefreshView();
      void HideNegativeValuesAllowedView();
      void HideValueOriginColumn();
      void DisablePathColumns();
      void HideNegativeValuesNotAllowedView();
      void HideIsNotPresentView();
      void RefreshData();
      TStartValueDTO FocusedStartValue { get; set; }
      bool CanCreateNewFormula { set; }
   }
}