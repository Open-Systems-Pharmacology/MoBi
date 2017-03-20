using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EductReactionPartnerContextMenuSpecificationFactory : ContextSpecificationWithLocalContainer<IReactionEductPartnerContextMenuSpecificationFactory>
   {
      protected override void Context()
      {
         sut = new ReactionEductPartnerContextMenuSpecificationFactory();
      }
   }

   public class When_asked_if_satisfied_by_DTOReactionBuilderDTO : concern_for_EductReactionPartnerContextMenuSpecificationFactory
   {
      private bool _result;

      protected override void Because()
      {
         _result = sut.IsSatisfiedBy(new ReactionPartnerBuilderDTO(new ReactionPartnerBuilder()), A.Fake<IReactionEductsPresenter>());
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_creating_a_context_menu_for_the_educt_of_a_reaction : concern_for_EductReactionPartnerContextMenuSpecificationFactory
   {
      private IReactionEductsPresenter _reactionEductsPresenter;
      private IReactionBuilder _reactionBuilder;
      private ReactionPartnerBuilderDTO _dto;

      protected override void Context()
      {
         base.Context();
         _reactionEductsPresenter = A.Fake<IReactionEductsPresenter>();
         _reactionBuilder = new ReactionBuilder();
         var reactionPartnerBuilder = new ReactionPartnerBuilder("A", 1);
         _reactionBuilder.AddProduct(reactionPartnerBuilder);
         _dto = new ReactionPartnerBuilderDTO(reactionPartnerBuilder)
         {
            IsEduct = false,
         };

         A.CallTo(() => _reactionEductsPresenter.Subject).Returns(_reactionBuilder);
      }

      [Observation]
      public void should_return_a_context_menu_for_educt()
      {
         sut.CreateFor(_dto, _reactionEductsPresenter).ShouldBeAnInstanceOf<ContextMenuForEductBuilder>();
      }
   }
}