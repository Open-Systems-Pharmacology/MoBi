using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public abstract class SelectionPresenter<TView, TPresenter, TSelectable> : AbstractPresenter<TView, TPresenter> where TView : IView<TPresenter>, ISelectionView<TSelectable> where TPresenter : IPresenter
   {
      private readonly IItemToListItemMapper<TSelectable> _itemToListItemMapper;
      private IEnumerable<TSelectable> _allItems;

      protected SelectionPresenter(TView view, IItemToListItemMapper<TSelectable> itemToListItemMapper) : base(view)
      {
         _itemToListItemMapper = itemToListItemMapper;
         _itemToListItemMapper.Initialize(GetName);
      }

      public void InitializeWith(IEnumerable<TSelectable> allItems)
      {
         _allItems = allItems;
         _view.InitializeWith(MapAllItems());
      }

      protected virtual IEnumerable<ListItemDTO<TSelectable>> MapAllItems() => _allItems.MapAllUsing(_itemToListItemMapper);

      public abstract string GetName(TSelectable item);
   }
}