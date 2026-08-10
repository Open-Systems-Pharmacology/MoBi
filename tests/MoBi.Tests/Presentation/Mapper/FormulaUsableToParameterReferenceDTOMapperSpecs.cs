using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_FormulaUsableToParameterReferenceDTOMapper : ContextSpecification<FormulaUsableToParameterReferenceDTOMapper>
   {
      protected IObjectTypeResolver _objectTypeResolver;
      protected IMoBiSimulation _simulation;
      protected IParameter _parameter;
      protected IParameter _referencingParameter;
      protected IReadOnlyList<ParameterReferenceDTO> _result;

      protected override void Context()
      {
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         sut = new FormulaUsableToParameterReferenceDTOMapper(new EntityPathResolver(new ObjectPathFactory(new AliasCreator())), _objectTypeResolver);

         var root = new Container { ContainerType = ContainerType.Simulation }.WithName("Sim");
         var organism = new Container().WithName("Organism");
         root.Add(organism);

         _parameter = new Parameter().WithName("P").WithFormula(new ConstantFormula(1));
         organism.Add(_parameter);

         //references the parameter with an absolute path
         var referencingFormula = new ExplicitFormula("P * 2").WithName("F1");
         referencingFormula.AddObjectPath(new FormulaUsablePath("Sim", "Organism", "P").WithAlias("P"));
         _referencingParameter = new Parameter().WithName("P2").WithFormula(referencingFormula);
         organism.Add(_referencingParameter);

         //references the parameter in the RHS formula with a relative path
         var rhsFormula = new ExplicitFormula("P + 1").WithName("F2");
         rhsFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "P").WithAlias("PRel"));
         var parameterWithRHS = new Parameter().WithName("P3").WithFormula(new ConstantFormula(1));
         parameterWithRHS.RHSFormula = rhsFormula;
         organism.Add(parameterWithRHS);

         //references another parameter
         organism.Add(new Parameter().WithName("Q").WithFormula(new ConstantFormula(2)));
         var otherFormula = new ExplicitFormula("Q * 2").WithName("F3");
         otherFormula.AddObjectPath(new FormulaUsablePath("Sim", "Organism", "Q").WithAlias("Q"));
         organism.Add(new Parameter().WithName("P4").WithFormula(otherFormula));

         new ReferencesResolver().ResolveReferencesIn(root);

         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.Model).Returns(new Model { Root = root });
      }
   }

   public class When_mapping_the_references_to_a_parameter_used_in_a_simulation : concern_for_FormulaUsableToParameterReferenceDTOMapper
   {
      protected override void Because()
      {
         _result = sut.MapFrom(_parameter, _simulation);
      }

      [Observation]
      public void should_map_the_object_referencing_the_parameter_in_its_formula()
      {
         var reference = _result.Single(x => string.Equals(x.UsedBy, "Organism|P2"));
         reference.UsedByObject.ShouldBeEqualTo(_referencingParameter);
         reference.UsedAs.ShouldBeEqualTo(AppConstants.Captions.Formula);
         reference.Alias.ShouldBeEqualTo("P");
         reference.FormulaName.ShouldBeEqualTo("F1");
         reference.Formula.ShouldBeEqualTo("P * 2");
      }

      [Observation]
      public void should_map_the_object_referencing_the_parameter_in_its_right_hand_side_formula()
      {
         var reference = _result.Single(x => string.Equals(x.UsedBy, "Organism|P3"));
         reference.UsedAs.ShouldBeEqualTo(AppConstants.Captions.RHSFormula);
         reference.Alias.ShouldBeEqualTo("PRel");
         reference.FormulaName.ShouldBeEqualTo("F2");
      }

      [Observation]
      public void should_not_map_objects_that_do_not_reference_the_parameter()
      {
         _result.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_mapping_the_references_to_a_parameter_that_is_not_used_anywhere : concern_for_FormulaUsableToParameterReferenceDTOMapper
   {
      protected override void Because()
      {
         _result = sut.MapFrom(_referencingParameter, _simulation);
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _result.ShouldBeEmpty();
      }
   }
}
