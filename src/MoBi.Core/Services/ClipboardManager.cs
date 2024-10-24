using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IClipboardManager
   {
      void CopyToClipBoard(IObjectBase copyToClipboard);
      void CopyToClipBoard(IReadOnlyList<IObjectBase> objectsToCopy);
      void CutToClipBoard<T>(T cutToClipBoard, Action<T> remove) where T : IObjectBase;
      void CutToClipBoard<T>(IReadOnlyList<T> objectsToCut, Action<T> remove) where T : IObjectBase;
      void PasteFromClipBoard<T>(Action<T> add) where T : class, IObjectBase;
      bool ClipBoardContainsType<T>() where T : class, IObjectBase;
 
       void Clear();
   }

   public class ClipboardManager : IClipboardManager
   {
      private readonly ClipboardData _clipboardData;
      private readonly ICloneManagerForBuildingBlock _cloneManager;

      public ClipboardManager(ICloneManagerForBuildingBlock cloneManager)
      {
         _clipboardData = new ClipboardData();
         _cloneManager = cloneManager;
      }

      public void CopyToClipBoard(IObjectBase copyToClipboard) => CopyToClipBoard(new[] {copyToClipboard});

      public void CutToClipBoard<T>(T cutToClipBoard, Action<T> remove) where T : IObjectBase => CutToClipBoard(new[] {cutToClipBoard}, remove);

      public void CopyToClipBoard(IReadOnlyList<IObjectBase> objectsToCopy) => _clipboardData.Init(objectsToCopy);

      public void CutToClipBoard<T>(IReadOnlyList<T> objectsToCut, Action<T> remove) where T : IObjectBase
      {
         _clipboardData.Init(objectsToCut.Cast<IObjectBase>().ToList());
         objectsToCut.Each(remove);
      }

      public bool ClipBoardContainsType<T>() where T : class, IObjectBase => 
         _clipboardData.PastedObjects.Any(pasteObject => pasteObject.IsAnImplementationOf<T>());

      public void PasteFromClipBoard<T>(Action<T> add) where T : class, IObjectBase
      {
         var pasteObjects = _clipboardData.PastedObjects;
         if (pasteObjects.Any(pasteObject => !pasteObject.IsAnImplementationOf<T>())) 
            return;

         var newObjects = new List<T>();
         _cloneManager.FormulaCache = new FormulaCache();

         pasteObjects.Cast<T>().Each(pasteObject => newObjects.Add(_cloneManager.Clone(pasteObject)));
         newObjects.OfType<IEntity>().Each(newEntity => newEntity.ParentContainer = null);
         newObjects.Each(add);
      }

      public void Clear() => _clipboardData.Clear();
   }
}