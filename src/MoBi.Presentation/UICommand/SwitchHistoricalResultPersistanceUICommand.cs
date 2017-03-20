using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class SwitchHistoricalResultPersistanceUICommand : ObjectUICommand<DataRepository>
   {
      protected override void PerformExecute()
      {
         Subject.SetPersistable(!Subject.IsPersistable());
      }
   }
}