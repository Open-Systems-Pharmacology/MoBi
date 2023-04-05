using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Mapper
{
   public abstract class concern_for_ReactionBuildingBlockToReactionDataTableMapper : ContextSpecification<ReactionBuildingBlockToReactionDataTableMapper>
   {
      protected IMoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      protected DataTable _result;

      protected override void Context()
      {
         sut = new ReactionBuildingBlockToReactionDataTableMapper(
            new StoichiometricStringCreator(
               new ReactionPartnerToReactionPartnerBuilderMapper()));

         _moBiReactionBuildingBlock = new MoBiReactionBuildingBlock();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(new []{_moBiReactionBuildingBlock});
      }
   }

   public class When_mapping_multiple_reactions : concern_for_ReactionBuildingBlockToReactionDataTableMapper
   {
      protected override void Context()
      {
         base.Context();
         getReactions().Each(_moBiReactionBuildingBlock.Add);
      }

      [Observation]
      public void yields_correct_number_of_data_rows()
      {
         _result.Rows.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void maps_values_to_columns_correclty()
      {
         var row = _result.Rows[0];

         row.Field<string>(AppConstants.Captions.Name).ShouldBeEqualTo("name");
         row.Field<string>(AppConstants.Captions.Stoichiometry).ShouldBeEqualTo("2 C => D1");
         row.Field<string>(AppConstants.Captions.Kinetic).ShouldBeEqualTo("k1*C");
         row.Field<string>(AppConstants.Captions.Description).ShouldBeEqualTo("description");
      }

      private IEnumerable<IReactionBuilder> getReactions()
      {
         var r1 = new ReactionBuilder();
         r1.AddEduct(new ReactionPartnerBuilder("C", 2.0));
         r1.AddProduct(new ReactionPartnerBuilder("D1", 1.0));
         r1.Name = "name";
         r1.WithKinetic(new ExplicitFormula("k1*C"));
         r1.Description = "description";
         
         yield return r1;

         yield return A.Fake<IReactionBuilder>();
      }
   }
}
