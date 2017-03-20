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
      string CreateFrom(IEnumerable<IReactionPartnerBuilder> educts, IEnumerable<IReactionPartnerBuilder> products);
      string CreateFrom(IEnumerable<IReactionPartner> educts, IEnumerable<IReactionPartner> products);
   }

   public class StoichiometricStringCreator : IStoichiometricStringCreator
   {
      private readonly IReactionPartnerToReactionPartnerBuilderMapper _reactionPartnerToReactionPartnerBuilderMapper;

      public StoichiometricStringCreator(IReactionPartnerToReactionPartnerBuilderMapper reactionPartnerToReactionPartnerBuilderMapper)
      {
         _reactionPartnerToReactionPartnerBuilderMapper = reactionPartnerToReactionPartnerBuilderMapper;
      }

      public string CreateFrom(IEnumerable<IReactionPartnerBuilder> educts, IEnumerable<IReactionPartnerBuilder> products)
      {
         return createStoiciometricSubString(educts) + " => " + createStoiciometricSubString(products);
      }

      public string CreateFrom(IEnumerable<IReactionPartner> educts, IEnumerable<IReactionPartner> products)
      {
         var mappedEducts = educts.MapAllUsing(_reactionPartnerToReactionPartnerBuilderMapper);
         var mappedProducts = products.MapAllUsing(_reactionPartnerToReactionPartnerBuilderMapper);
         return CreateFrom(mappedEducts,mappedProducts);
      }

      private string createStoiciometricSubString(IEnumerable<IReactionPartnerBuilder> educts)
      {
         var reactionPartnerBuilders = educts as IList<IReactionPartnerBuilder> ?? educts.ToList();
         
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

   public interface IReactionPartnerToReactionPartnerBuilderMapper:IMapper<IReactionPartner,IReactionPartnerBuilder>
   {
   }

   class ReactionPartnerToReactionPartnerBuilderMapper : IReactionPartnerToReactionPartnerBuilderMapper
   {
      public IReactionPartnerBuilder MapFrom(IReactionPartner input)
      {
         return new ReactionPartnerBuilder(input.Partner.Name,input.StoichiometricCoefficient);
      }
   }
}