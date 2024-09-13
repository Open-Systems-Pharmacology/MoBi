using System.Collections.Generic;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IPathAndValueEntitiesView : IView
   {
      void AddDistributedParameterView(IView view);
      void RefreshForUpdatedEntity();
   }

   public enum HideableElement
   {
      ValueOriginColumn,
      DeleteColumn,
      RefreshButton,
      DeleteButton,
      PresenceRibbon,
      ButtonRibbon,
      NegativeValuesRibbon
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

      void DisablePathColumns();

      void HideElement(HideableElement elementToHide);
      void RefreshData();
      TStartValueDTO FocusedStartValue { get; set; }
      bool CanCreateNewFormula { set; }
   }
}