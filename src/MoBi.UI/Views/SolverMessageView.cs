using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SolverMessageView : BaseModalView, ISolverMessageView
   {
      private readonly GridViewBinder<SolverWarning> _gridBinder;

      public SolverMessageView(IShell shell) : base(shell)
      {
         InitializeComponent();
         _gridBinder = new GridViewBinder<SolverWarning>(gridView);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridBinder.Bind(warning => warning.OutputTime)
            .WithCaption("At time:").AsReadOnly();

         _gridBinder.Bind(warning => warning.Warning)
            .AsReadOnly();
      }

      public void BindTo(IEnumerable<SolverWarning> warnings)
      {
         _gridBinder.BindToSource(warnings);
      }

      public void AttachPresenter(ISolverMessagePresenter presenter)
      {
      }
   }
}