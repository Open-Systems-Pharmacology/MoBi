using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Tasks
{
   public class XmlContentSelector : IXmlContentSelector
   {
      private readonly IMoBiApplicationController _applicationController;

      public XmlContentSelector(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public IEnumerable<XElement> SelectFrom(IEnumerable<XElement> possibleElements, string searchedEntityType)
      {
         using (var modal = _applicationController.Start<IModalPresenter>())
         {
            var presenter = _applicationController.Start<ISelectManyPresenter<XElement>>();
            modal.Text = AppConstants.Captions.SelectEntitiesToLoad(searchedEntityType);
            presenter.InitializeWith(possibleElements);
            modal.Encapsulate(presenter);
            return modal.Show() ? presenter.Selections : Enumerable.Empty<XElement>();
         }
      }
   }
}