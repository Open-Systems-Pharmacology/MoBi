using System.Windows.Forms;
using MoBi.Presentation.UICommand;
using OSPSuite.Presentation.UICommands;

namespace MoBi.UI.UICommands
{
   public class CopyTextCommand : ObjectUICommand<string>, ICopyTextCommand
   {
      protected override void PerformExecute()
      {
         Clipboard.SetText(Subject);
      }
   }
}