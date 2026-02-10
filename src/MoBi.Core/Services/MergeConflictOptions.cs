using System;

namespace MoBi.Core.Services;

[Flags]
public enum MergeConflictOptions
{
   ReplaceAll = 1,
   SkipAll = 1 << 1
}

public static class MergeConflictOptionsExtensions
{
   /// <summary>
   ///    Tests if the option is meant as replace the existing
   /// </summary>
   /// <returns>True if ReplaceAll, otherwise false</returns>
   public static bool IsReplace(this MergeConflictOptions option)
   {
      return option.Is(MergeConflictOptions.ReplaceAll);
   }

   /// <summary>
   ///    Tests if the option is meant as skip or leave the existing element
   /// </summary>
   /// <returns>True if SkipAll, otherwise false</returns>
   public static bool IsSkip(this MergeConflictOptions option)
   {
      return option.Is(MergeConflictOptions.SkipAll);
   }

   public static bool Is(this MergeConflictOptions option, MergeConflictOptions optionToCompare)
   {
      return (option & optionToCompare) != 0;
   }
}