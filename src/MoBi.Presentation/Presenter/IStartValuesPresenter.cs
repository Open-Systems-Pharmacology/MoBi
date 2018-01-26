using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IStartValuesPresenter : ISubjectPresenter,
      ILatchable,
      IListener<StartValuesBuildingBlockChangedEvent>,
      IListener<BulkUpdateFinishedEvent>,
      IListener<BulkUpdateStartedEvent>

   {
      void HideRefreshStartValuesView();
      void HideIsPresentView();
      void HideNegativeValuesAllowedView();
      void HideLegend();
      void HideDeleteColumn();

      IEnumerable<StartValueFormulaDTO> AllFormulas();

      /// <summary>
      ///    Checks if the color corresponds to Modified or Extended colors
      /// </summary>
      /// <param name="color">The color being checked</param>
      /// <returns>true if the color is one of the two identified</returns>
      bool IsColorDefault(Color color);

      /// <summary>
      ///    Turns on or off the IsModified filter
      /// </summary>
      bool IsModifiedFilterOn { get; set; }

      /// <summary>
      ///    Turns on or off the IsNew filter
      /// </summary>
      bool IsNewFilterOn { get; set; }

      /// <summary>
      ///    Adds a new empty start value to the view
      /// </summary>
      void AddNewEmptyStartValue();

      /// <summary>
      ///    Hides all sub presenters except the show filter selection
      /// </summary>
      void OnlyShowFilterSelection();

      /// <summary>
      /// Sets if new formula can be created. Default is true
      /// </summary>
      bool CanCreateNewFormula { set; }

      void ExtendStartValues();
   }

   public interface IStartValuesPresenter<TStartValueDTO> : IStartValuesPresenter, IBreadCrumbsPresenter where TStartValueDTO : IStartValueDTO
   {
      void SetFormula(TStartValueDTO startValueDTO, IFormula formula);
      void AddNewFormula(TStartValueDTO startValueDTO);

      void SetUnit(TStartValueDTO startValueDTO, Unit newUnit);
      void SetValue(TStartValueDTO startValueDTO, double? valueInDisplayUnit);

      Color BackgroundColorFor(TStartValueDTO startValueDTO);

      /// <summary>
      ///    Removes a Start Value from a start value building block
      /// </summary>
      /// <param name="elementToRemove">The element to remove</param>
      void RemoveStartValue(TStartValueDTO elementToRemove);

      /// <summary>
      ///    Updates an element in the container path of the start value
      /// </summary>
      /// <param name="startValue">The start value dto being updated</param>
      /// <param name="indexToUpdate">The index of the element that should be updated</param>
      /// <param name="newValue">The new value for the path element</param>
      void UpdateStartValueContainerPath(TStartValueDTO startValue, int indexToUpdate, string newValue);

      /// <summary>
      ///    Sets a new name for a start value.
      /// </summary>
      /// <param name="startValueDTO">The start value dto being updated</param>
      /// <param name="newValue">The new value of name for the dto</param>
      void UpdateStartValueName(TStartValueDTO startValueDTO, string newValue);

      /// <summary>
      ///    Determines if the start value should be shown in the view based on filter settings and whether it's been modified
      /// </summary>
      /// <param name="startValue">The start value to be hidden or shown</param>
      /// <returns>True if the value should be shown, otherwise false</returns>
      bool ShouldShow(TStartValueDTO startValue);

      /// <summary>
      ///    Sets a new value origin for a start value.
      /// </summary>
      void SetValueOrigin(TStartValueDTO startValueDTO, ValueOrigin newValueOrigin);

      /// <summary>
      ///    Function returns the background color used to display the  start value.
      ///    Per default, the defined functions returns the default color for grid view back ground
      /// </summary>
      Func<TStartValueDTO, Color> BackgroundColorRetriever { get; set; }

      /// <summary>
      ///    Function returns true if the start value was part of the original building block
      ///    If it's been added since edit, returns false
      /// </summary>
      Func<TStartValueDTO, bool> IsOriginalStartValue { get; set; }
   }
}