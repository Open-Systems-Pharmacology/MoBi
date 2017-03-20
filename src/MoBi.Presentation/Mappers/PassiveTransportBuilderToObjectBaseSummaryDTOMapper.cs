using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IPassiveTransportBuilderToObjectBaseSummaryDTOMapper : IMapper<ITransportBuilder, ObjectBaseSummaryDTO>
   {
       
   }

   public class PassiveTransportBuilderToObjectBaseSummaryDTOMapper : IPassiveTransportBuilderToObjectBaseSummaryDTOMapper
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public PassiveTransportBuilderToObjectBaseSummaryDTOMapper(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public ObjectBaseSummaryDTO MapFrom(ITransportBuilder transportBuilder)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            ApplicationIcon = ApplicationIcons.PassiveTransport,
            EntityName = transportBuilder.Name
         };
         dto.AddToDictionary(AppConstants.Captions.Type, ObjectTypes.PassiveTransportBuilder);
         dto.AddToDictionary(AppConstants.Captions.SourceDescriptor, transportBuilder.SourceCriteria.ToString());
         dto.AddToDictionary(AppConstants.Captions.TargetDescriptor, transportBuilder.TargetCriteria.ToString());
         dto.AddToDictionary(AppConstants.Captions.ForAll, transportBuilder.ForAll.ToString());

         if (transportBuilder.ForAll) return dto;

         dto.AddToDictionary(AppConstants.Captions.MoleculesToInclude, transportBuilder.MoleculeList.MoleculeNames.ToString(", "));
         dto.AddToDictionary(AppConstants.Captions.MoleculesToExclude, transportBuilder.MoleculeList.MoleculeNamesToExclude.ToString(", "));

         return dto;
      }
   }
}