using MoBi.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_ExplicitFormulaToExplicitFormulaDTOMapper : ContextSpecification<IExplicitFormulaToExplicitFormulaDTOMapper>
   {
      private IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathMapper;
      private IObjectPathFactory _objectPathFactory;
      protected IEntityPathResolver _entityPathResolver;
      private IContainer _topContainer;
      private IContainer _parentContainer;
      protected IParameter _parameter;
      protected ExplicitFormula _explicitFormula;
      protected IParameter _parameter2;

      protected override void Context()
      {
         _formulaUsablePathMapper = new FormulaUsablePathToFormulaUsablePathDTOMapper();
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _entityPathResolver = new EntityPathResolver(_objectPathFactory);
         sut = new ExplicitFormulaToExplicitFormulaDTOMapper(_formulaUsablePathMapper, _objectPathFactory, _entityPathResolver);

         _explicitFormula = new ExplicitFormula();
         _explicitFormula.AddObjectPath(
            new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "P2")
               .WithDimension(DimensionFactoryForSpecs.MassDimension)
               .WithAlias(AppConstants.Param)
            );


         _explicitFormula.FormulaString = "Param*2";

         _topContainer = new Container().WithName("TOP");
         _parentContainer = new Container().WithName("Parent");
         _topContainer.Add(_parentContainer);
         _parameter = new Parameter().WithName("P").WithFormula(_explicitFormula);
         _parameter2 = new Parameter().WithName("P2").WithFormula(new ConstantFormula(2));
         _parentContainer.Add(_parameter);
         _parentContainer.Add(_parameter2);
      }
   }

   public class When_mapping_an_explicit_formula_to_an_explicit_formula_dto_using_an_undefined_using_formula : concern_for_ExplicitFormulaToExplicitFormulaDTOMapper
   {
      private ExplicitFormulaBuilderDTO _dto;

      protected override void Because()
      {
         _dto = sut.MapFrom(_explicitFormula, null);
      }

      [Observation]
      public void should_return_a_dto_containing_unchanged_object_paths()
      {
         _dto.ObjectPaths.Count.ShouldBeEqualTo(1);
         _dto.ObjectPaths[0].Path.ShouldBeEqualTo(new[] {ObjectPath.PARENT_CONTAINER, "P2"}.ToPathString());
      }
   }

   public class When_mapping_an_explicit_formula_to_an_explicit_formula_dto_using_a_defined_using_formula : concern_for_ExplicitFormulaToExplicitFormulaDTOMapper
   {
      private ExplicitFormulaBuilderDTO _dto;

      protected override void Context()
      {
         base.Context();
         _explicitFormula.ResolveObjectPathsFor(_parameter);
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_explicitFormula, _parameter);
      }

      [Observation]
      public void should_return_a_dto_containing_absolute_path_for_object_paths_that_could_be_resolved()
      {
         _dto.ObjectPaths.Count.ShouldBeEqualTo(1);
         _dto.ObjectPaths[0].Path.ShouldBeEqualTo(_entityPathResolver.PathFor(_parameter2));
         _dto.ObjectPaths[0].Alias.ShouldBeEqualTo(AppConstants.Param);
         _dto.ObjectPaths[0].Dimension.ShouldBeEqualTo(DimensionFactoryForSpecs.MassDimension);
      }
   }
}