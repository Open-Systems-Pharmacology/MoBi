using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Assets;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectManyPresenter<T> : IInitializablePresenter<IEnumerable<T>>, IPresenter<ISelectManyView<T>>
   {
      IEnumerable<T> Selections { get; }
      string GetName(T item);
   }

   public abstract class SelectManyPresenter<T> : AbstractPresenter<ISelectManyView<T>, ISelectManyPresenter<T>>, ISelectManyPresenter<T>
   {
      private readonly IItemToListItemMapper<T> _itemToListItemMapper;
      private IEnumerable<T> _allItems;

      protected SelectManyPresenter(ISelectManyView<T> view, IItemToListItemMapper<T> itemToListItemMapper)
         : base(view)
      {
         _itemToListItemMapper = itemToListItemMapper;
         _itemToListItemMapper.Initialize(GetName);
      }

      public void InitializeWith(IEnumerable<T> allItems)
      {
         _allItems = allItems;
         _view.InitializeWith(_allItems.MapAllUsing(_itemToListItemMapper).OrderBy(x => x.DisplayName));
      }

      public IEnumerable<T> Selections
      {
         get { return _view.Selections.Select(x => x.Item); }
      }

      public abstract string GetName(T item);
   }

   public class SelectXmlElementPresenter : SelectManyPresenter<XElement>
   {
      public SelectXmlElementPresenter(ISelectManyView<XElement> view, IItemToListItemMapper<XElement> mapper) : base(view, mapper)
      {
      }

      public override string GetName(XElement item)
      {
         var name = item.GetAttribute(AppConstants.XmlNames.NameAttribute);
         if (name.Equals(string.Empty))
         {
            throw new MoBiException("xElement has not requiered attribute name");
         }
         return name;
      }
   }

   public class SelectObjectBasePresenter<T> : SelectManyPresenter<T> where T : IObjectBase
   {
      public SelectObjectBasePresenter(ISelectManyView<T> view, IItemToListItemMapper<T> mapper) : base(view, mapper)
      {
      }

      public override string GetName(T item)
      {
         return item.Name;
      }
   }
}