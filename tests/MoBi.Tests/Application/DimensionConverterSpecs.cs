using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.UnitSystem;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Application
{
   public abstract class concern_for_DimensionCoverterFor<T> : ContextSpecification<DimensionConverterFor<T>> where T : IWithDimension

   {
      private IDimension _source;
      private IDimension _target;

      protected override void Context()
      {
         _source = A.Fake<IDimension>();
         _target = A.Fake<IDimension>();
         sut = new TestDimensionConverterFor(_source, _target);
      }

      public class TestDimensionConverterFor : DimensionConverterFor<T>
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


   public class When_a_parameter_converter_is_asked_to_convert_a_Parameter : concern_for_DimensionCoverterFor<IParameter>
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

   public class When_a_parameter_converter_is_asked_to_convert_a_DataColumn : concern_for_DimensionCoverterFor<IParameter>
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