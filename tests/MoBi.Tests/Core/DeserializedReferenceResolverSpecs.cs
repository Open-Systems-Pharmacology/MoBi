using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;

namespace MoBi.Core
{
   public abstract class concern_for_DeserializedReferenceResolver : ContextSpecification<IDeserializedReferenceResolver>
   {
      protected IReferencesResolver _referenceResolver;
      protected ISimulationParameterOriginIdUpdater _simulationParameterOriginUpdater;

      protected MoBiProject _project;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         _referenceResolver = A.Fake<IReferencesResolver>();
         _simulationParameterOriginUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();
         sut = new DeserializedReferenceResolver(_referenceResolver,_simulationParameterOriginUpdater);

         _project= A.Fake<MoBiProject>();
         _simulation= A.Fake<IMoBiSimulation>();
      }
   }

   public class When_resolving_the_formula_and_template_references_in_a_deserialized_simulation : concern_for_DeserializedReferenceResolver
   {
      protected override void Because()
      {
         sut.ResolveFormulaAndTemplateReferences(_simulation,_project);
      }

      [Observation]
      public void should_resolve_the_reference_in_the_simulation()
      {
         A.CallTo(() => _referenceResolver.ResolveReferencesIn(_simulation.Model)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_parameter_origin_for_parameters_defined_in_the_simulation()
      {
         A.CallTo(() => _simulationParameterOriginUpdater.UpdateSimulationId(_simulation)).MustHaveHappened();
      }
   }


   public class When_resolving_the_formula_and_template_references_in_a_deserialized_simulation_transfer : concern_for_DeserializedReferenceResolver
   {
      private SimulationTransfer _simulationTransfer;

      protected override void Context()
      {
         base.Context();
         _simulationTransfer = new SimulationTransfer {Simulation = _simulation};
      }
      protected override void Because()
      {
         sut.ResolveFormulaAndTemplateReferences(_simulationTransfer, _project);
      }

      [Observation]
      public void should_resolve_the_reference_in_the_simulation()
      {
         A.CallTo(() => _referenceResolver.ResolveReferencesIn(_simulation.Model)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_parameter_origin_for_parameters_defined_in_the_simulation()
      {
         A.CallTo(() => _simulationParameterOriginUpdater.UpdateSimulationId(_simulation)).MustHaveHappened();
      }
   }
}	