using System.Globalization;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionBuilderToObjectBaseSummaryDTOMapper : IMapper<ReactionBuilder, ObjectBaseSummaryDTO>
   {
      
   }

   public class ReactionBuilderToObjectBaseSummaryDTOMapper : IReactionBuilderToObjectBaseSummaryDTOMapper
   {
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;

      public ReactionBuilderToObjectBaseSummaryDTOMapper(
         IStoichiometricStringCreator stoichiometricStringCreator,
         IObjectTypeResolver objectTypeResolver)
      {
         _stoichiometricStringCreator = stoichiometricStringCreator;
         _objectTypeResolver = objectTypeResolver;
      }

      public ObjectBaseSummaryDTO MapFrom(ReactionBuilder reactionBuilder)
      {
         var dto = new ObjectBaseSummaryDTO
         {
            ApplicationIcon = ApplicationIcons.Reaction,
            EntityName = reactionBuilder.Name
         };

         dto.AddToDictionary(AppConstants.Captions.Type, _objectTypeResolver.TypeFor(reactionBuilder));
         dto.AddToDictionary(AppConstants.Captions.NumberOfParameters, reactionBuilder.Parameters.Count().ToString(CultureInfo.InvariantCulture));
         dto.AddToDictionary(AppConstants.Captions.Stoichiometry, _stoichiometricStringCreator.CreateFrom(reactionBuilder.Educts, reactionBuilder.Products));
         dto.AddToDictionary(AppConstants.Captions.Kinetic, reactionBuilder.Formula != null ? reactionBuilder.Formula.ToString() : string.Empty);

         return dto;
      }
   }
}