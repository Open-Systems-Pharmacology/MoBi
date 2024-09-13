using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IPathAndValueEntitiesView : IView
   {
      void AddDistributedParameterView(IView view);
      void RefreshForUpdatedEntity();
   }

   [Flags]
   public enum HideableElement
   {
      None = 0,
      ValueOriginColumn = 1,
      DeleteColumn = 2,
      RefreshButton = 4,
      DeleteButton = 8,
      PresenceRibbon = 16,
      ButtonRibbon = 32,
      NegativeValuesRibbon = 64
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

      void HideElements(HideableElement elementsToHide);
      void RefreshData();
      TStartValueDTO FocusedStartValue { get; set; }
      bool CanCreateNewFormula { set; }
   }
}