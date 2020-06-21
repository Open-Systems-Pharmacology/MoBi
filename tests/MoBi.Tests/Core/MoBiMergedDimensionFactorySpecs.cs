using FakeItEasy;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiMergedDimensionFactory : ContextSpecification<MoBiDimensionFactory>
   {
      protected IMoBiDimensionConverter<IParameter> _converter;
      protected IDimension _source;
      protected IDimension _target;

      protected override void Context()
      {
         _source = new Dimension(new BaseDimensionRepresentation(), "DrugMass", "g");
         _source.AddUnit("mg", 1000, 0);
         _source.DefaultUnit = _source.Unit("mg");
         _target = new Dimension(new BaseDimensionRepresentation(), "Target", "mol");
         _target.AddUnit("mmol", 300, 0);
         _target.DefaultUnit = _target.Unit("mmol");
         _converter = new TestDimensionConverterFor<IParameter>(_source, _target);

         sut = new MoBiDimensionFactory();
      }

    
   }

   public class When_told_to_get_merged_dimension_for_Parameter : concern_for_MoBiMergedDimensionFactory
   {
      private IParameter _parameter;
      private MergedDimensionFor<IParameter> _res;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _parameter.Dimension = _source;
          var info = A.Fake<IMoBiDimensionMergingInformation>();
         A.CallTo(() => info.SourceDimension).Returns(_source);
         A.CallTo(() => info.TargetDimension).Returns(_target);
         A.CallTo(() => info.Matches(_source, _target,_parameter )).Returns(true);
         A.CallTo(() => info.Converter).Returns(_converter);
         sut.AddMergingInformation(info);
      }

      protected override void Because()
      {
         _res = sut.MergedDimensionFor(_parameter) as MergedDimensionFor<IParameter>;
      }

      [Observation]
      public void Shoul_return_a_new_merged_dimension()
      {
         _res.ShouldNotBeNull();
         _res.DefaultUnit.ShouldBeEqualTo(_source.DefaultUnit);
         _res.GetUnitNames().ShouldOnlyContainInOrder("g", "mg", "mol", "mmol");
         _res.BaseUnit.ShouldBeEqualTo(_source.BaseUnit);
      }
   }

   public class When_told_to_get_merged_dimension_for_McDataColum_with_a_Dimension_without_merging : concern_for_MoBiMergedDimensionFactory
   {
      private IParameter _parameter;
      private IDimension _res;
      private IDimension _notMerged;


      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _notMerged = A.Fake<IDimension>();
         _parameter.Dimension = _notMerged;
      }

      protected override void Because()
      {
         _res = sut.MergedDimensionFor(_parameter);
      }

      [Observation]
      public void should_return_the_dimension_of_the_column()
      {
         _res.ShouldBeEqualTo(_notMerged);
      }
   }

   public class When_two_posssible_converter_for_One_conversion_are_present : concern_for_MoBiMergedDimensionFactory
   {
      private IMoBiDimensionConverter<DataColumn> _otherConverter;
      private IParameter _parameter;
      private MergedDimensionFor<IParameter> _res;

      protected override void Context()
      {
         base.Context();
         _otherConverter = new TestDimensionConverterFor<DataColumn>(_source, _target);
         _parameter = A.Fake<IParameter>();
         _parameter.Dimension = _source;
         var info = A.Fake<IMoBiDimensionMergingInformation>();
         A.CallTo(() => info.SourceDimension).Returns(_source);
         A.CallTo(() => info.TargetDimension).Returns(_target);
         A.CallTo(() => info.Matches(_source, _target, _parameter)).Returns(true);
         A.CallTo(() => info.Converter).Returns(_converter);
         sut.AddMergingInformation(info);

         var info2 = A.Fake<IMoBiDimensionMergingInformation>();
         A.CallTo(() => info2.SourceDimension).Returns(_source);
         A.CallTo(() => info2.TargetDimension).Returns(_target);
         A.CallTo(() => info2.Matches(_source, _target, _parameter)).Returns(false);
         A.CallTo(() => info2.Converter).Returns(_otherConverter);
         sut.AddMergingInformation(info2);
      }

      protected override void Because()
      {
         _res = sut.MergedDimensionFor(_parameter) as MergedDimensionFor<IParameter>;
      }

      [Observation]
      public void Shoul_return_a_new_merged_dimnsion()
      {
         _res.ShouldNotBeNull();
         _res.DefaultUnit.ShouldBeEqualTo(_source.DefaultUnit);
         _res.GetUnitNames().ShouldOnlyContainInOrder("g", "mg", "mol", "mmol");
         _res.BaseUnit.ShouldBeEqualTo(_source.BaseUnit);
      }
   }

   internal class TestDimensionConverterFor<T> : DimensionConverter<T> where T : IWithDimension
   {
      public TestDimensionConverterFor(IDimension sourceDimension, IDimension targetDimension)
         : base(sourceDimension, targetDimension)
      {
      }

      public override void SetRefObject(T refObject)
      {

      }

      public override string UnableToResolveParametersMessage
      {
         get { return ""; }
      }

      protected override double? GetFactor()
      {
         return 1;
      }
   }
}