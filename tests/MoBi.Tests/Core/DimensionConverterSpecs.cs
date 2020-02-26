using FakeItEasy;
using MoBi.Core.Domain.UnitSystem;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core
{
   public abstract class concern_for_DimensionConverterFor<T> : ContextSpecification<DimensionConverter<T>> where T : IWithDimension

   {
      private IDimension _source;
      private IDimension _target;

      protected override void Context()
      {
         _source = A.Fake<IDimension>();
         _target = A.Fake<IDimension>();
         sut = new TestDimensionConverterFor(_source, _target);
      }

      public class TestDimensionConverterFor : DimensionConverter<T>
      {
         public TestDimensionConverterFor(IDimension sourceDimension, IDimension targetDimension) : base(sourceDimension, targetDimension)
         {
         }

         public override void SetRefObject(T refObject)
         {
         }

         public override string UnableToResolveParametersMessage
         {
            get { return "Totally messed up"; }
         }

         protected override double? GetFactor()
         {
            return 1;
         }
      }
   }


   public class When_a_parameter_converter_is_asked_to_convert_a_Parameter : concern_for_DimensionConverterFor<IParameter>
   {
      private bool _res;

      protected override void Because()
      {
         
         _res = sut.CanBeUsedFor(A.Fake<IParameter>());
      }

      [Test]
      public void should_return_true()
      {
         _res.ShouldBeTrue();
      }
   }

   public class When_a_parameter_converter_is_asked_to_convert_a_DataColumn : concern_for_DimensionConverterFor<IParameter>
   {
      private bool _res;

      protected override void Because()
      {
         
         _res = sut.CanBeUsedFor(A.Fake<DataColumn>());
      }

      [Observation]
      public void should_return_true()
      {
         _res.ShouldBeFalse();
      }
   }
}