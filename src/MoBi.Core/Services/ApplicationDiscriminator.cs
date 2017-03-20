using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Services
{
   public class ApplicationDiscriminator : IApplicationDiscriminator
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public ApplicationDiscriminator(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public string DiscriminatorFor<T>(T item) where T : IObjectBase
      {
         return item.GetType().Name;
      }

      public IReadOnlyCollection<IObjectBase> AllFor(string discriminator)
      {
         return _projectRetriever.Current.All().Where(bb => string.Equals(DiscriminatorFor(bb), discriminator)).ToList();
      }
   }
}