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

      public InitialCondition MapFrom(ImportedQuantityDTO importedQuantityDTO)
      {
         var initialCondition = _initialConditionsCreator.CreateInitialCondition(importedQuantityDTO.ContainerPath, importedQuantityDTO.Name, importedQuantityDTO.Dimension, importedQuantityDTO.DisplayUnit);
         initialCondition.Value = importedQuantityDTO.QuantityInBaseUnit;
         initialCondition.DisplayUnit = importedQuantityDTO.DisplayUnit;
         initialCondition.IsPresent = importedQuantityDTO.IsPresent;
         initialCondition.NegativeValuesAllowed = importedQuantityDTO.NegativeValuesAllowed;

         if(importedQuantityDTO.IsQuantitySpecified)
            initialCondition.Value = importedQuantityDTO.QuantityInBaseUnit;

         if (importedQuantityDTO.IsScaleDivisorSpecified)
            initialCondition.ScaleDivisor = importedQuantityDTO.ScaleDivisor;

         return initialCondition;
      }
   }
}
