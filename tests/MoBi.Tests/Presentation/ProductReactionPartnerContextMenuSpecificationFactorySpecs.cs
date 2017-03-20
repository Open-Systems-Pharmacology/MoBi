using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_ProductReactionPartnerContextMenuSpecificationFactory : ContextSpecificationWithLocalContainer<IReactionProductContextMenuSpecificationFactory>
   {
      protected IReactionProductsPresenter _reactionProductsPresenter;
      protected override void Context()
      {
         _reactionProductsPresenter = A.Fake<IReactionProductsPresenter>();
         sut = new ReactionProductPartnerContextMenuSpecificationFactory();
      }
   }

   public class When_asked_if_satisfied_by_reaction_builder : concern_for_ProductReactionPartnerContextMenuSpecificationFactory
   {
      private bool _result;

      protected override void Because()
      {
         _result = sut.IsSatisfiedBy(new ReactionPartnerBuilderDTO(new ReactionPartnerBuilder()), A.Fake<IReactionProductsPresenter>());
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_creating_a_context_menu_for_the_product_of_a_reaction : concern_for_ProductReactionPartnerContextMenuSpecificationFactory
   {
      
      private IReactionBuilder _reactionBuilder;
      private ReactionPartnerBuilderDTO _dto;

      protected override void Context()
      {
         base.Context();
         
         _reactionBuilder = new ReactionBuilder();
         var reactionPartnerBuilder = new ReactionPartnerBuilder("A", 1);
         _reactionBuilder.AddProduct(reactionPartnerBuilder);
         _dto = new ReactionPartnerBuilderDTO(reactionPartnerBuilder);

         A.CallTo(() => _reactionProductsPresenter.Subject).Returns(_reactionBuilder);
      }

      [Observation]
      public void should_return_a_context_menu_for_product()
      {
         sut.CreateFor(_dto, _reactionProductsPresenter).ShouldBeAnInstanceOf<ContextMenuForProductBuilder>();
      }
   }
}