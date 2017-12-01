using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ReportingHelper : ContextSpecification<ReportingHelper>
   {
      protected IDimension _dimension;
      protected Unit _unit;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      protected IWithDisplayUnit _withDisplayUnit;

      protected override void Context()
      {
         _dimension = A.Fake<IDimension>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _unit = A.Fake<Unit>();
         A.CallTo(_displayUnitRetriever).WithReturnType<Unit>().Returns(_unit);
         _withDisplayUnit = A.Fake<IWithDisplayUnit>();
         A.CallTo(() => _withDisplayUnit.DisplayUnit).Returns(_unit);
         A.CallTo(() => _withDisplayUnit.Dimension).Returns(_dimension);

         sut = new ReportingHelper(_displayUnitRetriever);
      }
   }

   public class When_using_reporting_helper_for_float : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(_dimension.DefaultUnit, _dimension, 0F);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_dimension.DefaultUnit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_for_double : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(_dimension.DefaultUnit, _dimension, 0D);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_dimension.DefaultUnit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_for_null : concern_for_ReportingHelper
   {
      [Observation]
      public void should_convert_as_expected()
      {
         var withDisplayUnit = A.Fake<IWithDisplayUnit>();
         sut.ConvertToDisplayUnit(withDisplayUnit, null).ShouldBeNull();
         sut.ConvertToDisplayUnit(null, _dimension, null).ShouldBeNull();
         sut.ConvertToDisplayUnit(_unit, _dimension, null).ShouldBeNull();
         sut.ConvertToDisplayUnit(_dimension.DefaultUnit, _dimension, null).ShouldBeNull();
      }
   }

   public class When_using_reporting_helper_without_unit_double : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(null, _dimension, 0D);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_without_unit_float : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(null, _dimension, 0F);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_without_unit_double_without_display_unit_setup : concern_for_ReportingHelper
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_dimension)).Returns(null);
      }

      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(null, _dimension, 0D);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_dimension.DefaultUnit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_without_unit_float_without_display_unit_setup : concern_for_ReportingHelper
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_dimension)).Returns(null);
      }

      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(null, _dimension, 0F);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_dimension.DefaultUnit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_with_unit_double : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(_unit, _dimension, 0D);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_with_object_double : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(_withDisplayUnit, 0D);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_with_unit_float : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(_unit, _dimension, 0F);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_with_object_float : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(_withDisplayUnit, 0F);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_using_reporting_helper_without_unit_float_with_display_unit_defined_for_dimension : concern_for_ReportingHelper
   {
      protected override void Because()
      {
         base.Because();
         sut.ConvertToDisplayUnit(null, _dimension, 0F);
      }

      [Observation]
      public void should_convert_as_expected()
      {
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_unit, 0D)).MustHaveHappened();
      }
   }

   public class When_getting_display_unit_for : concern_for_ReportingHelper
   {
      [Observation]
      public void should_get_unit_back()
      {
         sut.GetDisplayUnitFor(_unit, _dimension).ShouldBeEqualTo(_unit);
      }

      [Observation]
      public void should_get_unit_for_dimension_back()
      {
         sut.GetDisplayUnitFor(_dimension).ShouldBeEqualTo(_unit);
      }

      [Observation]
      public void should_get_display_unit_for_dimension_back()
      {
         sut.GetDisplayUnitFor(null, _dimension).ShouldBeEqualTo(_displayUnitRetriever.PreferredUnitFor(_dimension));
      }
   }

   public class When_no_display_unit_defined_and_getting_display_unit_for : concern_for_ReportingHelper
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_dimension)).Returns(null);
      }

      [Observation]
      public void should_get_unit_back()
      {
         sut.GetDisplayUnitFor(_unit, _dimension).ShouldBeEqualTo(_unit);
      }

      [Observation]
      public void should_get_unit_for_dimension_back()
      {
         sut.GetDisplayUnitFor(_dimension).ShouldBeEqualTo(_dimension.DefaultUnit);
      }

      [Observation]
      public void should_get_display_unit_for_dimension_back()
      {
         sut.GetDisplayUnitFor(null, _dimension).ShouldBeEqualTo(_dimension.DefaultUnit);
      }
   }
}