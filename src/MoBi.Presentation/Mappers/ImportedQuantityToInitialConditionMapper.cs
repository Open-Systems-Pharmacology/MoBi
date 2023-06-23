using OSPSuite.Utility;

using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IImportedQuantityToInitialConditionMapper : IMapper<ImportedQuantityDTO, InitialCondition>
   {

   }

   public class ImportedQuantityToInitialConditionMapper : IImportedQuantityToInitialConditionMapper
   {
      private readonly IInitialConditionsCreator _initialConditionsCreator;

      public ImportedQuantityToInitialConditionMapper(IInitialConditionsCreator initialConditionsCreator)
      {
         _initialConditionsCreator = initialConditionsCreator;
      }

      public InitialCondition MapFrom(ImportedQuantityDTO input)
      {
         var msv = _initialConditionsCreator.CreateInitialCondition(input.ContainerPath, input.Name, input.Dimension, input.DisplayUnit);
         msv.Value = input.QuantityInBaseUnit;
         msv.DisplayUnit = input.DisplayUnit;
         msv.IsPresent = input.IsPresent;
         msv.NegativeValuesAllowed = input.NegativeValuesAllowed;

         if(input.IsQuantitySpecified)
            msv.Value = input.QuantityInBaseUnit;

         if (input.IsScaleDivisorSpecified)
            msv.ScaleDivisor = input.ScaleDivisor;

         return msv;
      }
   }
}
