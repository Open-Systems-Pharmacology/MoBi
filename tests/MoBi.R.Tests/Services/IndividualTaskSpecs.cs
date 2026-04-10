using System;
using System.IO;
using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.R.Tests.Services;

internal class concern_for_IndividualTask : ContextForIntegration<IIndividualTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetIndividualTask();
   }
}

internal class concern_for_IndividualTask_with_project : concern_for_IndividualTask
{
   private MoBiProject _project;

   protected override void Context()
   {
      base.Context();
      var projectFile = HelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
      _project = Api.GetProjectTask().LoadProject(projectFile);
   }

   protected void AddBuildingBlocksToProject(IBuildingBlock buildingBlock)
   {
      var module = new Module { buildingBlock };

      _project.AddModule(module);
   }
}

internal class When_setting_individual_parameters_with_inconsistent_array_lengths : concern_for_IndividualTask
{
   private IndividualBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new IndividualBuildingBlock().WithName("Individual Building Block");
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetIndividualParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Weight", "Organism|Height"],
         quantityValues: [70.0] // Only 1 element - inconsistent!
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_individual_parameters_that_all_exist_with_project : concern_for_IndividualTask_with_project
{
   private IndividualBuildingBlock _buildingBlock;
   private IndividualParameter _parameter1;
   private IndividualParameter _parameter2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new IndividualBuildingBlock().WithName("Individual Building Block");

      _parameter1 = new IndividualParameter
      {
         Path = new ObjectPath("Organism", "Weight"),
         Value = 70.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(70.0)
      };

      _parameter2 = new IndividualParameter
      {
         Path = new ObjectPath("Organism", "Height"),
         Value = 180.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(180.0)
      };

      _buildingBlock.Add(_parameter1);
      _buildingBlock.Add(_parameter2);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetIndividualParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Weight", "Organism|Height"],
         quantityValues: [75.0, 185.0]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_parameter()
   {
      _buildingBlock["Organism|Weight".ToObjectPath()].Value.ShouldBeEqualTo(75.0);
   }

   [Observation]
   public void should_update_the_value_of_second_parameter()
   {
      _buildingBlock["Organism|Height".ToObjectPath()].Value.ShouldBeEqualTo(185.0);
   }
}

internal class When_setting_individual_parameters_that_all_exist : concern_for_IndividualTask
{
   private IndividualBuildingBlock _buildingBlock;
   private IndividualParameter _parameter1;
   private IndividualParameter _parameter2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new IndividualBuildingBlock().WithName("Individual Building Block");

      _parameter1 = new IndividualParameter
      {
         Path = new ObjectPath("Organism", "Weight"),
         Value = 70.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(70.0)
      };

      _parameter2 = new IndividualParameter
      {
         Path = new ObjectPath("Organism", "Height"),
         Value = 180.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(180.0)
      };

      _buildingBlock.Add(_parameter1);
      _buildingBlock.Add(_parameter2);
   }

   protected override void Because()
   {
      sut.SetIndividualParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Weight", "Organism|Height"],
         quantityValues: [75.0, 185.0]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_parameter()
   {
      _buildingBlock["Organism|Weight".ToObjectPath()].Value.ShouldBeEqualTo(75.0);
   }

   [Observation]
   public void should_update_the_value_of_second_parameter()
   {
      _buildingBlock["Organism|Height".ToObjectPath()].Value.ShouldBeEqualTo(185.0);
   }
}

internal class When_setting_individual_parameter_that_does_not_exist : concern_for_IndividualTask
{
   private IndividualBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new IndividualBuildingBlock().WithName("Individual Building Block");

      var parameter1 = new IndividualParameter
      {
         Path = new ObjectPath("Organism", "Weight"),
         Value = 70.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(70.0)
      };

      _buildingBlock.Add(parameter1);
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetIndividualParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Weight", "Organism|NonExistentParameter"],
         quantityValues: [75.0, 100.0]
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_individual_parameter_with_non_existent_path_in_project : concern_for_IndividualTask_with_project
{
   private IndividualBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new IndividualBuildingBlock().WithName("Individual Building Block");

      var parameter1 = new IndividualParameter
      {
         Path = new ObjectPath("Organism", "Weight"),
         Value = 70.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(70.0)
      };

      _buildingBlock.Add(parameter1);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetIndividualParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Weight", "Organism|NonExistentParameter"],
         quantityValues: [75.0, 100.0]
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_exporting_individual_to_pkml : concern_for_IndividualTask_with_project
{
   private IndividualBuildingBlock _buildingBlock;
   private string _filePath;

   protected override void Context()
   {
      base.Context();
      _filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pkml");
      _buildingBlock = new IndividualBuildingBlock().WithName("Individual Export Test");
      _buildingBlock.Add(new IndividualParameter { Path = new ObjectPath("Organism", "Weight"), Value = 73.0 });
      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.ExportToPKML(_buildingBlock, _filePath);
   }

   [Observation]
   public void should_create_a_valid_pkml_file()
   {
      File.Exists(_filePath).ShouldBeTrue();
      XDocument.Load(_filePath).ShouldNotBeNull();
   }

   public override void Cleanup()
   {
      base.Cleanup();
      File.Delete(_filePath);
   }
}