using System.Collections.Generic;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IStartValuesPresenter : ISubjectPresenter,
      ILatchable,
      IListener<PathAndValueEntitiesBuildingBlockChangedEvent>,
      IListener<BulkUpdateFinishedEvent>,
      IListener<BulkUpdateStartedEvent>

   {
      IEnumerable<ValueFormulaDTO> AllFormulas();

      /// <summary>
      ///    Adds a new empty start value to the view
      /// </summary>
      void AddNewEmptyStartValue();

      /// <summary>
      ///    Sets if new formula can be created. Default is true
      /// </summary>
      bool CanCreateNewFormula { set; }
   }

   public interface IStartValuesPresenter<TDTO> : IStartValuesPresenter, IBreadCrumbsPresenter where TDTO : IPathAndValueEntityDTO
   {
      void SetFormula(TDTO startValueDTO, IFormula formula);
      void AddNewFormula(TDTO startValueDTO);

      void SetUnit(TDTO startValueDTO, Unit newUnit);
      void SetValue(TDTO startValueDTO, double? valueInDisplayUnit);

      /// <summary>
      ///    Removes a Start Value from a start value building block
      /// </summary>
      /// <param name="elementToRemove">The element to remove</param>
      void RemoveStartValue(TDTO elementToRemove);

      /// <summary>
      ///    Updates an element in the container path of the start value
      /// </summary>
      /// <param name="pathAndValueEntity">The start value dto being updated</param>
      /// <param name="indexToUpdate">The index of the element that should be updated</param>
      /// <param name="newValue">The new value for the path element</param>
      void UpdateStartValueContainerPath(TDTO pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Sets a new name for a start value.
      /// </summary>
      /// <param name="startValueDTO">The start value dto being updated</param>
      /// <param name="newValue">The new value of name for the dto</param>
      void UpdateStartValueName(TDTO startValueDTO, string newValue);

      /// <summary>
      ///    Sets a new value origin for a start value.
      /// </summary>
      void SetValueOrigin(TDTO startValueDTO, ValueOrigin newValueOrigin);
   }
}