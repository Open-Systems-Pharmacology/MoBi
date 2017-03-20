using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportToPDFCommand<T> : ObjectUICommand<T> where T : class
   {
      private readonly IMoBiApplicationController _applicationController;

      public ExportToPDFCommand(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var presenter = _applicationController.Start<IReportingPresenter>())
         {
            presenter.CreateReport(Subject);
         }
      }
   }

   public class ExportHistoryToPDFCommand : ExportToPDFCommand<IHistoryManager>
   {
      private readonly IMoBiContext _context;

      public ExportHistoryToPDFCommand(IMoBiApplicationController applicationController, IMoBiContext context)
         : base(applicationController)
      {
         _context = context;
      }

      protected override void PerformExecute()
      {
         Subject = _context.HistoryManager;
         base.PerformExecute();
      }
   }

   public class ExportProjectToPDFCommand : ExportToPDFCommand<IMoBiProject>
   {
      private readonly IMoBiContext _context;

      public ExportProjectToPDFCommand(IMoBiApplicationController applicationController, IMoBiContext context)
         : base(applicationController)
      {
         _context = context;
      }

      protected override void PerformExecute()
      {
         Subject = _context.CurrentProject;
         base.PerformExecute();
      }
   }

   public class ExportChartToPDFCommand : ExportToPDFCommand<ICurveChart>
   {
      public ExportChartToPDFCommand(IMoBiApplicationController applicationController) : base(applicationController)
      {
      }
   }

   public class ExportCollectionToPDFCommand<T> : ExportToPDFCommand<IReadOnlyCollection<T>>
   {
      public ExportCollectionToPDFCommand(IMoBiApplicationController applicationController, IMoBiContext context) : base(applicationController)
      {
         var project = context.CurrentProject;
         IEnumerable<T> all;
         if (typeof(T).IsAnImplementationOf<IBuildingBlock>())
            all = project.AllBuildingBlocks().Where(x => x.IsAnImplementationOf<T>()).Cast<T>();
         
         else if (typeof (T).IsAnImplementationOf<IMoBiSimulation>())
            all = project.Simulations.Cast<T>();
         
         else if (typeof (T).IsAnImplementationOf<DataRepository>())
            all = project.AllObservedData.Cast<T>();
         
         else
            all = Enumerable.Empty<T>();

         Subject = new ReadOnlyCollection<T>(all.ToList());
      }
   }
}