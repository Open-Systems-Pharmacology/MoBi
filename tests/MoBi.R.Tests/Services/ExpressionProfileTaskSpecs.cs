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

internal class concern_for_ExpressionProfileTask : ContextForIntegration<IExpressionProfileTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetExpressionProfileTask();
   }
}

internal class concern_for_ExpressionProfileTask_with_project : concern_for_ExpressionProfileTask
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

internal class When_setting_expression_parameters_with_inconsistent_array_lengths : concern_for_ExpressionProfileTask
{
   private ExpressionProfileBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ExpressionProfileBuildingBlock().WithName("Expression Profile Building Block");
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetExpressionParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Liver|Protein", "Organism|Kidney|Protein"],
         quantityValues: [1.5] // Only 1 element - inconsistent!
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_expression_parameters_that_all_exist_with_project : concern_for_ExpressionProfileTask_with_project
{
   private ExpressionProfileBuildingBlock _buildingBlock;
   private ExpressionParameter _parameter1;
   private ExpressionParameter _parameter2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ExpressionProfileBuildingBlock().WithName("Expression Profile Building Block");

      _parameter1 = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Liver", "Protein"),
         Value = 1.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(1.0)
      };

      _parameter2 = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Kidney", "Protein"),
         Value = 0.5,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(0.5)
      };

      _buildingBlock.Add(_parameter1);
      _buildingBlock.Add(_parameter2);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetExpressionParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Liver|Protein", "Organism|Kidney|Protein"],
         quantityValues: [2.0, 1.5]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_parameter()
   {
      _buildingBlock["Organism|Liver|Protein".ToObjectPath()].Value.ShouldBeEqualTo(2.0);
   }

   [Observation]
   public void should_update_the_value_of_second_parameter()
   {
      _buildingBlock["Organism|Kidney|Protein".ToObjectPath()].Value.ShouldBeEqualTo(1.5);
   }
}

internal class When_setting_expression_parameters_that_all_exist : concern_for_ExpressionProfileTask
{
   private ExpressionProfileBuildingBlock _buildingBlock;
   private ExpressionParameter _parameter1;
   private ExpressionParameter _parameter2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ExpressionProfileBuildingBlock().WithName("Expression Profile Building Block");

      _parameter1 = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Liver", "Protein"),
         Value = 1.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(1.0)
      };

      _parameter2 = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Kidney", "Protein"),
         Value = 0.5,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(0.5)
      };

      _buildingBlock.Add(_parameter1);
      _buildingBlock.Add(_parameter2);
   }

   protected override void Because()
   {
      sut.SetExpressionParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Liver|Protein", "Organism|Kidney|Protein"],
         quantityValues: [2.0, 1.5]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_parameter()
   {
      _buildingBlock["Organism|Liver|Protein".ToObjectPath()].Value.ShouldBeEqualTo(2.0);
   }

   [Observation]
   public void should_update_the_value_of_second_parameter()
   {
      _buildingBlock["Organism|Kidney|Protein".ToObjectPath()].Value.ShouldBeEqualTo(1.5);
   }
}

internal class When_setting_expression_parameter_that_does_not_exist : concern_for_ExpressionProfileTask
{
   private ExpressionProfileBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ExpressionProfileBuildingBlock().WithName("Expression Profile Building Block");

      var parameter1 = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Liver", "Protein"),
         Value = 1.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(1.0)
      };

      _buildingBlock.Add(parameter1);
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetExpressionParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Liver|Protein", "Organism|Brain|NonExistentProtein"],
         quantityValues: [2.0, 3.0]
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_expression_parameter_with_non_existent_path_in_project : concern_for_ExpressionProfileTask_with_project
{
   private ExpressionProfileBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ExpressionProfileBuildingBlock().WithName("Expression Profile Building Block");

      var parameter1 = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Liver", "Protein"),
         Value = 1.0,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(1.0)
      };

      _buildingBlock.Add(parameter1);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetExpressionParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Liver|Protein", "Organism|Brain|NonExistentProtein"],
         quantityValues: [2.0, 3.0]
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_single_expression_parameter : concern_for_ExpressionProfileTask
{
   private ExpressionProfileBuildingBlock _buildingBlock;
   private ExpressionParameter _parameter;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ExpressionProfileBuildingBlock().WithName("Expression Profile Building Block");

      _parameter = new ExpressionParameter
      {
         Path = new ObjectPath("Organism", "Heart", "Enzyme"),
         Value = 0.75,
         Dimension = Constants.Dimension.NO_DIMENSION,
         Formula = new ConstantFormula(0.75)
      };

      _buildingBlock.Add(_parameter);
   }

   protected override void Because()
   {
      sut.SetExpressionParameter(
         _buildingBlock,
         quantityPaths: ["Organism|Heart|Enzyme"],
         quantityValues: [1.25]
      );
   }

   [Observation]
   public void should_update_the_parameter_value()
   {
      _buildingBlock["Organism|Heart|Enzyme".ToObjectPath()].Value.ShouldBeEqualTo(1.25);
   }
}

internal class When_exporting_expression_profile_to_pkml : concern_for_ExpressionProfileTask_with_project
{
   private ExpressionProfileBuildingBlock _buildingBlock;
   private string _filePath;

   protected override void Context()
   {
      base.Context();
      _filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pkml");
      _buildingBlock = new ExpressionProfileBuildingBlock { Type = ExpressionTypes.MetabolizingEnzyme };
      _buildingBlock.Name = "CYP3A4|Human|Enzyme";
      _buildingBlock.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "CYP3A4"), Value = 1.0 });
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