using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Services
{
   public interface ISerializationContextFactory
   {
      SerializationContext Create(SerializationContext parentSerializationContext = null);
   }

   public class SerializationContextFactory : ISerializationContextFactory
   {
      private readonly ISerializationDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public SerializationContextFactory(ISerializationDimensionFactory dimensionFactory, IObjectBaseFactory objectBaseFactory,
         ICloneManagerForModel cloneManagerForModel)
      {
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
      }

      public SerializationContext Create(SerializationContext parentSerializationContext = null)
      {
         var serializationContext = SerializationTransaction.Create(_dimensionFactory, _objectBaseFactory, new WithIdRepository(), _cloneManagerForModel);

         if (parentSerializationContext != null)
         {
            parentSerializationContext.Repositories.Each(serializationContext.AddRepository);
            parentSerializationContext.IdRepository.All().Each(serializationContext.Register);
         }

         return serializationContext;
      }
   }
}