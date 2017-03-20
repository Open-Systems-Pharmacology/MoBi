using OSPSuite.Utility;

using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IImportedQuantityToParameterStartValueMapper : IMapper<ImportedQuantityDTO, IParameterStartValue>
   {
      
   }

   public class ImportedQuantityToParameterStartValueMapper : IImportedQuantityToParameterStartValueMapper
   {
      private readonly IParameterStartValuesCreator _psvCreator;

      public ImportedQuantityToParameterStartValueMapper(IParameterStartValuesCreator psvCreator)
      {
         _psvCreator = psvCreator;
      }

      public IParameterStartValue MapFrom(ImportedQuantityDTO input)
      {
         var psv = _psvCreator.CreateParameterStartValue(input.Path, input.QuantityInBaseUnit, input.Dimension);
         psv.DisplayUnit = input.DisplayUnit;

         return psv;
      }
   }
}
