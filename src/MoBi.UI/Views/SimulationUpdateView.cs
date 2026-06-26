using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SimulationUpdateView : BaseModalView, ISimulationUpdateView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private ISimulationUpdatePresenter _presenter;
      private readonly GridViewBinder<SimulationUpdateStatusDTO> _gridViewBinder;

      public SimulationUpdateView(IShell shell, IImageListRetriever imageListRetriever) : base(shell)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<SimulationUpdateStatusDTO>(gridView);
         gridView.AllowsFiltering = false;
         gridView.MultiSelect = false;
         gridView.OptionsView.RowAutoHeight = true;
      }

      public void AttachPresenter(ISimulationUpdatePresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<SimulationUpdateStatusDTO> dtos)
      {
         _gridViewBinder.BindToSource(dtos.ToList());
         gridView.BestFitColumns();
      }

      public void RefreshData() => gridControl.RefreshDataSource();

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.Bind(x => x.Name)
            .WithCaption(AppConstants.Captions.Name)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Status)
            .WithCaption(AppConstants.Captions.UpdateStatus)
            .WithRepository(statusRepositoryFor)
            .AsReadOnly();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         // Use the extra button because the dedicated cancel closes the dialog. We want to stop the updates, but not close
         // leaving the summary of progress visible.
         ExtraVisible = true;
         CancelVisible = false;
         ButtonExtra.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton);
         OkEnabled = false;
         ExtraEnabled = true;

         descriptionLabel.AsDescription();
         descriptionLabel.Text = AppConstants.Captions.CancelSimulationUpdateClarification.FormatForDescription();
      }

      protected override void ExtraClicked() => _presenter.Cancel();

      public void ShowCompleted()
      {
         ExtraEnabled = false;
         OkEnabled = true;
      }

      private RepositoryItem statusRepositoryFor(SimulationUpdateStatusDTO dto)
      {
         var statusRepositoryItem = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return statusRepositoryItem.AddItem(dto.Status, dto.StatusIcon, dto.StatusDisplay);
      }
   }
}