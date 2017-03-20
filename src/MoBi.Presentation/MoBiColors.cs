using System.Drawing;

namespace MoBi.Presentation
{
   public static class MoBiColors
   {
      /// <summary>
      ///Color used for rows containg a molecule or parameter start value that has been marked as extended
      /// </summary>
      public static Color Extended = Color.LightGreen;

      /// <summary>
      /// Used for rows that do not match the existing builder values
      /// </summary>
      public static Color Modified = Color.LightYellow;

      /// <summary>
      /// Color used for cell that are locked/disabled 
      /// </summary>
      public static Color Disabled = Color.LightGray;

      /// <summary>
      /// The default cell color
      /// </summary>
      public static Color Default = Color.White;

      /// <summary>
      /// Indicates that a builder cannot be resolved from it's source (parameter and molecule start values
      /// </summary>
      public static Color CannotResolve = Color.LightSkyBlue;

      public static Color Black = Color.Black;
   }
}