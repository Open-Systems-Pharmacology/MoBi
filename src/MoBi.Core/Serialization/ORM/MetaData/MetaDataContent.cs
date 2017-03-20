using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public class MetaDataContent : MetaData<int>
   {
      public virtual byte[] Data { get; set; }
   }
}