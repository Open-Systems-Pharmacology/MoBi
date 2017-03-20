using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.Helpers;

namespace MoBi.Presentation
{
   public abstract class concern_for_MergeConflictOptions : ContextSpecification<MergeConflictOptions>
   {
   }

   public class when_option_is_skip : concern_for_MergeConflictOptions
   {
      protected override void Because()
      {
         sut = MergeConflictOptions.SkipOnce;
      }

      [Observation]
      public void is_skip_should_return_true()
      {
         sut.IsSkip().ShouldBeTrue();
      }

      [Observation]
      public void others_should_be_false()
      {
         sut.IsAppliedToAll().ShouldBeFalse();
         sut.IsMerge().ShouldBeFalse();
         sut.IsClone().ShouldBeFalse();
         sut.IsReplace().ShouldBeFalse();
      }
   }

   public class when_option_is_skip_all : concern_for_MergeConflictOptions
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
      public void is_applied_to_all_should_return_true()
      {
         sut.IsAppliedToAll().ShouldBeTrue();
      }

      [Observation]
      public void all_others_should_be_false()
      {
         sut.IsMerge().ShouldBeFalse();
         sut.IsClone().ShouldBeFalse();
         sut.IsReplace().ShouldBeFalse();
      }
   }

   public class when_option_is_replace : concern_for_MergeConflictOptions
   {
      protected override void Because()
      {
         sut = MergeConflictOptions.ReplaceOnce;
      }

      [Observation]
      public void is_replace_should_return_true()
      {
         sut.IsReplace().ShouldBeTrue();
      }

      [Observation]
      public void others_should_be_false()
      {
         sut.IsAppliedToAll().ShouldBeFalse();
         sut.IsMerge().ShouldBeFalse();
         sut.IsClone().ShouldBeFalse();
         sut.IsSkip().ShouldBeFalse();
      }
   }

   public class when_option_is_replace_all : concern_for_MergeConflictOptions
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
      public void is_applied_to_all_should_return_true()
      {
         sut.IsAppliedToAll().ShouldBeTrue();
      }

      [Observation]
      public void all_others_should_be_false()
      {
         sut.IsMerge().ShouldBeFalse();
         sut.IsClone().ShouldBeFalse();
         sut.IsSkip().ShouldBeFalse();
      }

      public class when_option_is_merge : concern_for_MergeConflictOptions
      {
         protected override void Because()
         {
            sut = MergeConflictOptions.MergeOnce;
         }

         [Observation]
         public void is_clone_should_return_true()
         {
            sut.IsMerge().ShouldBeTrue();
         }

         [Observation]
         public void others_should_be_false()
         {
            sut.IsAppliedToAll().ShouldBeFalse();
            sut.IsReplace().ShouldBeFalse();
            sut.IsSkip().ShouldBeFalse();
            sut.IsClone().ShouldBeFalse();
         }
      }

      public class when_option_is_merge_all : concern_for_MergeConflictOptions
      {
         protected override void Because()
         {
            sut = MergeConflictOptions.MergeAll;
         }

         [Observation]
         public void is_replace_should_return_true()
         {
            sut.IsMerge().ShouldBeTrue();
         }

         [Observation]
         public void is_applied_to_all_should_return_true()
         {
            sut.IsAppliedToAll().ShouldBeTrue();
         }

         [Observation]
         public void all_others_should_be_false()
         {
            sut.IsReplace().ShouldBeFalse();
            sut.IsSkip().ShouldBeFalse();
            sut.IsClone().ShouldBeFalse();
         }
      }





      public class when_option_is_clone : concern_for_MergeConflictOptions
      {
         protected override void Because()
         {
            sut = MergeConflictOptions.CloneOnce;
         }

         [Observation]
         public void is_clone_should_return_true()
         {
            sut.IsClone().ShouldBeTrue();
         }

         [Observation]
         public void others_should_be_false()
         {
            sut.IsAppliedToAll().ShouldBeFalse();
            sut.IsReplace().ShouldBeFalse();
            sut.IsSkip().ShouldBeFalse();
            sut.IsMerge().ShouldBeFalse();
         }
      }

      public class when_option_is_clone_all : concern_for_MergeConflictOptions
      {
         protected override void Because()
         {
            sut = MergeConflictOptions.CloneAll;
         }

         [Observation]
         public void is_replace_should_return_true()
         {
            sut.IsClone().ShouldBeTrue();
         }

         [Observation]
         public void is_applied_to_all_should_return_true()
         {
            sut.IsAppliedToAll().ShouldBeTrue();
         }

         [Observation]
         public void all_others_should_be_false()
         {
            sut.IsReplace().ShouldBeFalse();
            sut.IsSkip().ShouldBeFalse();
            sut.IsMerge().ShouldBeFalse();
         }
      }
   }
}