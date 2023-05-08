using OSPSuite.Utility;

using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IImportedQuantityToParameterValueMapper : IMapper<ImportedQuantityDTO, ParameterValue>
   {
      
   }

   public class ImportedQuantityToParameterValueMapper : IImportedQuantityToParameterValueMapper
   {
      private readonly IParameterValuesCreator _psvCreator;

      public ImportedQuantityToParameterValueMapper(IParameterValuesCreator psvCreator)
      {
         _psvCreator = psvCreator;
      }

      public ParameterValue MapFrom(ImportedQuantityDTO input)
      {
         var psv = _psvCreator.CreateParameterValue(input.Path, input.QuantityInBaseUnit, input.Dimension);
         psv.DisplayUnit = input.DisplayUnit;

         return psv;
      }
   }
}
