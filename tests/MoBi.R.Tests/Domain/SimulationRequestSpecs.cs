using System.Collections.Generic;
using System.Linq;
using MoBi.R.Domain;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Domain;

internal abstract class concern_for_SimulationRequest : ContextSpecification<SimulationRequest>
{
   protected override void Context()
   {
      sut = new SimulationRequest();
   }
}

internal class when_adding_a_duplicate_molecule_calculation_method : concern_for_SimulationRequest
{
   private IReadOnlyList<MoleculeCalculationMethodOverride> _result;

   protected override void Context()
   {
      base.Context();
      sut.AddMoleculeUsedCalculationMethod("Molecule1", "Category1", "OriginalMethod");
   }

   protected override void Because()
   {
      sut.AddMoleculeUsedCalculationMethod("Molecule1", "Category1", "ReplacedMethod");
      _result = sut.AllCalculationMethodOverrides();
   }

   [Observation]
   public void should_replace_the_previously_added_calculation_method()
   {
      _result.Count.ShouldBeEqualTo(1);
      _result[0].MoleculeName.ShouldBeEqualTo("Molecule1");
      _result[0].UsedCalculationMethods.Count.ShouldBeEqualTo(1);
      var method = _result[0].UsedCalculationMethods.First();
      method.Category.ShouldBeEqualTo("Category1");
      method.CalculationMethod.ShouldBeEqualTo("ReplacedMethod");
   }
}
