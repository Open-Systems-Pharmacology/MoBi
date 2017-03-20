using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Application
{
   public abstract class concern_for_StoichiometricStringCreater : ContextSpecification<IStoichiometricStringCreator>
   {
      private IReactionPartnerToReactionPartnerBuilderMapper _reactionPartnerToReactionPartnerBuilderMapper;

      protected override void Context()
      {
         _reactionPartnerToReactionPartnerBuilderMapper = A.Fake<IReactionPartnerToReactionPartnerBuilderMapper>();
         sut = new StoichiometricStringCreator(_reactionPartnerToReactionPartnerBuilderMapper);
      }
   }


   public class When_told_to_create_the_string_form_educt_and_product_list : concern_for_StoichiometricStringCreater
   {
      private string _result;
      private IEnumerable<IReactionPartnerBuilder> _educts;
      private IEnumerable<IReactionPartnerBuilder> _products;

      protected override void Context()
      {
         base.Context();
         var educt1 = A.Fake<IReactionPartnerBuilder>();
         educt1.MoleculeName = "E1";
         educt1.StoichiometricCoefficient = 1;
         var educt2 = A.Fake<IReactionPartnerBuilder>();
         educt2.MoleculeName = "E2";
         educt2.StoichiometricCoefficient = 3;
         var product1 = A.Fake<IReactionPartnerBuilder>();
         product1.MoleculeName = "P1";
         product1.StoichiometricCoefficient = 2;
         var product2 = A.Fake<IReactionPartnerBuilder>();
         product2.MoleculeName = "P2";
         product2.StoichiometricCoefficient = 2;
         _educts = new[] {educt1, educt2};
         _products = new[] {product1, product2};
      }

      protected override void Because()
      {
         
         _result = sut.CreateFrom(_educts, _products);
      }

      [Observation]
      public void should_return_the_correct_string()
      {
         _result.ShouldBeEqualTo("E1 + 3 E2 => 2 P1 + 2 P2");
      }
   }
}