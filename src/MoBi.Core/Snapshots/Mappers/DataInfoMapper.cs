using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public class DataInfoMapper : DataInfoMapper<MoBiProject>
{
   public DataInfoMapper(ExtendedPropertyMapper extendedPropertyMapper, IDimension molWeightDimension) : base(extendedPropertyMapper, molWeightDimension)
   {
   }
}