using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain
{
   public class ClipboardData
   {
      private readonly List<IObjectBase> _pastedObjects = new List<IObjectBase>();

      public IEnumerable<IObjectBase> PastedObjects
      {
         get { return _pastedObjects; }
      }

      /// <summary>
      /// Init clipboard with data and removed old ones
      /// </summary>
      /// <param name="pasteObjects"></param>
      public void Init(IEnumerable<IObjectBase> pasteObjects)
      {
         Clear();
         AddRange(pasteObjects);
      }

      /// <summary>
      /// add new items to clipboard. Do not erase previous ones
      /// </summary>
      public void AddRange(IEnumerable<IObjectBase> pasteObjects)
      {
         _pastedObjects.AddRange(pasteObjects);
      }

      /// <summary>
      /// add item to clipboard. Do not erase previous ones
      /// </summary>
      public void Add(IObjectBase pastedObject)
      {
         _pastedObjects.Add(pastedObject);
      }

      public void Clear()
      {
         _pastedObjects.Clear();
      }

   }

  
}