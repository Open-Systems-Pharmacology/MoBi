using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class TagsView : BaseUserControl, ITagsView
   {
      private readonly GridViewBinder<TagDTO> _gridBinder;
      private readonly UxAddAndRemoveButtonRepository _addRemoveButtonRepository = new UxAddAndRemoveButtonRepository();
      private ITagsPresenter _presenter;

      public TagsView()
      {
         InitializeComponent();
         _gridBinder = new GridViewBinder<TagDTO>(gridView);
         Caption = AppConstants.Captions.Tags;
      }

      public void AttachPresenter(ITagsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<TagDTO> tags)
      {
         _gridBinder.BindToSource(tags);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridBinder.Bind(tag => tag.Value)
            .WithCaption(AppConstants.Captions.Tag)
            .AsReadOnly();

         _addRemoveButtonRepository.ButtonClick += (o, e) => OnEvent(() => onButtonClicked(e, _gridBinder.FocusedElement));

         _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _addRemoveButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH * 2);

         btnAddTag.Click += (o, e) => OnEvent(_presenter.AddNewTag);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnAddTag.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddTag);

         layoutControlItemAddTag.AdjustButtonSize(layoutControl);
      }

      private void onButtonClicked(ButtonPressedEventArgs buttonPressedEventArgs, TagDTO tagDTO)
      {
         var pressedButton = buttonPressedEventArgs.Button;
         if (pressedButton.Kind.Equals(ButtonPredefines.Plus))
            _presenter.AddNewTag();
         else
            _presenter.RemoveTag(tagDTO);
      }
   }
}