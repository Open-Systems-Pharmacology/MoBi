using OSPSuite.Utility;

using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mappers
{
   public interface IImportedQuantityToMoleculeStartValueMapper : IMapper<ImportedQuantityDTO, InitialCondition>
   {

   }

   public class ImportedQuantityToMoleculeStartValueMapper : IImportedQuantityToMoleculeStartValueMapper
   {
      private readonly IInitialConditionsCreator _msvCreator;

      public ImportedQuantityToMoleculeStartValueMapper(IInitialConditionsCreator msvCreator)
      {
         _msvCreator = msvCreator;
      }

      public InitialCondition MapFrom(ImportedQuantityDTO input)
      {
         var msv = _msvCreator.CreateInitialCondition(input.ContainerPath, input.Name, input.Dimension, input.DisplayUnit);
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
