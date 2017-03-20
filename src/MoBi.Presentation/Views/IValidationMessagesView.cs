using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IValidationMessagesView : IModalView<IValidationMessagesPresenter>
   {
      void ShowMessages(IEnumerable<ValidationMessage> messages);
      void BindToFilter(LogFilterDTO logStatusFilter);
   }
}