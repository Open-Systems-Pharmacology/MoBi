using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Reporting;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public class ReportingPresenter : AbstractReportingPresenter
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public ReportingPresenter(IReportingView view, IReportTemplateRepository reportTemplateRepository, IDialogCreator dialogCreator, IReportingTask reportingTask, IObjectTypeResolver objectTypeResolver, IStartOptions runOptions) :
            base(view, reportTemplateRepository, dialogCreator, reportingTask, runOptions)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      protected override string RetrieveObjectTypeFrom(object objectToReport)
      {
         return _objectTypeResolver.TypeFor(objectToReport);
      }
   }
}