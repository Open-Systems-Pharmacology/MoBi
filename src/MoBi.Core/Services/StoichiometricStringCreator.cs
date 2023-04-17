using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IStoichiometricStringCreator
   {
      string CreateFrom(IEnumerable<ReactionPartnerBuilder> educts, IEnumerable<ReactionPartnerBuilder> products);
      string CreateFrom(IEnumerable<ReactionPartner> educts, IEnumerable<ReactionPartner> products);
   }

   public class StoichiometricStringCreator : IStoichiometricStringCreator
   {
      private readonly IReactionPartnerToReactionPartnerBuilderMapper _reactionPartnerToReactionPartnerBuilderMapper;

      public StoichiometricStringCreator(IReactionPartnerToReactionPartnerBuilderMapper reactionPartnerToReactionPartnerBuilderMapper)
      {
         _reactionPartnerToReactionPartnerBuilderMapper = reactionPartnerToReactionPartnerBuilderMapper;
      }

      public string CreateFrom(IEnumerable<ReactionPartnerBuilder> educts, IEnumerable<ReactionPartnerBuilder> products)
      {
         return createStoiciometricSubString(educts) + " => " + createStoiciometricSubString(products);
      }

      public string CreateFrom(IEnumerable<ReactionPartner> educts, IEnumerable<ReactionPartner> products)
      {
         var mappedEducts = educts.MapAllUsing(_reactionPartnerToReactionPartnerBuilderMapper);
         var mappedProducts = products.MapAllUsing(_reactionPartnerToReactionPartnerBuilderMapper);
         return CreateFrom(mappedEducts,mappedProducts);
      }

      private string createStoiciometricSubString(IEnumerable<ReactionPartnerBuilder> educts)
      {
         var reactionPartnerBuilders = educts as IList<ReactionPartnerBuilder> ?? educts.ToList();
         
         if (educts == null || !reactionPartnerBuilders.Any()) return string.Empty;
         
         var result = new StringBuilder();
         var last = reactionPartnerBuilders.Last();
         foreach (var educt in reactionPartnerBuilders)
         {
            if (!ValueComparer.AreValuesEqual(educt.StoichiometricCoefficient, 1.0))
               result.AppendFormat("{0} {1}", educt.StoichiometricCoefficient, educt.MoleculeName);
            else
               result.AppendFormat("{0}", educt.MoleculeName);

            if (!educt.Equals(last))
            {
               result.Append(" + ");
            }
         }
         return result.ToString();
      }
   }

   public interface IReactionPartnerToReactionPartnerBuilderMapper:IMapper<ReactionPartner,ReactionPartnerBuilder>
   {
   }

   class ReactionPartnerToReactionPartnerBuilderMapper : IReactionPartnerToReactionPartnerBuilderMapper
   {
      public ReactionPartnerBuilder MapFrom(ReactionPartner input)
      {
         return new ReactionPartnerBuilder(input.Partner.Name,input.StoichiometricCoefficient);
      }
   }
}