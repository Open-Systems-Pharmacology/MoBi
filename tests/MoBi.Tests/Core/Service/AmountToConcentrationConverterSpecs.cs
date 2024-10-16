﻿using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Exceptions;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Helpers;
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
      protected IAmoutToConcentrationFormulaMapper _amountToConcentrationFormulaMapper;
      protected MoleculeBuilder _moleculeBuilder;
      private IObjectBaseFactory _objectBaseFactory;
      private IFormulaTask _formulaTask;
      protected InitialCondition _initialCondition;
      protected FormulaCache _formulaCache;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      private IObjectTypeResolver _objectTypeResolver;
      private IFormulaFactory _formulaFactory;
      protected IFormula _constantZeroFormula;

      protected override void Context()
      {
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _amountToConcentrationFormulaMapper = A.Fake<IAmoutToConcentrationFormulaMapper>();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _formulaTask = A.Fake<IFormulaTask>();
         _displayUnitRetriever= A.Fake<IDisplayUnitRetriever>();
         _formulaFactory= A.Fake<IFormulaFactory>();
         _constantZeroFormula= new ConstantFormula(0);
         _formulaCache = new FormulaCache();
         _objectTypeResolver=new ObjectTypeResolver();
         A.CallTo(() => _formulaTask.AddParentVolumeReferenceToFormula(A<IFormula>._)).Returns(Constants.VOLUME_ALIAS);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT)).Returns(DomainHelperForSpecs.AmountDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT_PER_TIME)).Returns(DomainHelperForSpecs.AmountPerTimeDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION)).Returns(DomainHelperForSpecs.ConcentrationDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME)).Returns(DomainHelperForSpecs.ConcentrationPerTimeDimension);
         A.CallTo(() => _objectBaseFactory.Create<ExplicitFormula>()).Returns(new ExplicitFormula());
         A.CallTo(() => _formulaFactory.ConstantFormula(0, DomainHelperForSpecs.ConcentrationDimension)).Returns(_constantZeroFormula);
         sut = new AmountToConcentrationConverter(_reactionDimensionRetriever, _dimensionFactory, _amountToConcentrationFormulaMapper,
            _objectBaseFactory, _formulaTask,_displayUnitRetriever,_objectTypeResolver,_formulaFactory);
      }
   }

   public class When_converting_a_molecule_builder_that_is_already_in_concentration_in_concentration : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(5), Dimension = DomainHelperForSpecs.ConcentrationDimension };
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_do_nothing()
      {
         _moleculeBuilder.GetDefaultInitialCondition().ShouldBeEqualTo(5);
      }
   }

   public class When_converting_a_molecule_builder_with_a_constant_zero_formula_from_amount_to_concentration : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(0), Dimension = DomainHelperForSpecs.AmountDimension };
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_keep_the_formula_to_zero()
      {
         _moleculeBuilder.GetDefaultInitialCondition().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
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
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(5), Dimension = DomainHelperForSpecs.AmountDimension };
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
         _moleculeBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
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
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = _explicitFormula, Dimension = DomainHelperForSpecs.AmountDimension };
         A.CallTo(() => _amountToConcentrationFormulaMapper.HasMappingFor(_explicitFormula)).Returns(true);
         A.CallTo(() => _amountToConcentrationFormulaMapper.MappedFormulaFor(_explicitFormula)).Returns("C+D");
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_have_replaced_the_formula_instead_of_dividing_by_the_volume()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("C+D");
         _explicitFormula.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
      }


      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
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
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = _explicitFormula, Dimension = DomainHelperForSpecs.AmountDimension };
         A.CallTo(() => _amountToConcentrationFormulaMapper.HasMappingFor(_explicitFormula)).Returns(false);
      }

      protected override void Because()
      {
         sut.Convert(_moleculeBuilder, _formulaCache);
      }

      [Observation]
      public void should_divide_the_existing_formula_by_the_volume_of_the_parent_container()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("(A+B)/V");
         _explicitFormula.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _moleculeBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
      }
   }

   public class When_converting_a_reaction_builder_from_amount_to_concentration: concern_for_AmountToConcentrationConverter
   {
      private ReactionBuilder _reactionBuilder;
      private ExplicitFormula _explicitFormula;

      protected override void Context()
      {
         base.Context();
         _explicitFormula = new ExplicitFormula("A+B");
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _reactionBuilder = new ReactionBuilder{ Formula = _explicitFormula, Dimension = DomainHelperForSpecs.AmountPerTimeDimension };
      }
      protected override void Because()
      {
         sut.Convert(_reactionBuilder,_formulaCache);
      }

      [Observation]
      public void should_divide_the_existing_formula_by_the_volume_of_the_parent_container()
      {
         _explicitFormula.FormulaString.ShouldBeEqualTo("(A+B)/V");
         _explicitFormula.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationPerTimeDimension);
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_per_time_dimension()
      {
         _reactionBuilder.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationPerTimeDimension);
      }
   }

   public class When_converting_a_molecule_start_value_that_is_already_in_concentration_in_concentration : concern_for_AmountToConcentrationConverter
   {

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         _initialCondition = new InitialCondition { Formula = new ExplicitFormula("10"), Dimension = DomainHelperForSpecs.ConcentrationDimension };
      }

      protected override void Because()
      {
         sut.Convert(_initialCondition);
      }

      [Observation]
      public void should_do_nothing()
      {
         _initialCondition.Formula.Calculate(_initialCondition).ShouldBeEqualTo(10);
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
         _initialCondition = new InitialCondition { Value = 5, Dimension = DomainHelperForSpecs.AmountDimension };
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(_initialCondition)).Returns(_unit);
      }

      protected override void Because()
      {
         sut.Convert(_initialCondition, _formulaCache);
      }

      [Observation]
      public void should_have_converted_the_formula_to_an_explicit_formula()
      {
         _initialCondition.Formula.IsExplicit().ShouldBeTrue();
         _initialCondition.Formula.DowncastTo<ExplicitFormula>().FormulaString.ShouldBeEqualTo("5/V");
      }

      [Observation]
      public void should_have_updated_the_dimension_to_concentration_dimension()
      {
         _initialCondition.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
         _initialCondition.Formula.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimension);
      }

      [Observation]
      public void should_have_updated_the_display_unit_to_use_the_preferred_display()
      {
         _initialCondition.DisplayUnit.ShouldBeEqualTo(_unit);
      }
   }

   public class When_converting_a_molecule_from_concentration_to_amount : concern_for_AmountToConcentrationConverter
   {
      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = new MoleculeBuilder { DefaultStartFormula = new ConstantFormula(5), Dimension = DomainHelperForSpecs.ConcentrationDimension };
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
         _reactionBuilder= new ReactionBuilder { Dimension = DomainHelperForSpecs.ConcentrationPerTimeDimension };
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.Convert(_reactionBuilder)).ShouldThrowAn<CannotConvertConcentrationToAmountException>();
      }
   }
}