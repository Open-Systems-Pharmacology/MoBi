using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectionPresenter<TSelectable>
   {
      void InitializeWith(IEnumerable<TSelectable> allItems, Func<TSelectable, IComparable> orderedBy = null);
   }

   public abstract class SelectionPresenter<TView, TPresenter, TSelectable> : AbstractPresenter<TView, TPresenter>, ISelectionPresenter<TSelectable> where TView : IView<TPresenter>, ISelectionView<TSelectable> where TPresenter : IPresenter
   {
      private readonly IItemToListItemMapper<TSelectable> _itemToListItemMapper;
      private IEnumerable<TSelectable> _allItems;

      protected SelectionPresenter(TView view, IItemToListItemMapper<TSelectable> itemToListItemMapper) : base(view)
      {
         _itemToListItemMapper = itemToListItemMapper;
         _itemToListItemMapper.Initialize(GetName);
      }

      public void InitializeWith(IEnumerable<TSelectable> allItems, Func<TSelectable, IComparable> orderedBy = null)
      {
         _allItems = allItems;
         _view.InitializeWith(MapAllItems(orderedBy ?? GetName));
      }

      protected virtual IEnumerable<ListItemDTO<TSelectable>> MapAllItems(Func<TSelectable, IComparable> orderBy)
      {
         return _allItems.MapAllUsing(_itemToListItemMapper).OrderBy(x => orderBy(x.Item));
      }

      public abstract string GetName(TSelectable item);
   }
}