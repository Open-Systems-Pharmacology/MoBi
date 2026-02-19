using System;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Tests.Services;

internal class concern_for_ParameterValuesTask : ContextForIntegration<IParameterValuesTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetParameterValuesTask();
   }
}

internal class concern_for_ParameterValuesTask_with_project : concern_for_ParameterValuesTask
{
   protected MoBiProject _project;
   protected IMoBiContext _context;

   protected override void Context()
   {
      base.Context();
      var projectFile = HelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
      _project = Api.GetProjectTask().LoadProject(projectFile);
      _context = OSPSuite.R.Api.Container.Resolve<IMoBiContext>();
   }

   protected void AddBuildingBlocksToProject(params IBuildingBlock[] buildingBlock)
   {
      var module = new Module();
      buildingBlock.Each(x => module.Add(x));
      module.IsPKSimModule = true;

      _project.AddModule(module);
   }
}

internal class When_removing_parameter_values_from_a_building_block_in_a_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ParameterValue _parameterValue1;
   private ParameterValue _parameterValue2;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _parameterValue1 = new ParameterValue
      {
         Path = new ObjectPath("PV1", "Path1", "PARAMETER")
      };
      _parameterValue2 = new ParameterValue
      {
         Path = new ObjectPath("PV2", "Path2", "PARAMETER")
      };
      _buildingBlock.Add(_parameterValue1);
      _buildingBlock.Add(_parameterValue2);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.DeleteParameterValues(_buildingBlock, ["PV1|Path1|PARAMETER", "PV2|Path2|PARAMETER"]);
   }

   [Observation]
   public void should_remove_the_parameter_value_from_the_building_block()
   {
      _buildingBlock.ShouldNotContain(_parameterValue1);
      _buildingBlock.ShouldNotContain(_parameterValue2);
   }
}

internal class When_removing_parameter_values_from_a_building_block_not_in_a_project : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ParameterValue _parameterValue1;
   private ParameterValue _parameterValue2;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _parameterValue1 = new ParameterValue
      {
         Path = new ObjectPath("PV1", "Path1", "PARAMETER")
      };
      _parameterValue2 = new ParameterValue
      {
         Path = new ObjectPath("PV2", "Path2", "PARAMETER")
      };
      _buildingBlock.Add(_parameterValue1);
      _buildingBlock.Add(_parameterValue2);
   }

   protected override void Because()
   {
      sut.DeleteParameterValues(_buildingBlock, ["PV1|Path1|PARAMETER", "PV2|Path2|PARAMETER"]);
   }

   [Observation]
   public void should_remove_the_parameter_value_from_the_building_block()
   {
      _buildingBlock.ShouldNotContain(_parameterValue1);
      _buildingBlock.ShouldNotContain(_parameterValue2);
   }
}

internal class When_extending_parameter_values_with_all_molecules_for_building_block_in_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      var parameter = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter"
      };
      
      _molecule1.AddParameter(parameter);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);

      _topContainer = new Container().WithMode(ContainerMode.Logical).WithName("TopContainer");
      _spatialStructure.AddTopContainer(_topContainer);
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
   }

   [Observation]
   public void should_add_parameter_values_for_molecules()
   {
      _buildingBlock.Single().Path.PathAsString.ShouldBeEqualTo("TopContainer|physical|Molecule1|parameter");
   }
}

internal class When_extending_parameter_values_with_all_molecules_for_building_block_not_in_project : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);

      var parameter = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter"
      };

      _molecule1.AddParameter(parameter);
      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);
   }

   protected override void Because()
   {
      sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
   }

   [Observation]
   public void should_add_parameter_values_for_molecules()
   {
      _buildingBlock.Single().Path.PathAsString.ShouldBeEqualTo("TopContainer|physical|Molecule1|parameter");
   }
}

internal class When_extending_parameter_values_with_specific_molecules_for_building_block_in_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoleculeBuilder _molecule3;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule3 = new MoleculeBuilder().WithName("Molecule3").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);
      _moleculeBuildingBlock.Add(_molecule3);

      var parameter = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter"
      };
      _molecule3.AddParameter(parameter);
      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, ["Molecule1", "Molecule3"]);
   }

   [Observation]
   public void should_add_parameter_values_for_molecules()
   {
      _buildingBlock.Single().Path.PathAsString.ShouldBeEqualTo("TopContainer|physical|Molecule3|parameter");
   }
}

internal class When_extending_parameter_values_with_specific_molecules_for_building_block_not_in_project : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoleculeBuilder _molecule3;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule3 = new MoleculeBuilder().WithName("Molecule3").WithDimension(Constants.Dimension.NO_DIMENSION);
      var parameter = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter"
      };

      _molecule2.AddParameter(parameter);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);
      _moleculeBuildingBlock.Add(_molecule3);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);
   }

   protected override void Because()
   {
      sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, ["Molecule2"]);
   }

   [Observation]
   public void should_add_parameter_values_for_molecules()
   {
      _buildingBlock.Single().Path.PathAsString.ShouldBeEqualTo("TopContainer|physical|Molecule2|parameter");
   }
}

internal class When_extending_parameter_values_with_empty_molecule_names_array : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);

      var parameter1 = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter1"
      };

      var parameter2 = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter2"
      };

      _molecule1.AddParameter(parameter1);
      _molecule2.AddParameter(parameter2);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, []);
   }

   [Observation]
   public void should_add_parameter_values_for_molecules()
   {
      var paths = _buildingBlock.Select(x => x.Path.PathAsString).ToList();
      paths.ShouldOnlyContain("TopContainer|physical|Molecule1|parameter1", "TopContainer|physical|Molecule2|parameter2");
   }
}

internal class When_setting_parameter_values_with_inconsistent_array_lengths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["Path1|Container1|Parameter1", "Path2|Container2|Parameter2"],
         quantityValues: [100, 200],
         dimensionNames: ["Time"] // Only 1 element - inconsistent!
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_parameter_values_that_all_already_exist_with_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ParameterValue _parameterValue1;
   private ParameterValue _parameterValue2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");

      // Create parameter values first
      _parameterValue1 = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter1"),
         Value = 50
      };

      _parameterValue2 = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter2"),
         Value = 75
      };

      _buildingBlock.Add(_parameterValue1);
      _buildingBlock.Add(_parameterValue2);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1", "TopContainer|Physical|Parameter2"],
         quantityValues: [100, 200],
         dimensionNames: ["Time", "Time"]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_parameter_value()
   {
      var pv1 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_update_the_value_of_second_parameter_value()
   {
      var pv2 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter2");
      pv2.Value.ShouldBeEqualTo(200);
   }
}

internal class When_setting_parameter_values_that_dont_exist_with_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1", "TopContainer|Physical|Parameter2"],
         quantityValues: [150, 250],
         dimensionNames: ["Time", "Time"]
      );
   }

   [Observation]
   public void should_add_new_parameter_values_to_building_block()
   {
      _buildingBlock.Count().ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_create_parameter_value_for_parameter1_with_correct_value()
   {
      var pv1 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv1.ShouldNotBeNull();
      pv1.Value.ShouldBeEqualTo(150);
   }

   [Observation]
   public void should_create_parameter_value_for_parameter2_with_correct_value()
   {
      var pv2 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter2");
      pv2.ShouldNotBeNull();
      pv2.Value.ShouldBeEqualTo(250);
   }
}

internal class When_setting_parameter_values_with_mix_of_existing_and_new_with_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ParameterValue _existingParameterValue;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");

      var topContainer = new Container().WithName("TopContainer");
      var physicalContainer = new Container().WithMode(ContainerMode.Physical).WithName("Physical");
      topContainer.Add(physicalContainer);

      // Add one existing parameter value
      _existingParameterValue = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter1"),
         Value = 50
      };
      _buildingBlock.Add(_existingParameterValue);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1", "TopContainer|Physical|Parameter2", "TopContainer|Physical|Parameter3"],
         quantityValues: [100, 200, 300],
         dimensionNames: ["Time", "Time", "Time"]
      );
   }

   [Observation]
   public void should_update_existing_parameter_value()
   {
      var pv1 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_add_new_parameter_value_for_parameter2()
   {
      var pv2 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter2");
      pv2.ShouldNotBeNull();
      pv2.Value.ShouldBeEqualTo(200);
   }

   [Observation]
   public void should_add_new_parameter_value_for_parameter3()
   {
      var pv3 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter3");
      pv3.ShouldNotBeNull();
      pv3.Value.ShouldBeEqualTo(300);
   }

   [Observation]
   public void should_increase_building_block_count()
   {
      _buildingBlock.Count().ShouldBeEqualTo(3);
   }
}

internal class When_setting_parameter_values_that_all_already_exist : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ParameterValue _parameterValue1;
   private ParameterValue _parameterValue2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");

      // Create parameter values first
      _parameterValue1 = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter1"),
         Value = 50
      };

      _parameterValue2 = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter2"),
         Value = 75
      };

      _buildingBlock.Add(_parameterValue1);
      _buildingBlock.Add(_parameterValue2);
   }

   protected override void Because()
   {
      sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1", "TopContainer|Physical|Parameter2"],
         quantityValues: [100, 200],
         dimensionNames: ["Time", "Time"]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_parameter_value()
   {
      var pv1 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_update_the_value_of_second_parameter_value()
   {
      var pv2 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter2");
      pv2.Value.ShouldBeEqualTo(200);
   }
}

internal class When_setting_parameter_values_that_dont_exist : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
   }

   protected override void Because()
   {
      sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1", "TopContainer|Physical|Parameter2"],
         quantityValues: [150, 250],
         dimensionNames: ["Time", "Time"]
      );
   }

   [Observation]
   public void should_add_new_parameter_values_to_building_block()
   {
      _buildingBlock.Count().ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_create_parameter_value_for_parameter1_with_correct_value()
   {
      var pv1 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv1.ShouldNotBeNull();
      pv1.Value.ShouldBeEqualTo(150);
   }

   [Observation]
   public void should_create_parameter_value_for_parameter2_with_correct_value()
   {
      var pv2 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter2");
      pv2.ShouldNotBeNull();
      pv2.Value.ShouldBeEqualTo(250);
   }
}

internal class When_setting_parameter_values_with_mix_of_existing_and_new : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ParameterValue _existingParameterValue;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");

      var topContainer = new Container().WithName("TopContainer");
      var physicalContainer = new Container().WithMode(ContainerMode.Physical).WithName("Physical");
      topContainer.Add(physicalContainer);

      // Add one existing parameter value
      _existingParameterValue = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter1"),
         Value = 50
      };
      _buildingBlock.Add(_existingParameterValue);
   }

   protected override void Because()
   {
      sut.SetParameterValue(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1", "TopContainer|Physical|Parameter2", "TopContainer|Physical|Parameter3"],
         quantityValues: [100, 200, 300],
         dimensionNames: ["Time", "Time", "Time"]
      );
   }

   [Observation]
   public void should_update_existing_parameter_value()
   {
      var pv1 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_add_new_parameter_value_for_parameter2()
   {
      var pv2 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter2");
      pv2.ShouldNotBeNull();
      pv2.Value.ShouldBeEqualTo(200);
   }

   [Observation]
   public void should_add_new_parameter_value_for_parameter3()
   {
      var pv3 = _buildingBlock.FindByPath("TopContainer|Physical|Parameter3");
      pv3.ShouldNotBeNull();
      pv3.Value.ShouldBeEqualTo(300);
   }

   [Observation]
   public void should_increase_building_block_count()
   {
      _buildingBlock.Count().ShouldBeEqualTo(3);
   }
}