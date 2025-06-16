using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public class DataColumnMapper : DataColumnMapper<MoBiProject>
{
   private readonly IDimensionFactory _dimensionFactory;

   public DataColumnMapper(DataInfoMapper dataInfoMapper, QuantityInfoMapper quantityInfoMapper, IDimensionFactory dimensionFactory) : base(dataInfoMapper, quantityInfoMapper)
   {
      _dimensionFactory = dimensionFactory;
   }

   protected override IDimension DimensionFrom(DataColumn snapshot)
   {
      return _dimensionFactory.Dimension(snapshot.Dimension);
   }
}