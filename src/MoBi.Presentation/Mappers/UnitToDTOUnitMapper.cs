using OSPSuite.Utility;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mappers
{
   public interface IUnitToDTOUnitMapper : IMapper<Unit, Unit>
   {
   }

   public class UnitToDTOUnitMapper : IUnitToDTOUnitMapper
   {
      public Unit MapFrom(Unit unit)
      {
         return new Unit(unit.Name, unit.Factor, unit.Offset);
      }
   }
}