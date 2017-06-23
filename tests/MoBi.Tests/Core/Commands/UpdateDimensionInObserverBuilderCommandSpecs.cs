using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateDimensionInObserverBuilderCommand : ContextSpecification<UpdateDimensionInObserverBuilderCommand>
   {
      protected IDimension _newDimension;
      protected IDimension _oldDimension;
      protected IObserverBuilder _observedBuilder;
      protected IObserverBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = new ObserverBuildingBlock();
         _context = A.Fake<IMoBiContext>();
         _newDimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);
         _oldDimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Concentration);

         _observedBuilder = new ObserverBuilder
         {
            Dimension = _oldDimension,
            Formula = new ConstantFormula(5).WithDimension(_oldDimension)
         }.WithId("5");

         sut = new UpdateDimensionInObserverBuilderCommand(_observedBuilder, _newDimension, _buildingBlock);
      }
   }

   public class when_converting_dimensions_on_observer_builder : concern_for_UpdateDimensionInObserverBuilderCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void dimensions_associated_with_observer_have_changed()
      {
         _observedBuilder.Dimension.ShouldBeEqualTo(_newDimension);
         _observedBuilder.Formula.Dimension.ShouldBeEqualTo(_newDimension);
      }
   }

   public class when_reversing_dimension_change_on_observer_builder : concern_for_UpdateDimensionInObserverBuilderCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<IObserverBuilder>(_observedBuilder.Id)).Returns(_observedBuilder);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void original_dimension_restored()
      {
         _observedBuilder.Dimension.ShouldBeEqualTo(_oldDimension);
         _observedBuilder.Formula.Dimension.ShouldBeEqualTo(_oldDimension);
      }
   }
}