﻿using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditNeighborhoodBuilderView : BaseUserControl, IEditNeighborhoodBuilderView
   {
      private IEditNeighborhoodBuilderPresenter _presenter;
      private readonly ScreenBinder<ObjectBaseDTO> _screenBinder = new ScreenBinder<ObjectBaseDTO>();

      public EditNeighborhoodBuilderView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditNeighborhoodBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Activate()
      {
         ActiveControl = tbName;
      }

      public void AddFirstNeighborView(IView view)
      {
         panelFirstNeighbor.FillWith(view);
      }

      public void AddSecondNeighborView(IView view)
      {
         panelSecondNeighbor.FillWith(view);
      }

      public void BindTo(ObjectBaseDTO objectBaseDTO)
      {
         _screenBinder.BindToSource(objectBaseDTO);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Name)
            .To(tbName);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override bool HasError => _screenBinder.HasError;
   }
}