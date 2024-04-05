using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{

   public interface IExtendablePathAndValueBuildingBlockPresenter<in TDTO> : IPathAndValueBuildingBlockPresenter<TDTO>, ISubjectPresenter,
      ILatchable,
      IListener<PathAndValueEntitiesBuildingBlockChangedEvent>,
      IListener<BulkUpdateFinishedEvent>,
      IListener<BulkUpdateStartedEvent>
      where TDTO : IPathAndValueEntityDTO

   {
      /// <summary>
      ///    Removes an entity from a path and value entity building block
      /// </summary>
      /// <param name="elementToRemove">The element to remove</param>
      void RemovePathAndValueEntity(TDTO elementToRemove);

      /// <summary>
      ///    Updates an element in the container path of the path and value entity
      /// </summary>
      /// <param name="pathAndValueEntity">The path and value entity dto being updated</param>
      /// <param name="indexToUpdate">The index of the element that should be updated</param>
      /// <param name="newValue">The new value for the path element</param>
      void UpdatePathAndValueEntityContainerPath(TDTO pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Sets a new name for a path and value entity.
      /// </summary>
      /// <param name="pathAndValueEntityDTO">The path and value entity dto being updated</param>
      /// <param name="newValue">The new value of name for the dto</param>
      void UpdatePathAndValueEntityName(TDTO pathAndValueEntityDTO, string newValue);

      /// <summary>
      ///    Adds a new empty path and value entity to the view
      /// </summary>
      void AddNewEmptyPathAndValueEntity();

      /// <summary>
      ///    Sets if new formula can be created. Default is true
      /// </summary>
      bool CanCreateNewFormula { set; }
   }
}