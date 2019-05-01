using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EductReactionPartnerContextMenuSpecificationFactory : ContextSpecification<IReactionEductPartnerContextMenuSpecificationFactory>
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
}