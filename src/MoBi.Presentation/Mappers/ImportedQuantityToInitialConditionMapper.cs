using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
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
         return _initialConditionsCreator.CreateInitialCondition(
            importedQuantityDTO.ContainerPath, 
            importedQuantityDTO.Name, 
            importedQuantityDTO.Dimension, 
            importedQuantityDTO.DisplayUnit,
            valueOrigin:null,
            isPresent:importedQuantityDTO.IsPresent,
            valueInBaseUnit:importedQuantityDTO.QuantityInBaseUnit,
            scaleDivisor:importedQuantityDTO.IsScaleDivisorSpecified ? importedQuantityDTO.ScaleDivisor : Constants.DEFAULT_SCALE_DIVISOR,
            negativeValuesAllowed:importedQuantityDTO.NegativeValuesAllowed);
      }
   }
}
