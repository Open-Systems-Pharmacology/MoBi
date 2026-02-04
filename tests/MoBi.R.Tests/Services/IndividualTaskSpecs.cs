using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Populations;
using MoBi.R.Services;
using OSPSuite.R.Domain;
using ParameterValue = OSPSuite.Core.Domain.Populations.ParameterValue;

namespace MoBi.R.Tests.Services
{
   internal abstract class concern_for_IndividualTask : ContextSpecification<IIndividualTask>
   {
      protected override void Context()
      {
         sut = new IndividualTask();
      }
   }

   internal class when_creating_individual_with_name : concern_for_IndividualTask
   {
      private IndividualBuildingBlock _individual;

      protected override void Because()
      {
         _individual = sut.CreateIndividual("John Doe");
      }

      [Observation]
      public void should_create_individual_with_given_name()
      {
         _individual.ShouldNotBeNull();
         _individual.Name.ShouldBeEqualTo("John Doe");
      }
   }

   internal class when_adding_distributed_parameter : concern_for_IndividualTask
   {
      private IndividualBuildingBlock _individual;

      protected override void Context()
      {
         base.Context();
         _individual = new IndividualBuildingBlock();
      }

      protected override void Because()
      {
         var pv = new ParameterValue(new ObjectPath("Organism","Liver","Volume").ToString(), 1.23, percentile: 0.5);
         var paramWithUnit = new ParameterValueWithUnit(pv, "L");

         sut.AddDistributedParameter(_individual, paramWithUnit);
      }

      [Observation]
      public void should_add_parameter_with_correct_path_and_value()
      {
         var expectedPath = new ObjectPath("Organism", "Liver", "Volume").ToString();
         var added = _individual.SingleOrDefault(x => x.Path.ToString() == expectedPath);
         added.ShouldNotBeNull();
         added.Value.ShouldBeEqualTo(1.23);
      }
   }

   internal class when_adding_derived_parameter : concern_for_IndividualTask
   {
      private IndividualBuildingBlock _individual;

      protected override void Context()
      {
         base.Context();
         _individual = new IndividualBuildingBlock();
      }

      protected override void Because()
      {
         var pv = new ParameterValue(new ObjectPath("Organism", "Kidney", "BloodFlow").ToString(), 5.6, percentile: 0.5);
         var paramWithUnit = new ParameterValueWithUnit(pv, "L/h");

         sut.AddDerivedParameter(_individual, paramWithUnit);
      }

      [Observation]
      public void should_add_parameter_with_correct_path_and_value()
      {
         var expectedPath = new ObjectPath("Organism", "Kidney", "BloodFlow").ToString();
         var added = _individual.SingleOrDefault(x => x.Path.ToString() == expectedPath);
         added.ShouldNotBeNull();
         added.Value.ShouldBeEqualTo(5.6);
      }
   }

   internal class when_adding_null_parameters_or_individual : concern_for_IndividualTask
   {
      private IndividualBuildingBlock _individual;

      protected override void Context()
      {
         base.Context();
         _individual = new IndividualBuildingBlock();
      }

      protected override void Because()
      {
         // null parameter should be ignored
         sut.AddDistributedParameter(_individual, null);
         sut.AddDerivedParameter(_individual, null);

         // null individual should be ignored
         var pv = new ParameterValue(new ObjectPath("A").ToString(), 1.0, percentile: 0.5);
         var paramWithUnit = new ParameterValueWithUnit(pv);
         sut.AddDistributedParameter(null, paramWithUnit);
         sut.AddDerivedParameter(null, paramWithUnit);
      }

      [Observation]
      public void should_not_add_any_parameter()
      {
         _individual.Count().ShouldBeEqualTo(0);
      }
   }

   internal class when_creating_individual_from_parameter_lists : concern_for_IndividualTask
   {
      private IndividualBuildingBlock _individual;

      protected override void Because()
      {
         var distributed = new[]
         {
            new ParameterValueWithUnit(new ParameterValue(new ObjectPath("Organism","Liver","Volume").ToString(), 3.14, percentile: 0.5), "L"),
            new ParameterValueWithUnit(new ParameterValue(new ObjectPath("Organism","Heart","Volume").ToString(), 2.71, percentile: 0.5), "L")
         };

         var derived = new[]
         {
            new ParameterValueWithUnit(new ParameterValue(new ObjectPath("Organism","Kidney","Flow").ToString(), 10.0, percentile: 0.5), "L/h")
         };

         _individual = sut.CreateIndividualFrom("Jane Doe", distributed, derived);
      }

      [Observation]
      public void should_create_individual_with_given_name()
      {
         _individual.Name.ShouldBeEqualTo("Jane Doe");
      }

      [Observation]
      public void should_add_all_distributed_and_derived_parameters()
      {
         _individual.Count().ShouldBeEqualTo(3);

         _individual.SingleOrDefault(x => x.Path.ToString() == new ObjectPath("Organism", "Liver", "Volume").ToString()).Value.ShouldBeEqualTo(3.14);
         _individual.SingleOrDefault(x => x.Path.ToString() == new ObjectPath("Organism", "Heart", "Volume").ToString()).Value.ShouldBeEqualTo(2.71);
         _individual.SingleOrDefault(x => x.Path.ToString() == new ObjectPath("Organism", "Kidney", "Flow").ToString()).Value.ShouldBeEqualTo(10.0);
      }
   }

   internal class when_creating_individual_from_null_lists : concern_for_IndividualTask
   {
      private IndividualBuildingBlock _individual;

      protected override void Because()
      {
         _individual = sut.CreateIndividualFrom("NoParams", null, null);
      }

      [Observation]
      public void should_create_individual_with_no_parameters()
      {
         _individual.Name.ShouldBeEqualTo("NoParams");
         _individual.Count().ShouldBeEqualTo(0);
      }
   }
}