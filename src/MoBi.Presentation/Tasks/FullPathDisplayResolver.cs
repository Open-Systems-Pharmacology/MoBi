using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Mappers;

namespace MoBi.Presentation.Tasks
{
   public class FullPathDisplayResolver : IFullPathDisplayResolver
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      public FullPathDisplayResolver(IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper)
      {
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      public string FullPathFor(IObjectBase objectBase, bool addSimulationName = false)
      {
         var quantity = objectBase as IQuantity;
         if (quantity != null)
            return _quantityDisplayPathMapper.DisplayPathAsStringFor(quantity, addSimulationName);

         var entity = objectBase as IEntity;
         if (entity != null)
            return entity.EntityPath();

         return objectBase.Name;
      }
   }
}