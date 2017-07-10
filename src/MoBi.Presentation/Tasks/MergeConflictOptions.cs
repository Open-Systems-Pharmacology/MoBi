using System;

namespace MoBi.Presentation.Tasks
{
   [Flags]
   public enum MergeConflictOptions
   {
      ReplaceOnce = 1 << 0,
      SkipOnce = 1 << 1,
      Cancel = 1 << 2,
      MergeOnce = 1 << 3,
      CloneOnce = 1 << 4,
      ReplaceAll = 1 << 5,
      SkipAll = 1 << 6,
      MergeAll = 1 << 7,
      CloneAll = 1 << 8,
      AutoRename = 1 << 9, 

      Replace = ReplaceAll | ReplaceOnce,
      Merge = MergeOnce | MergeAll,
      Skip = SkipAll | SkipOnce,
      Clone = CloneAll | CloneOnce,

      AppliesToAll = ReplaceAll | MergeAll | SkipAll | CloneAll
   }
   public static class MergeConflictOptionsExtensions
   {
      /// <summary>
      /// Tests if the option is meant as clone the merged item and add
      /// </summary>
      /// <returns>True if Replace or ReplaceAll, otherwise false</returns>
      public static bool IsClone(this MergeConflictOptions option)
      {
         return option.Is(MergeConflictOptions.Clone);
      }

      /// <summary>
      /// Tests if the given option is meant to be used for all subsequent instances of the merge conflict
      /// </summary>
      /// <returns></returns>
      public static bool IsAppliedToAll(this MergeConflictOptions option)
      {
         return option.Is(MergeConflictOptions.AppliesToAll);
      }

      /// <summary>
      /// Tests if the option is meant as replace the existing
      /// </summary>
      /// <returns>True if Replace or ReplaceAll, otherwise false</returns>
      public static bool IsReplace(this MergeConflictOptions option)
      {
         return option.Is(MergeConflictOptions.Replace);
      }

      /// <summary>
      /// Tests if the option is meant as merge two conflicting elements
      /// </summary>
      /// <returns>True if Merge or MergeAll, otherwise false</returns>
      public static bool IsMerge(this MergeConflictOptions option)
      {
         return option.Is(MergeConflictOptions.Merge);
      }

      /// <summary>
      /// Tests if the option is meant as skip or leave the existing element
      /// </summary>
      /// <returns>True if Skip or SkipAll, otherwise false</returns>
      public static bool IsSkip(this MergeConflictOptions option)
      {
         return option.Is(MergeConflictOptions.Skip);
      }

      public static bool IsAutoRename(this MergeConflictOptions option)
      {
         return option.Is(MergeConflictOptions.AutoRename);
      }

      public static bool Is(this MergeConflictOptions option, MergeConflictOptions optionToCompare)
      {
         return (option & optionToCompare) != 0;
      }

   }
}