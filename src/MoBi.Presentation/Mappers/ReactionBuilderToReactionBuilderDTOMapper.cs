using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionBuilderToReactionBuilderDTOMapper : IMapper<IReactionBuilder, ReactionBuilderDTO>
   {
   }

   public class ReactionBuilderToReactionBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IReactionBuilderToReactionBuilderDTOMapper
   {
      private readonly IReactionPartnerBuilderToDTOReactionBuilderPartnerMapper _reactionPartnerBuilderToDTOReactionBuilderPartnerMapper;
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;

      public ReactionBuilderToReactionBuilderDTOMapper(IReactionPartnerBuilderToDTOReactionBuilderPartnerMapper reactionPartnerBuilderToDtoReactionBuilderPartner,
         IStoichiometricStringCreator stoichiometricStringCreator)
      {
         _reactionPartnerBuilderToDTOReactionBuilderPartnerMapper = reactionPartnerBuilderToDtoReactionBuilderPartner;
         _stoichiometricStringCreator = stoichiometricStringCreator;
      }

      public ReactionBuilderDTO MapFrom(IReactionBuilder reactionBuilder)
      {
         var dto = new ReactionBuilderDTO(reactionBuilder);
         MapProperties(reactionBuilder, dto);
         dto.StoichiometricFormula = _stoichiometricStringCreator.CreateFrom(reactionBuilder.Educts, reactionBuilder.Products);
         dto.Educts = reactionBuilder.Educts.MapAllUsing(_reactionPartnerBuilderToDTOReactionBuilderPartnerMapper).ToBindingList();
         dto.Educts.Each(item => item.IsEduct = true);
         dto.Products = reactionBuilder.Products.MapAllUsing(_reactionPartnerBuilderToDTOReactionBuilderPartnerMapper).ToBindingList();
         dto.Products.Each(item => item.IsEduct = false);

         if (reactionBuilder.Formula != null)
         {
            dto.Kinetic = reactionBuilder.Formula.ToString();
         }
         return dto;
      }
   }

   public interface IReactionPartnerBuilderToDTOReactionBuilderPartnerMapper : IMapper<IReactionPartnerBuilder, ReactionPartnerBuilderDTO>
   {
   }

   internal class ReactionPartnerBuilderToDTOReactionBuilderPartnerMapper : IReactionPartnerBuilderToDTOReactionBuilderPartnerMapper
   {
      public ReactionPartnerBuilderDTO MapFrom(IReactionPartnerBuilder reactionPartnerBuilder)
      {
         return new ReactionPartnerBuilderDTO(reactionPartnerBuilder);
      }
   }
}