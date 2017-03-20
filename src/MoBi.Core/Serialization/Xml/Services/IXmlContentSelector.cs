using System.Collections.Generic;
using System.Xml.Linq;

namespace MoBi.Core.Serialization.Xml.Services
{
   public interface IXmlContentSelector
   {
      IEnumerable<XElement> SelectFrom(IEnumerable<XElement> possibleElements, string searchedEntityType);
   }
}