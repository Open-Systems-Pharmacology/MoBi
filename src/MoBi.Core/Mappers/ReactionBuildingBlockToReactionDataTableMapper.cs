using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Mappers
{
   public interface IReactionBuildingBlockToReactionDataTableMapper : IMapper<IReactionBuildingBlock, DataTable>
   {
      
   }

   public class ReactionBuildingBlockToReactionDataTableMapper : IReactionBuildingBlockToReactionDataTableMapper
   {
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;
      private static readonly string _name = AppConstants.Captions.Name;
      private static readonly string _stoichiometry = AppConstants.Captions.Stoichiometry;
      private static readonly string _kinetic = AppConstants.Captions.Kinetic;
      private const string _description = AppConstants.Captions.Description;

      public ReactionBuildingBlockToReactionDataTableMapper(IStoichiometricStringCreator stoichiometricStringCreator)
      {
         _stoichiometricStringCreator = stoichiometricStringCreator;
      }

      private static DataTable generateEmptyReactionDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn(_name);
         dt.AddColumn(_stoichiometry);
         dt.AddColumn(_kinetic);
         dt.AddColumn(_description);
         dt.TableName = AppConstants.Captions.Reactions;
         return dt;
      }

      private void exportReactionBuildingBlock(IEnumerable<IReactionBuilder> reactionBuildingBlock, DataTable dt)
      {
         reactionBuildingBlock.Each(reactionBuilder => exportReactionBuilder(reactionBuilder, dt));
      }

      private void exportReactionBuilder(IReactionBuilder reactionBuilder, DataTable dt)
      {
         var stoichiometricString = getStoichiometricStringFromReactionBuilder(reactionBuilder);
         var kinetic = reactionBuilder.Formula.ToString();
         var row = dt.Rows.Add();
         row[_name] = reactionBuilder.Name;
         row[_stoichiometry] = stoichiometricString;
         row[_kinetic] = kinetic;
         row[_description] = reactionBuilder.Description;
      }

      private string getStoichiometricStringFromReactionBuilder(IReactionBuilder reactionBuilder)
      {
         return _stoichiometricStringCreator.CreateFrom(reactionBuilder.Educts, reactionBuilder.Products);
      }

      public DataTable MapFrom(IReactionBuildingBlock input)
      {
         var reactionDataTable = generateEmptyReactionDataTable();

         exportReactionBuildingBlock(input, reactionDataTable);
         return reactionDataTable;
      }
   }
}