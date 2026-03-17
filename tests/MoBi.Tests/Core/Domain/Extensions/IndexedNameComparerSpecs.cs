using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core.Domain.Extensions
{
   public abstract class concern_for_IndexedNameComparer : ContextSpecification<IndexedNameComparer>
   {
      protected override void Context()
      {
         sut = new IndexedNameComparer();
      }
   }

   public class When_comparing_names_with_same_prefix_and_numeric_indexes : concern_for_IndexedNameComparer
   {
      [Observation]
      public void should_sort_by_numeric_value_of_the_index()
      {
         (sut.Compare("application_2", "application_10") < 0).ShouldBeTrue();
         (sut.Compare("application_10", "application_2") > 0).ShouldBeTrue();
      }

      [Observation]
      public void should_return_zero_for_equal_names()
      {
         sut.Compare("application_1", "application_1").ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_order_lower_index_before_higher_index()
      {
         (sut.Compare("application_1", "application_2") < 0).ShouldBeTrue();
         (sut.Compare("application_5", "application_3") > 0).ShouldBeTrue();
      }
   }

   public class When_comparing_names_with_different_prefixes : concern_for_IndexedNameComparer
   {
      [Observation]
      public void should_sort_by_prefix_first()
      {
         (sut.Compare("alpha_1", "beta_1") < 0).ShouldBeTrue();
         (sut.Compare("beta_1", "alpha_1") > 0).ShouldBeTrue();
      }
   }

   public class When_comparing_names_with_multiple_underscores : concern_for_IndexedNameComparer
   {
      [Observation]
      public void should_use_only_the_last_segment_as_index()
      {
         (sut.Compare("some_long_prefix_2", "some_long_prefix_10") < 0).ShouldBeTrue();
         (sut.Compare("some_long_prefix_10", "some_long_prefix_2") > 0).ShouldBeTrue();
      }
   }

   public class When_comparing_names_with_non_numeric_suffixes : concern_for_IndexedNameComparer
   {
      [Observation]
      public void should_fall_back_to_string_comparison()
      {
         (sut.Compare("application_abc", "application_def") < 0).ShouldBeTrue();
         (sut.Compare("application_def", "application_abc") > 0).ShouldBeTrue();
      }

      [Observation]
      public void should_fall_back_to_string_comparison_for_mismatches()
      {
         (sut.Compare("application_11", "application_def") < 0).ShouldBeTrue();
         (sut.Compare("application_def", "application_11") > 0).ShouldBeTrue();
      }
   }

   public class When_comparing_null_names : concern_for_IndexedNameComparer
   {
      [Observation]
      public void should_return_zero_for_two_nulls()
      {
         sut.Compare(null, null).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_order_null_before_non_null()
      {
         (sut.Compare(null, "application_1") < 0).ShouldBeTrue();
         (sut.Compare("application_1", null) > 0).ShouldBeTrue();
      }
   }

   public class When_comparing_names_with_zero_padded_indexes : concern_for_IndexedNameComparer
   {
      [Observation]
      public void should_use_alphabetic_comparison_for_zero_padded_indexes()
      {
         (sut.Compare("application_01", "application_02") < 0).ShouldBeTrue();
         (sut.Compare("application_02", "application_01") > 0).ShouldBeTrue();
      }

      [Observation]
      public void should_use_alphabetic_comparison_when_either_suffix_is_zero_padded()
      {
         // "09" (zero-padded) vs "10" (not zero-padded): alphabetic "09" < "10"
         (sut.Compare("application_09", "application_10") < 0).ShouldBeTrue();
      }

      [Observation]
      public void should_not_treat_bare_zero_as_zero_padded()
      {
         // "0" is not zero-padded, so numeric comparison applies: 0 < 1
         (sut.Compare("application_0", "application_1") < 0).ShouldBeTrue();
      }
   }
}