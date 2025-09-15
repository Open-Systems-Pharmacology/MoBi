using System.Linq;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
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
      sut.ParameterValueBuildingBlockByName(_module, "not_there").ShouldBeNull();
   }

   [Observation]
   public void returns_building_block_when_found()
   {
      sut.ParameterValueBuildingBlockByName(_module, "P2").ShouldBeAnInstanceOf<ParameterValuesBuildingBlock>();
   }

   [Observation]
   public void returns_all_building_block_names_in_module()
   {
      var allInitialConditionsBuildingBlockNames = sut.AllParameterValueBuildingBlockNames(_module);
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