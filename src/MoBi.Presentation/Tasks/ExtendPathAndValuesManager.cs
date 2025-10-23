using System;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public interface IExtendPathAndValuesManager<T> : IMergeManager<T> where T : PathAndValueEntity
   {
      /// <summary>
      ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
      ///    method will be used to remove the existing element
      /// </summary>
      Action<T> RemoveAction { set; }

      /// <summary>
      ///    In the event of a conflict where user specifies that the newly merged element should replace the existing, this
      ///    method will be used to add the newly merged element
      /// </summary>
      Action<T> AddAction { set; }
   }
   
   public abstract class ExtendPathAndValuesManager<T> : AbstractMergeManager<T>, IExtendPathAndValuesManager<T> where T : PathAndValueEntity
   {
      public Action<T> RemoveAction
      {
         set => _removeAction = value;
      }

      public Action<T> AddAction
      {
         set => _addAction = value;
      }
   }
}