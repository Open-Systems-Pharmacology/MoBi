using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_MergeConflictOptions : ContextSpecification<MergeConflictOptions>
   {
   }

   public class When_option_is_skip_all : concern_for_MergeConflictOptions
   {
      protected override void Because()
      {
         sut = MergeConflictOptions.SkipAll;
      }

      [Observation]
      public void is_skip_should_return_true()
      {
         sut.IsSkip().ShouldBeTrue();
      }

      [Observation]
      public void all_others_should_be_false()
      {
         sut.IsReplace().ShouldBeFalse();
      }
   }

   public class When_option_is_replace_all : concern_for_MergeConflictOptions
   {
      protected override void Because()
      {
         sut = MergeConflictOptions.ReplaceAll;
      }

      [Observation]
      public void is_replace_should_return_true()
      {
         sut.IsReplace().ShouldBeTrue();
      }

      [Observation]
      public void all_others_should_be_false()
      {
         sut.IsSkip().ShouldBeFalse();
      }
   }
}