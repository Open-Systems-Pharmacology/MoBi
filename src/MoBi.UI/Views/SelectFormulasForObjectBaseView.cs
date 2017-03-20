using System.Collections.Generic;
using System.Linq;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.RepositoryItems;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectFormulasForObjectBaseView : BaseModalView, ISelectFormulasForObjectBaseView
   {
      private ISelectFormulasForObjectBasePresenter _presenter;
      private GridViewBinder<ObjectFormulaDTO> _gridBinder;

      public SelectFormulasForObjectBaseView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridBinder = new GridViewBinder<ObjectFormulaDTO>(grdSelections);
         _gridBinder.Bind(dto => dto.Name).AsReadOnly();
         _gridBinder.Bind(dto => dto.Formula).WithRepository(getComboBox);
      }

      private RepositoryItem getComboBox(ObjectFormulaDTO arg)
      {
         var comboBox = new UxRepositoryItemComboBox(grdSelections);
         comboBox.Items.AddRange(_presenter.GetFormulasFor(arg.ObjectBase).ToArray());
         return comboBox;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.SelectFormulasForObjectBaseSelection;
      }

      public void AttachPresenter(ISelectFormulasForObjectBasePresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IEnumerable<ObjectFormulaDTO> dtos)
      {
         _gridBinder.BindToSource(dtos);
      }
   }

   
}