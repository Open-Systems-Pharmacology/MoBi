using System;
using System.Collections.Generic;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IExtendablePathAndValueBuildingBlockPresenter<TPathAndValueEntityDTO> : IPathAndValueBuildingBlockPresenter<TPathAndValueEntityDTO>, ISubjectPresenter,
      ILatchable,
      IListener<PathAndValueEntitiesBuildingBlockChangedEvent>,
      IListener<BulkUpdateFinishedEvent>,
      IListener<BulkUpdateStartedEvent>
      where TPathAndValueEntityDTO : IPathAndValueEntityDTO

   {
      /// <summary>
      ///    Removes an entity from a path and value entity building block
      /// </summary>
      /// <param name="elementToRemove">The element to remove</param>
      void RemovePathAndValueEntity(TPathAndValueEntityDTO elementToRemove);

      /// <summary>
      ///    Updates an element in the container path of the path and value entity
      /// </summary>
      /// <param name="pathAndValueEntity">The path and value entity dto being updated</param>
      /// <param name="indexToUpdate">The index of the element that should be updated</param>
      /// <param name="newValue">The new value for the path element</param>
      void UpdatePathAndValueEntityContainerPath(TPathAndValueEntityDTO pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Sets a new name for a path and value entity.
      /// </summary>
      /// <param name="pathAndValueEntityDTO">The path and value entity dto being updated</param>
      /// <param name="newValue">The new value of name for the dto</param>
      void UpdatePathAndValueEntityName(TPathAndValueEntityDTO pathAndValueEntityDTO, string newValue);

      /// <summary>
      ///    Adds a new empty path and value entity to the view
      /// </summary>
      TPathAndValueEntityDTO AddNewEmptyPathAndValueEntity();

      /// <summary>
      ///    Sets if new formula can be created. Default is true
      /// </summary>
      bool CanCreateNewFormula { set; }

      void Delete(IReadOnlyList<TPathAndValueEntityDTO> selectedStartValues);
   }
}