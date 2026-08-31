using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class SelectCommitTargetView : BaseModalView, ISelectCommitTargetView
   {
      private ISelectCommitTargetPresenter _presenter;

      public SelectCommitTargetView()
      {
         InitializeComponent();
         cmbModule.SelectedIndexChanged += (o, e) => OnEvent(moduleChanged);
      }

      public void AttachPresenter(ISelectCommitTargetPresenter presenter) => _presenter = presenter;

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemModule.Text = AppConstants.Captions.Module.FormatForLabel();
         layoutItemParameterValues.Text = ObjectTypes.ParameterValuesBuildingBlock.FormatForLabel();
         cmbModule.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         cmbParameterValues.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
         descriptionLabel.AsDescription();
         Text = AppConstants.Captions.SelectCommitTarget;
      }

      public void BindModules(IEnumerable<ListItemDTO<Module>> modules, Module selectedModule)
      {
         fill(cmbModule, modules);
         select(cmbModule, item => Equals(item.DowncastTo<ListItemDTO<Module>>().Item, selectedModule));
      }

      public void BindParameterValues(IEnumerable<ListItemDTO<ParameterValuesBuildingBlock>> parameterValues, ParameterValuesBuildingBlock selectedParameterValues)
      {
         fill(cmbParameterValues, parameterValues);
         select(cmbParameterValues, item => Equals(item.DowncastTo<ListItemDTO<ParameterValuesBuildingBlock>>().Item, selectedParameterValues));
      }

      public Module SelectedModule => (cmbModule.SelectedItem as ListItemDTO<Module>)?.Item;

      public ParameterValuesBuildingBlock SelectedParameterValues => (cmbParameterValues.SelectedItem as ListItemDTO<ParameterValuesBuildingBlock>)?.Item;

      public void SetDescription(string description) => descriptionLabel.Text = description;

      private void moduleChanged()
      {
         if (SelectedModule != null)
            _presenter.ModuleChanged();
      }

      private static void fill<T>(UxComboBoxEdit comboBox, IEnumerable<ListItemDTO<T>> items)
      {
         comboBox.Properties.Items.Clear();
         items.Each(item => comboBox.Properties.Items.Add(item));
      }

      private static void select(UxComboBoxEdit comboBox, System.Func<object, bool> isSelectedItem)
      {
         for (var i = 0; i < comboBox.Properties.Items.Count; i++)
         {
            if (!isSelectedItem(comboBox.Properties.Items[i]))
               continue;

            comboBox.SelectedIndex = i;
            return;
         }
      }
   }
}
