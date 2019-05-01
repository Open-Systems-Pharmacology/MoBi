using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_ProductReactionPartnerContextMenuSpecificationFactory : ContextSpecification<IReactionProductContextMenuSpecificationFactory>
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
}