using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Serializer.Xml.Extensions;

namespace MoBi.Presentation.Presenter
{
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
            throw new MoBiException("xElement has not required attribute name");
         }

         return name;
      }
   }
}