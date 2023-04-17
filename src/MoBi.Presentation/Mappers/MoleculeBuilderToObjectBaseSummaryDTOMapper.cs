using System.Globalization;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderToObjectBaseSummaryDTOMapper : IMapper<MoleculeBuilder, ObjectBaseSummaryDTO>
   {
      
   }

   public class MoleculeBuilderToObjectBaseSummaryDTOMapper : IMoleculeBuilderToObjectBaseSummaryDTOMapper
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public MoleculeBuilderToObjectBaseSummaryDTOMapper(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public ObjectBaseSummaryDTO MapFrom(MoleculeBuilder moleculeBuilder)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            ApplicationIcon = ApplicationIcons.Molecule,
            EntityName = moleculeBuilder.Name,
         };

         dto.AddToDictionary(AppConstants.Captions.Type, _objectTypeResolver.TypeFor(moleculeBuilder));
         dto.AddToDictionary(AppConstants.Captions.NumberOfParameters, moleculeBuilder.Parameters.Count().ToString(CultureInfo.InvariantCulture));
         dto.AddToDictionary(AppConstants.Captions.TransporterMolecules, moleculeBuilder.TransporterMoleculeContainerCollection.Count().ToString(CultureInfo.InvariantCulture));

         return dto;
         
      }
   }
}