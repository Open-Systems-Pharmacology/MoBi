using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class RenameTask<T> where T : class, IObjectBase
   {
      private readonly IInteractionTask _interactionTask;
      private readonly IMoBiContext _context;

      protected RenameTask(IInteractionTask interactionTask, IMoBiContext context)
      {
         _interactionTask = interactionTask;
         _context = context;
      }

      public void Rename(T objectBase, IBuildingBlock buildingBlock)
      {
         var forbiddenNames = GetForbiddenNames(objectBase);
         var command = _interactionTask.Rename(objectBase, forbiddenNames, buildingBlock);
         _context.AddToHistory(command);
      }

      /// <summary>
      ///    Gets the forbidden names for the given object, from the local list the objects name is removed.
      ///    This is done to ensure that an already added objects name is not considered as forbidden, for the object itself.
      /// </summary>
      /// <param name="objectBase">The object base for which we get the forbidden Names.</param>
      /// <returns> The forbidden names.</returns>
      public IEnumerable<string> GetForbiddenNamesWithoutSelf(T objectBase)
      {
         var unallowedNames = GetUnallowedNames(objectBase).ToList();
         // only remove objects name here to be sure that the same name is not used for a complicating object anywhere else
         unallowedNames.Remove(objectBase.Name);
         return unallowedNames.Union(_interactionTask.ForbiddenNamesFor(objectBase));
      }

      public abstract IEnumerable<string> GetUnallowedNames(T objectBase);

      /// <summary>
      ///    Gets the forbidden names here the active objects name maybe forbidden.
      /// </summary>
      /// <param name="objectBase">The object base for which we get the forbidden Names.</param>
      /// <returns> The forbidden names.</returns>
      public IEnumerable<string> GetForbiddenNames(T objectBase)
      {
         var unallowedNames = GetUnallowedNames(objectBase).ToList();
         return unallowedNames.Union(_interactionTask.ForbiddenNamesFor(objectBase));
      }
   }
}