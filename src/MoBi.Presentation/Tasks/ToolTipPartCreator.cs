using System.Collections.Generic;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Tasks
{
   public class ToolTipPartCreator : IToolTipPartCreator
   {
      public IList<ToolTipPart> ToolTipFor<T>(T objectRequestingToolTip)
      {
         //No dynamic tooltips supported yet
         return new List<ToolTipPart>();
      }

      public IList<ToolTipPart> ToolTipFor(string toolTipToDisplay)
      {
         //No dynamic tooltips supported yet
         return new List<ToolTipPart>();
      }
   }
}