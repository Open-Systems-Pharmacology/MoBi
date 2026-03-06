using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Formatters;
using OSPSuite.DataBinding;

namespace MoBi.UI.Views
{
   public partial class EditMoleculeCalculationMethodsView : BaseUserControl, IEditMoleculeCalculationMethodsView
   {
      private readonly IMoleculeUsedCalculationMethodsDTOToTreeNodeMapper _moleculeUsedCalculationMethodsDTOToTreeNodeMapper;
      private IEditMoleculeCalculationMethodsPresenter _presenter;
      private readonly UxTreeView _treeView;
      private readonly GridViewBinder<UsedCalculationMethodDTO> _gridBinder;

      public EditMoleculeCalculationMethodsView(IMoleculeUsedCalculationMethodsDTOToTreeNodeMapper moleculeUsedCalculationMethodsDTOToTreeNodeMapper,
         IImageListRetriever imageListRetriever)
      {
         _moleculeUsedCalculationMethodsDTOToTreeNodeMapper = moleculeUsedCalculationMethodsDTOToTreeNodeMapper;
         InitializeComponent();
         _treeView = new UxTreeView { StateImageList = imageListRetriever.AllImages16x16 };
         treeViewPanel.FillWith(_treeView);
         _gridBinder = new GridViewBinder<UsedCalculationMethodDTO>(gridViewCalculationMethods);
         _treeView.OptionsSelection.MultiSelect = false;

         _gridBinder.Bind(dto => dto.Category)
            .AsReadOnly()
            .WithFormat(new UsedCalculationMethodCategoryFormatter());

         _gridBinder.Bind(dto => dto.CalculationMethodName)
            .WithCaption(AppConstants.Captions.CalculationMethod)
            .WithEditRepository(getComboboxRepositoryItem)
            .OnValueUpdating += onCalculationMethodChanged;

         _treeView.SelectedNodeChanged += node => OnEvent(() => updateForSelectedNode(node));
      }

      private void onCalculationMethodChanged(UsedCalculationMethodDTO dto, PropertyValueSetEventArgs<string> e)
      {
         _presenter.SetCalculationMethod(dto, e.OldValue, e.NewValue);
      }

      private RepositoryItem getComboboxRepositoryItem(UsedCalculationMethodDTO dto)
      {
         var comboBox = new UxRepositoryItemComboBox(gridViewCalculationMethods);
         comboBox.FillComboBoxRepositoryWith(_presenter.GetCalculationMethodsForCategory(dto.Category));
         return comboBox;
      }

      private void updateForSelectedNode(ITreeNode node)
      {
         if (node == null)
            return;
         _presenter.UpdateForSelectedMolecule(node.TagAsObject as MoleculeUsedCalculationMethodsDTO);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.CalculationMethods;
         ApplicationIcon = ApplicationIcons.Permeability;
         gridViewCalculationMethods.OptionsView.ShowGroupPanel = false;

         layoutControlItemCalculationMethods.TextLocation = Locations.Top;
         layoutControlItemCalculationMethods.TextVisible = true;
         layoutControlItemCalculationMethods.Text = AppConstants.Captions.UsedCalculationMethods.FormatForLabel();

         layoutControlItemMolecules.TextLocation = Locations.Top;
         layoutControlItemMolecules.TextVisible = true;
         layoutControlItemMolecules.Text = AppConstants.Captions.Molecules.FormatForLabel();
      }

      public void AttachPresenter(IEditMoleculeCalculationMethodsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IReadOnlyList<MoleculeUsedCalculationMethodsDTO> molecules)
      {
         var nodes = molecules.MapAllUsing(_moleculeUsedCalculationMethodsDTOToTreeNodeMapper);
         _treeView.Clear();
         nodes.Each(node => _treeView.AddNode(node));
         if (nodes.Any())
            _presenter.UpdateForSelectedMolecule(nodes.First().TagAsObject as MoleculeUsedCalculationMethodsDTO);
      }

      public void BindTo(IReadOnlyList<UsedCalculationMethodDTO> usedCalculationMethods)
      {
         _gridBinder.BindToSource(usedCalculationMethods);
      }
   }
}