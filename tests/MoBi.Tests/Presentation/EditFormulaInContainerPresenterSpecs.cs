using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Helpers;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFormulaInContainerPresenter : ContextSpecification<EditFormulaInContainerPresenter>
   {
      protected IEditFormulaInContainerView _editFormulaView;
      private IFormulaPresenterCache _formulaPresenterCache;
      protected IMoBiContext _context;
      private IFormulaToFormulaInfoDTOMapper _formulaToDTOInfoMapper;
      protected ICommandCollector _commandCollector;
      protected IMoBiFormulaTask _formulaTask;
      private ICircularReferenceChecker _circularReferenceChecker;

      protected override void Context()
      {
         _editFormulaView = A.Fake<IEditFormulaInContainerView>();
         _context = A.Fake<IMoBiContext>();
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _formulaToDTOInfoMapper = new FormulaToFormulaInfoDTOMapper();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _circularReferenceChecker = A.Fake<ICircularReferenceChecker>();
         sut = new EditFormulaInContainerPresenter(_editFormulaView, _formulaPresenterCache, _context, _formulaToDTOInfoMapper, new FormulaTypeCaptionRepository(), _formulaTask, _circularReferenceChecker);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }
   }

   internal class When_selecting_named_formula_type : concern_for_EditFormulaInContainerPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buildingBlockWithFormulaCache;
      private IFormulaCache _formulaCache;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _buildingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = A.Fake<IFormulaCache>();
         A.CallTo(() => _buildingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);

         IFormula formula = new ExplicitFormula();
         _parameter.Formula = formula;
         A.CallTo(() => _context.Create<ExplicitFormula>()).Returns(A.Fake<ExplicitFormula>());


         sut.Init(_parameter, _buildingBlockWithFormulaCache, new UsingFormulaDecoder());
      }

      protected override void Because()
      {
         sut.AddNewFormula(string.Empty);
      }

      [Observation]
      public void should_Not_Add_a_formula_to_formula_cache()
      {
         A.CallTo(() => _formulaCache.Add(A<Formula>._)).MustNotHaveHappened();
         A.CallTo(() => _buildingBlockWithFormulaCache.AddFormula(A<Formula>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_add_any_command()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<IMoBiCommand>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_clear_the_formula_view()
      {
         A.CallTo(() => _editFormulaView.ClearFormulaView()).MustHaveHappened();
      }
   }

   public class When_selecting_an_constant_formula : concern_for_EditFormulaInContainerPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buildingBlockWithFormulaCache;

      protected override void Context()
      {
         base.Context();
         _buildingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _parameter = new Parameter().WithDimension(A.Fake<IDimension>());
         var constantFormula = new ConstantFormula().WithDimension(_parameter.Dimension);
         A.CallTo(() => _formulaTask.CreateNewFormula<ConstantFormula>(_parameter.Dimension)).Returns(constantFormula);
      }

      protected override void Because()
      {
         sut.Init(_parameter, _buildingBlockWithFormulaCache);
      }

      [Observation]
      public void should_create_a_constant_formula_using_the_dimension_of_the_parameter()
      {
         var formula = sut.Subject as ConstantFormula;
         formula.ShouldNotBeNull();
         formula.Dimension.ShouldBeEqualTo(_parameter.Dimension);
      }

      [Observation]
      public void display_name_for_not_supported_formula_type()
      {
         sut.DisplayFor(typeof(DistributedTableFormula)).ShouldBeEqualTo(nameof(DistributedTableFormula).SplitToUpperCase());
      }

      [Observation]
      public void presenter_cannot_create_new_distributed_formula()
      {
         sut.CanCreateFormulaType(typeof(DistributedTableFormula)).ShouldBeFalse();
      }
   }

   internal class When_adding_a_named_Formula_after_selecting_named_formula_type : concern_for_EditFormulaInContainerPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buildingBlockWithFormulaCache;
      private ExplicitFormula _explicitFormula;
      private IFormula _oldFormula;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _buildingBlockWithFormulaCache = new ParameterValuesBuildingBlock();


         _oldFormula = new ExplicitFormula("1+2").WithId("OLD_FORMULA");
         _parameter.Formula = _oldFormula;
         _explicitFormula = new ExplicitFormula {Id = "Formula", Name = "toto"};
         A.CallTo(() => _context.ObjectRepository.ContainsObjectWithId(_oldFormula.Id)).Returns(true);
         A.CallTo(() => _formulaTask.CreateNewFormula(typeof(ExplicitFormula), _parameter.Dimension)).Returns(_explicitFormula);

         sut.Init(_parameter, _buildingBlockWithFormulaCache);

         //add so that it will be found when setting the value in the parameter
         _buildingBlockWithFormulaCache.AddFormula(_explicitFormula);

         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(A<Type>._, A<IDimension>._, A<IEnumerable<string>>._, _buildingBlockWithFormulaCache, null))
            .Returns((A.Fake<IMoBiCommand>(), _explicitFormula));
      }

      protected override void Because()
      {
         sut.AddNewFormula();
      }

      [Observation]
      public void should_Add_a_formula_to_formula_cache()
      {
         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(typeof (ExplicitFormula), A<IDimension>._, A<IEnumerable<string>>._, _buildingBlockWithFormulaCache, null))
            .MustHaveHappened();
      }

      [Observation]
      public void should_change_parents_formula_to_new_formula()
      {
         A.CallTo(() => _formulaTask.UpdateFormula(_parameter, _oldFormula, _explicitFormula, A<FormulaDecoder>._, _buildingBlockWithFormulaCache)).MustHaveHappened();
      }
   }

   public class When_adding_a_new_formula_to_the_edited_building_block : concern_for_EditFormulaInContainerPresenter
   {
      private IBuildingBlock _buildingBlockWithFormulaCache;
      private IFormulaCache _formulaCache;
      private IParameter _parameter;
      private IEnumerable<string> _availableFormulaNames;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _buildingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);

         _formulaCache.Add(new ExplicitFormula().WithId("A").WithName("A"));
         _formulaCache.Add(new TableFormulaWithOffset().WithId("B").WithName("B"));
         _formulaCache.Add(new SumFormula().WithId("C").WithName("C"));

         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(A<Type>._, A<IDimension>._, A<IEnumerable<string>>._, _buildingBlockWithFormulaCache, null))
            .Invokes(x => _availableFormulaNames = x.GetArgument<IEnumerable<string>>(2))
            .Returns((A.Fake<IMoBiCommand>(), null));


         sut.Init(_parameter, _buildingBlockWithFormulaCache, new UsingFormulaDecoder());
      }

      protected override void Because()
      {
         sut.AddNewFormula();
      }

      [Observation]
      public void should_ensure_that_the_formula_created_as_a_unique_name()
      {
         _availableFormulaNames.ShouldOnlyContain("A", "B", "C");
      }
   }

   public class When_retrieving_the_list_of_all_available_formula_for_the_edited_parameter : concern_for_EditFormulaInContainerPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buildingBlockWithFormulaCache;
      private FormulaCache _formulaCache;
      private IDimension _dimEquivalent;
      private IDimension _dimParameter;
      private IDimension _anotherDimension;

      protected override void Context()
      {
         base.Context();
         _dimParameter = DomainHelperForSpecs.AmountDimension;
         _dimEquivalent = new Dimension(_dimParameter.BaseRepresentation, "Equivalent", _dimParameter.BaseUnit.Name);
         _anotherDimension = DomainHelperForSpecs.TimeDimension;
         _parameter = A.Fake<IParameter>().WithDimension(_dimParameter);
         _parameter.Formula = new ExplicitFormula("1+2").WithDimension(_dimParameter);
         _buildingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);
         _formulaCache.Add(new ExplicitFormula().WithId("B").WithName("B").WithDimension(_dimEquivalent));
         _formulaCache.Add(new ExplicitFormula().WithId("C").WithName("C").WithDimension(_anotherDimension));
         _formulaCache.Add(new ExplicitFormula().WithId("A").WithName("A").WithDimension(_dimParameter));

         sut.Init(_parameter, _buildingBlockWithFormulaCache, new UsingFormulaDecoder());
      }

      [Observation]
      public void should_return_all_available_formula_with_a_dimension_equivalent_to_the_one_of_the_parameter()
      {
         sut.DisplayFormulaNames().ShouldOnlyContainInOrder("A", "B");
      }
   }

   public class When_retrieving_the_list_of_all_available_formula_for_the_edited_rhs_parameter_formula : concern_for_EditFormulaInContainerPresenter
   {
      private IParameter _parameter;
      private IBuildingBlock _buildingBlockWithFormulaCache;
      private FormulaCache _formulaCache;
      private IDimension _dimParameter;
      private IDimension _rhsDim;

      protected override void Context()
      {
         base.Context();
         _dimParameter = DomainHelperForSpecs.AmountDimension;
         _rhsDim = DomainHelperForSpecs.AmountPerTimeDimension;
         _parameter = A.Fake<IParameter>().WithDimension(_dimParameter);
         _parameter.RHSFormula = new ExplicitFormula("1+2").WithDimension(_rhsDim);

         _buildingBlockWithFormulaCache = A.Fake<IBuildingBlock>();
         _formulaCache = new FormulaCache();
         A.CallTo(() => _buildingBlockWithFormulaCache.FormulaCache).Returns(_formulaCache);
         _formulaCache.Add(new ExplicitFormula().WithId("C").WithName("C").WithDimension(_rhsDim));
         _formulaCache.Add(new ExplicitFormula().WithId("A").WithName("A").WithDimension(_dimParameter));

         sut.IsRHS = true;
         sut.Init(_parameter, _buildingBlockWithFormulaCache);
      }

      [Observation]
      public void should_return_all_available_formula_with_a_dimension_equivalent_to_the_one_of_the_parameter()
      {
         sut.DisplayFormulaNames().ShouldOnlyContainInOrder("C");
      }
   }
}