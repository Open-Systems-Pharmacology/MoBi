using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ObjectBaseSummaryView : BaseUserControl, IObjectBaseSummaryView
   {
      private ScreenBinder<ObjectBaseSummaryDTO> _screenBinder;
      private IObjectBaseSummaryPresenter _presenter;
      private readonly GridViewBinder<KeyValuePair<string, string>> _gridViewBinder;

      public ObjectBaseSummaryView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<KeyValuePair<string, string>>(gridView);
      }

      public void AttachPresenter(IObjectBaseSummaryPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ObjectBaseSummaryDTO objectBaseDTO)
      {
         _screenBinder.BindToSource(objectBaseDTO);
         _gridViewBinder.BindToSource(objectBaseDTO.Dictionary.OrderBy(x => x.Key));
         pictureBox.Image = objectBaseDTO.ApplicationIcon;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ObjectBaseSummaryDTO>();
         _screenBinder.Bind(x => x.EntityName).To(_labelName);

         _gridViewBinder.Bind(x => x.Key).WithCaption(AppConstants.Captions.Data).AsReadOnly();
         _gridViewBinder.Bind(x => x.Value).WithCaption(AppConstants.Captions.Value).AsReadOnly();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         customizeGrid();
      }

      private void customizeGrid()
      {
         gridView.AllowsFiltering = false;
         gridView.OptionsMenu.EnableColumnMenu = false;
      }
   }
}
