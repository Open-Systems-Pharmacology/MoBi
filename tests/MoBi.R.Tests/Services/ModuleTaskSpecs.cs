using System.Linq;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Exceptions;
using static MoBi.R.Tests.HelperForSpecs;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_ModuleTask : ContextForIntegration<IModuleTask>
{
   protected override void Context()
   {
      base.Context();
      sut = Api.GetModuleTask();
   }
}

internal class When_getting_parameter_values : concern_for_ModuleTask
{
   private Module _module;

   protected override void Context()
   {
      base.Context();
      _module = sut.LoadModulesFromFile(DataTestFileFullPath("simulation with two modules.pkml")).First();
   }

   [Observation]
   public void returns_null_when_not_found()
   {
      sut.ParameterValuesBuildingBlockByName(_module, "not_there").ShouldBeNull();
   }

   [Observation]
   public void returns_building_block_when_found()
   {
      sut.ParameterValuesBuildingBlockByName(_module, "P2").ShouldBeAnInstanceOf<ParameterValuesBuildingBlock>();
   }

   [Observation]
   public void returns_all_building_block_names_in_module()
   {
      var allInitialConditionsBuildingBlockNames = sut.AllParameterValuesBuildingBlockNames(_module);
      allInitialConditionsBuildingBlockNames.Length.ShouldBeEqualTo(1);
      allInitialConditionsBuildingBlockNames.ShouldContain("P2");
   }
}

internal class When_getting_initial_conditions : concern_for_ModuleTask
{
   private Module _module;

   protected override void Context()
   {
      base.Context();
      _module = sut.LoadModulesFromFile(DataTestFileFullPath("simulation with two modules.pkml")).First();
   }

   [Observation]
   public void returns_null_when_not_found()
   {
      sut.InitialConditionBuildingBlockByName(_module, "not_there").ShouldBeNull();
   }

   [Observation]
   public void returns_building_block_when_found()
   {
      sut.InitialConditionBuildingBlockByName(_module, "M2").ShouldBeAnInstanceOf<InitialConditionsBuildingBlock>();
   }

   [Observation]
   public void returns_all_building_block_names_in_module()
   {
      var allInitialConditionsBuildingBlockNames = sut.AllInitialConditionsBuildingBlockNames(_module);
      allInitialConditionsBuildingBlockNames.Length.ShouldBeEqualTo(1);
      allInitialConditionsBuildingBlockNames.ShouldContain("M2");
   }
}

internal class When_loading_file_with_multiple_modules : concern_for_ModuleTask
{
   private Module[] _result;

   protected override void Because()
   {
      _result = sut.LoadModulesFromFile(DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void the_result_should_be_multiple_modules()
   {
      _result.Length.ShouldBeEqualTo(2);
   }
}

internal class When_creating_a_module : concern_for_ModuleTask
{
   private Module _result;

   protected override void Because()
   {
      _result = sut.CreateModule("test");
   }

   [Observation]
   public void the_result_should_have_the_given_name()
   {
      _result.Name.ShouldBeEqualTo("test");
   }

   [Observation]
   public void the_result_should_have_no_single_type_building_blocks()
   {
      _result.Molecules.ShouldBeNull();
      _result.Reactions.ShouldBeNull();
      _result.SpatialStructure.ShouldBeNull();
      _result.PassiveTransports.ShouldBeNull();
      _result.Observers.ShouldBeNull();
      _result.EventGroups.ShouldBeNull();
   }

   [Observation]
   public void the_result_should_have_no_initial_conditions_or_parameter_values()
   {
      _result.InitialConditionsCollection.ShouldBeEmpty();
      _result.ParameterValuesCollection.ShouldBeEmpty();
   }
}

internal class When_creating_a_module_with_building_blocks : concern_for_ModuleTask
{
   private MoleculeBuildingBlock _molecules;
   private ReactionBuildingBlock _reactions;
   private InitialConditionsBuildingBlock _initialConditions;
   private Module _result;

   protected override void Context()
   {
      base.Context();
      _molecules = new MoleculeBuildingBlock { Name = "M" };
      _reactions = new ReactionBuildingBlock { Name = "R" };
      _initialConditions = new InitialConditionsBuildingBlock { Name = "IC" };
   }

   protected override void Because()
   {
      _result = sut.CreateModule("test", _molecules, _reactions, _initialConditions);
   }

   [Observation]
   public void the_result_should_have_the_given_name()
   {
      _result.Name.ShouldBeEqualTo("test");
   }

   [Observation]
   public void the_result_should_contain_the_given_building_blocks()
   {
      _result.Molecules.ShouldBeEqualTo(_molecules);
      _result.Reactions.ShouldBeEqualTo(_reactions);
      _result.InitialConditionsCollection.ShouldContain(_initialConditions);
   }
}

internal class When_adding_a_single_building_block_to_a_module : concern_for_ModuleTask
{
   private Module _module;
   private MoleculeBuildingBlock _molecules;

   protected override void Context()
   {
      base.Context();
      _module = sut.CreateModule("test");
      _molecules = new MoleculeBuildingBlock { Name = "M" };
   }

   protected override void Because()
   {
      sut.AddBuildingBlocksToModule(_module, _molecules);
   }

   [Observation]
   public void the_module_should_contain_the_building_block()
   {
      _module.Molecules.ShouldBeEqualTo(_molecules);
   }
}

internal class When_adding_multiple_different_building_blocks_via_params : concern_for_ModuleTask
{
   private Module _module;
   private MoleculeBuildingBlock _molecules;
   private ReactionBuildingBlock _reactions;

   protected override void Context()
   {
      base.Context();
      _module = sut.CreateModule("test");
      _molecules = new MoleculeBuildingBlock { Name = "M" };
      _reactions = new ReactionBuildingBlock { Name = "R" };
   }

   protected override void Because()
   {
      sut.AddBuildingBlocksToModule(_module, _molecules, _reactions);
   }

   [Observation]
   public void the_module_should_contain_the_molecules_building_block()
   {
      _module.Molecules.ShouldBeEqualTo(_molecules);
   }

   [Observation]
   public void the_module_should_contain_the_reactions_building_block()
   {
      _module.Reactions.ShouldBeEqualTo(_reactions);
   }
}

internal class When_adding_a_duplicate_single_type_building_block_to_a_module : concern_for_ModuleTask
{
   private Module _module;

   protected override void Context()
   {
      base.Context();
      _module = sut.CreateModule("test");
      sut.AddBuildingBlocksToModule(_module, new MoleculeBuildingBlock { Name = "M1" });
   }

   [Observation]
   public void should_throw_when_adding_another_building_block_of_the_same_single_type()
   {
      The.Action(() => sut.AddBuildingBlocksToModule(_module, new MoleculeBuildingBlock { Name = "M2" }))
         .ShouldThrowAn<OSPSuiteException>();
   }
}

internal class When_adding_multiple_initial_conditions_building_blocks_to_a_module : concern_for_ModuleTask
{
   private Module _module;
   private InitialConditionsBuildingBlock _initialConditions1;
   private InitialConditionsBuildingBlock _initialConditions2;

   protected override void Context()
   {
      base.Context();
      _module = sut.CreateModule("test");
      _initialConditions1 = new InitialConditionsBuildingBlock { Name = "IC1" };
      _initialConditions2 = new InitialConditionsBuildingBlock { Name = "IC2" };
   }

   protected override void Because()
   {
      sut.AddBuildingBlocksToModule(_module, _initialConditions1, _initialConditions2);
   }

   [Observation]
   public void the_module_should_contain_both_building_blocks()
   {
      _module.InitialConditionsCollection.Count().ShouldBeEqualTo(2);
      _module.InitialConditionsCollection.ShouldContain(_initialConditions1);
      _module.InitialConditionsCollection.ShouldContain(_initialConditions2);
   }
}

internal class When_adding_multiple_parameter_values_building_blocks_to_a_module : concern_for_ModuleTask
{
   private Module _module;
   private ParameterValuesBuildingBlock _parameterValues1;
   private ParameterValuesBuildingBlock _parameterValues2;

   protected override void Context()
   {
      base.Context();
      _module = sut.CreateModule("test");
      _parameterValues1 = new ParameterValuesBuildingBlock { Name = "PV1" };
      _parameterValues2 = new ParameterValuesBuildingBlock { Name = "PV2" };
   }

   protected override void Because()
   {
      sut.AddBuildingBlocksToModule(_module, _parameterValues1, _parameterValues2);
   }

   [Observation]
   public void the_module_should_contain_both_building_blocks()
   {
      _module.ParameterValuesCollection.Count().ShouldBeEqualTo(2);
      _module.ParameterValuesCollection.ShouldContain(_parameterValues1);
      _module.ParameterValuesCollection.ShouldContain(_parameterValues2);
   }
}

internal class When_removing_a_building_block_from_a_module : concern_for_ModuleTask
{
   private Module _module;
   private MoleculeBuildingBlock _molecules;

   protected override void Context()
   {
      base.Context();
      _module = sut.CreateModule("test");
      _molecules = new MoleculeBuildingBlock { Name = "M" };
      sut.AddBuildingBlocksToModule(_module, _molecules);
   }

   protected override void Because()
   {
      sut.RemoveBuildingBlockFromModule(_module, _molecules);
   }

   [Observation]
   public void the_module_should_no_longer_contain_the_building_block()
   {
      _module.Molecules.ShouldBeNull();
   }
}