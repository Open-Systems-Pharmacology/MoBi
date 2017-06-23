using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Exceptions;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Core.Service
{
   public abstract class concern_for_AmountToConcentrationConverter : ContextSpecification<IAmountToConcentrationConverter>
   {
      protected IDimensionFactory _dimensionFactory;
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IAmoutToConcentrationFormulaMapper _amoutToConcentrationFormulaMapper;
      protected IMoleculeBuilder _moleculeBuilder;
      private IObjectBaseFactory _objectBaseFactory;
      private IFormulaTask _formulaTask;
      protected MoleculeStartValue _moleculeStartValue;
      protected FormulaCache _formulaCache;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      private IObjectTypeResolver _objectTypeResolver;
      private IFormulaFactory _formulaFactory;
      protected IFormula _constantZeroFormula;

      protected override void Context()
      {
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _amoutToConcentrationFormulaMapper = A.Fake<IAmoutToConcentrationFormulaMapper>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaTask = A.Fake<IFormulaTask>();
         _displayUnitRetriever= A.Fake<IDisplayUnitRetriever>();
         _formulaFactory= A.Fake<IFormulaFactory>();
         _constantZeroFormula= new ConstantFormula(0);
         _formulaCache = new FormulaCache();
         _objectTypeResolver=new ObjectTypeResolver();
         A.CallTo(() => _formulaTask.AddParentVolumeReferenceToFormula(A<IFormula>._)).Returns(Constants.VOLUME_ALIAS);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(HelperForSpecs.AmountDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME)).Returns(HelperForSpecs.AmountPerTimeDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION)).Returns(HelperForSpecs.ConcentrationDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME)).Returns(HelperForSpecs.ConcentrationPerTimeDimension);
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         A.CallTo(() => _formulaFactory.ConstantFormula(0, HelperForSpecs.ConcentrationDimension)).Returns(_constantZeroFormula);
         sut = new AmountToConcentrationConverter(_reactionDimensionRetriever, _dimensionFactory, _amoutToConcentrationFormulaMapper,
            _objectBaseFactory, _formulaTask,_displayUnitRetriever,_objectTypeResolver,_formulaFactory);
      }
   }

   public class When_converting_a_molecule_builder_that_is_already_in_concentration_in_concentration : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(5), Dimension = HelperForSpecs.ConcentrationDimension };
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_do_nothing()
      {
         _moleculeBuilder.GetDefaultMoleculeStartValue().ShouldBeEqualTo(5);
      }
   }

   public class When_converting_a_molecule_builder_with_a_constant_zero_formula_from_amount_to_concentration : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(0), Dimension = HelperForSpecs.AmountDimension };
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_keep_the_formula_to_zero()
      {
         _moleculeBuilder.GetDefaultMoleculeStartValue().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }


      [Observation]
      public void should_have_used_th_formula_factory_to_create_a_new_constnat_formula_with_the_expected_dimension()
      {
         _moleculeBuilder.DefaultStartFormula.ShouldBeEqualTo(_constantZeroFormula);
      }
   }

   public class When_converting_a_molecule_builder_with_a_constant_non_zero_formula_from_amount_to_concentration : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(5), Dimension = HelperForSpecs.AmountDimension };
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_have_converted_the_formula_to_an_explicit_formula()
      {
         _moleculeBuilder.DefaultStartFormula.IsExplicit().ShouldBeTrue();
         _moleculeBuilder.DefaultStartFormula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("5/V");
      }

      [Observation]
      public void should_have_added_the_converted_formula_to_the_formula_cache()
      {
        _formulaCache.Contains(_moleculeBuilder.DefaultStartFormula).ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }
   }

   public class When_converting_a_molecule_builder_with_an_explicit_formula_for_which_an_explicit_formula_mapping_exists : concern_for_AmountToConcentrationConverter
   {
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("A+B");
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = _explicitFormula, Dimension = HelperForSpecs.AmountDimension };
         A.CallTo(() => _amoutToConcentrationFormulaMapper.HasMappingFor(_explicitFormula)).Returns(true);
         A.CallTo(() => _amoutToConcentrationFormulaMapper.MappedFormulaFor(_explicitFormula)).Returns("C+D");
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_have_replaced_the_formula_instead_of_dividing_by_the_volume()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("C+D");
         _explicitFormula.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }


      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }
   }

   public class When_converting_a_molecule_builder_with_an_explicit_formula_for_which_an_explicit_formula_mapping_does_not_exist : concern_for_AmountToConcentrationConverter
   {
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("A+B");
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = _explicitFormula, Dimension = HelperForSpecs.AmountDimension };
         A.CallTo(() => _amoutToConcentrationFormulaMapper.HasMappingFor(_explicitFormula)).Returns(false);
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_divide_the_existing_formula_by_the_volume_of_the_parent_container()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("(A+B)/V");
         _explicitFormula.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }
   }

   public class When_converting_a_reaction_builder_from_amount_to_concentration: concern_for_AmountToConcentrationConverter
   {
      private IReactionBuilder _reactionBuilder;
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("A+B");
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _reactionBuilder = new ReactionBuilder{ Formula = _explicitFormula, Dimension = HelperForSpecs.AmountPerTimeDimension };
      }
      protected override void Because()
      {
         sut.Convert(_reactionBuilder,_formulaCache);
      }

      [Observation]
      public void should_divide_the_existing_formula_by_the_volume_of_the_parent_container()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("(A+B)/V");
         _explicitFormula.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationPerTimeDimension);
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_per_time_dimension()
      {
         _reactionBuilder.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationPerTimeDimension);
      }
   }

   public class When_converting_a_molecule_start_value_that_is_already_in_concentration_in_concentration : concern_for_AmountToConcentrationConverter
   {

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeStartValue = new MoleculeStartValue{ Formula = new ExplicitFormula("10"), Dimension = HelperForSpecs.ConcentrationDimension };
      }

      protected override void Because()
      {
         sut.Convert(_moleculeStartValue);
      }

      [Observation]
      public void should_do_nothing()
      {
         _moleculeStartValue.Formula.Calculate(_moleculeStartValue).ShouldBeEqualTo(10);
      }
   }

   public class When_converting_a_molecule_start_value__with_a_constant_non_zero_formula_from_amount_to_concentration : concern_for_AmountToConcentrationConverter
   {
      private Unit _unit;

      protected override void Context()
      {
         base.Context();
         _unit= A.Fake<Unit>();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeStartValue = new MoleculeStartValue { StartValue = 5, Dimension = HelperForSpecs.AmountDimension };
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_moleculeStartValue)).Returns(_unit);
      }

      protected override void Because()
      {
         sut.Convert(_moleculeStartValue, _formulaCache);
      }

      [Observation]
      public void should_have_converted_the_formula_to_an_explicit_formula()
      {
         _moleculeStartValue.Formula.IsExplicit().ShouldBeTrue();
         _moleculeStartValue.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("5/V");
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeStartValue.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
         _moleculeStartValue.Formula.Dimension.ShouldBeEqualTo(HelperForSpecs.ConcentrationDimension);
      }

      [Observation]
      public void should_have_updated_the_display_unit_to_use_the_preferred_display()
      {
         _moleculeStartValue.DisplayUnit.ShouldBeEqualTo(_unit);
      }
   }

   public class When_converting_a_molecule_from_concentration_to_amount : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(5), Dimension = HelperForSpecs.ConcentrationDimension };
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Convert(_moleculeBuilder)).ShouldThrowAn<CannotConvertConcentrationToAmountException>();
      }
   }

   public class When_converting_a_reaction_from_concentration_to_amount : concern_for_AmountToConcentrationConverter
   {
      private ReactionBuilder _reactionBuilder;

      protected override void Context()
      {
         base.Context();
         _reactionBuilder= new ReactionBuilder { Dimension = HelperForSpecs.ConcentrationPerTimeDimension };
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Convert(_reactionBuilder)).ShouldThrowAn<CannotConvertConcentrationToAmountException>();
      }
   }
}