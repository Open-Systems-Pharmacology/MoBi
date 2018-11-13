using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;
using SimModelNET;

namespace MoBi.UI.Views
{
   public partial class SolverMessageView : BaseModalView, ISolverMessageView
   {
      private readonly GridViewBinder<ISolverWarning> _gridBinder;

      public SolverMessageView(IShell shell) : base(shell)
      {
         InitializeComponent();
         _gridBinder = new GridViewBinder<ISolverWarning>(gridView);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridBinder.Bind(warning => warning.OutputTime)
            .WithCaption("At time:").AsReadOnly();

         _gridBinder.Bind(warning => warning.Warning)
            .AsReadOnly();
      }

      public void BindTo(IEnumerable<ISolverWarning> warnings)
      {
         _gridBinder.BindToSource(warnings);
      }

      public void AttachPresenter(ISolverMessagePresenter presenter)
      {
      }
   }
}