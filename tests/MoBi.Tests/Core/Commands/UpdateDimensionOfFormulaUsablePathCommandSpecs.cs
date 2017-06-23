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
   public abstract class concern_for_UpdateDimensionOfFormulaUsablePathCommand : ContextSpecification<UpdateDimensionOfFormulaUsablePathCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected string _alias;
      protected IFormula _formula;
      private IDimension _newDimension;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IBuildingBlock>();
         _alias = "alias";
         _formula = new ExplicitFormula("string");
         _formula.AddObjectPath(new FormulaUsablePath { Alias = _alias, Dimension = HelperForSpecs.AmountDimension });
         _newDimension = HelperForSpecs.ConcentrationDimension;
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IFormula>(_formula.Id)).Returns(_formula);
         A.CallTo(() => _context.DimensionFactory.Dimension(_newDimension.ToString())).Returns(_newDimension);
         A.CallTo(() => _context.DimensionFactory.Dimension(HelperForSpecs.AmountDimension.ToString())).Returns(HelperForSpecs.AmountDimension);

         sut = new UpdateDimensionOfFormulaUsablePathCommand(_newDimension, _formula, _alias, _buildingBlock);
      }
   }

   public class When_updating_the_dimension_of_a_usable_path : concern_for_UpdateDimensionOfFormulaUsablePathCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_usable_path_dimension()
      {
         _formula.FormulaUsablePathBy(_alias).Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }
   }

   public class When_reverting_the_dimension_of_a_usable_path : concern_for_UpdateDimensionOfFormulaUsablePathCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_update_the_usable_path_dimension()
      {
         _formula.FormulaUsablePathBy(_alias).Dimension.ShouldBeEqualTo(HelperForSpecs.AmountDimension);
      }
   }
}
