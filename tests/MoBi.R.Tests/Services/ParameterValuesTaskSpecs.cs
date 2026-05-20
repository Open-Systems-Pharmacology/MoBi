using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
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

internal class When_extending_parameter_values_returns_only_added_paths_for_building_block_in_project : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private IContainer _topContainer;
   private string[] _result;
   private string _preExistingPath;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      var molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);

      var parameter1 = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter"
      };
      var parameter2 = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(5.0),
         Name = "parameter"
      };

      molecule1.AddParameter(parameter1);
      molecule2.AddParameter(parameter2);
      _moleculeBuildingBlock.Add(molecule1);
      _moleculeBuildingBlock.Add(molecule2);

      _topContainer = new Container().WithMode(ContainerMode.Logical).WithName("TopContainer");
      _spatialStructure.AddTopContainer(_topContainer);
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));

      // Pre-populate the building block with one parameter value that will also be generated by extend
      _preExistingPath = "TopContainer|physical|Molecule1|parameter";
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "physical", "Molecule1", "parameter"),
         Value = 1.0
      });

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
   }

   [Observation]
   public void should_not_include_pre_existing_path_in_returned_paths()
   {
      _result.ShouldNotContain(_preExistingPath);
   }

   [Observation]
   public void should_return_only_newly_added_paths()
   {
      _result.Length.ShouldBeEqualTo(1);
      _result.ShouldContain("TopContainer|physical|Molecule2|parameter");
   }
}

internal class When_extending_parameter_values_returns_only_added_paths_for_building_block_not_in_project : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private IContainer _topContainer;
   private string[] _result;
   private string _preExistingPath;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      var molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);

      var parameter1 = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(4.0),
         Name = "parameter"
      };
      var parameter2 = new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(5.0),
         Name = "parameter"
      };

      molecule1.AddParameter(parameter1);
      molecule2.AddParameter(parameter2);
      _moleculeBuildingBlock.Add(molecule1);
      _moleculeBuildingBlock.Add(molecule2);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      // Pre-populate the building block with one parameter value that will also be generated by extend
      _preExistingPath = "TopContainer|physical|Molecule1|parameter";
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "physical", "Molecule1", "parameter"),
         Value = 1.0
      });
   }

   protected override void Because()
   {
      _result = sut.AddLocalMoleculeParameters(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
   }

   [Observation]
   public void should_not_include_pre_existing_path_in_returned_paths()
   {
      _result.ShouldNotContain(_preExistingPath);
   }

   [Observation]
   public void should_return_only_newly_added_paths()
   {
      _result.Length.ShouldBeEqualTo(1);
      _result.ShouldContain("TopContainer|physical|Molecule2|parameter");
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
      The.Action(() => sut.SetParameterValues(
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
      sut.SetParameterValues(
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

internal class When_setting_parameter_values_with_different_dimension_on_existing_entries : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private IDimension _originalDimension;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");

      var dimensionFactory = OSPSuite.R.Api.Container.Resolve<IDimensionFactory>();
      _originalDimension = dimensionFactory.Dimension("Volume");

      var parameterValue = new ParameterValue
      {
         Path = new ObjectPath("TopContainer", "Physical", "Parameter1"),
         Value = 1.5,
         Dimension = _originalDimension
      };

      _buildingBlock.Add(parameterValue);
      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetParameterValues(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Parameter1"],
         quantityValues: [1],
         dimensionNames: ["Amount"]
      );
   }

   [Observation]
   public void should_update_the_dimension_of_the_parameter_value()
   {
      var pv = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv.Dimension.Name.ShouldBeEqualTo("Amount");
   }

   [Observation]
   public void should_update_the_value()
   {
      var pv = _buildingBlock.FindByPath("TopContainer|Physical|Parameter1");
      pv.Value.ShouldBeEqualTo(1);
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
      sut.SetParameterValues(
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
      sut.SetParameterValues(
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
      sut.SetParameterValues(
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
      sut.SetParameterValues(
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
      sut.SetParameterValues(
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

internal class When_getting_all_paths_from_building_block : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue { Path = new ObjectPath("Path1", "Container1", "Parameter1") });
      _buildingBlock.Add(new ParameterValue { Path = new ObjectPath("Path2", "Container2", "Parameter2") });
      _buildingBlock.Add(new ParameterValue { Path = new ObjectPath("Path3", "Container3", "Parameter3") });
   }

   protected override void Because()
   {
      _result = sut.AllPathsFrom(_buildingBlock);
   }

   [Observation]
   public void should_return_all_paths()
   {
      _result.Length.ShouldBeEqualTo(3);
      _result.ShouldContain("Path1|Container1|Parameter1");
      _result.ShouldContain("Path2|Container2|Parameter2");
      _result.ShouldContain("Path3|Container3|Parameter3");
   }
}

internal class When_getting_all_paths_from_empty_building_block : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
   }

   protected override void Because()
   {
      _result = sut.AllPathsFrom(_buildingBlock);
   }

   [Observation]
   public void should_return_empty_array()
   {
      _result.ShouldBeEmpty();
   }
}

internal class When_getting_all_values_from_building_block_with_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private double[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path3", "Container3", "Parameter3"),
         Value = 300.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllValuesFrom(_buildingBlock, ["Path1|Container1|Parameter1", "Path3|Container3|Parameter3"]);
   }

   [Observation]
   public void should_return_values_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeEqualTo(100.0);
      _result[1].ShouldBeEqualTo(300.0);
   }
}

internal class When_getting_all_values_from_building_block_without_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private double[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllValuesFrom(_buildingBlock, []);
   }

   [Observation]
   public void should_return_all_values()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result.ShouldContain(100.0);
      _result.ShouldContain(200.0);
   }
}

internal class When_getting_all_values_with_null_value : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private double[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = null,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllValuesFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_nan_for_null_values()
   {
      _result.Length.ShouldBeEqualTo(1);
      double.IsNaN(_result[0]).ShouldBeTrue();
   }
}

internal class When_getting_all_values_with_duplicate_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllValuesFrom(_buildingBlock, ["Path1|Container1|Parameter1", "Path1|Container1|Parameter1"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_values_with_non_existent_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllValuesFrom(_buildingBlock, ["Path1|Container1|Parameter1", "NonExistent|Path"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_dimensions_from_building_block_with_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = new Dimension(new BaseDimensionRepresentation(), "Time", "min")
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path3", "Container3", "Parameter3"),
         Value = 300.0,
         Dimension = new Dimension(new BaseDimensionRepresentation(), "Concentration", "mol/l")
      });
   }

   protected override void Because()
   {
      _result = sut.AllDimensionsFrom(_buildingBlock, ["Path2|Container2|Parameter2", "Path3|Container3|Parameter3"]);
   }

   [Observation]
   public void should_return_dimension_names_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeEqualTo("Time");
      _result[1].ShouldBeEqualTo("Concentration");
   }
}

internal class When_getting_all_base_units_from_building_block_with_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = new Dimension(new BaseDimensionRepresentation(), "Time", "min")
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path3", "Container3", "Parameter3"),
         Value = 300.0,
         Dimension = new Dimension(new BaseDimensionRepresentation(), "Concentration", "mol/l")
      });
   }

   protected override void Because()
   {
      _result = sut.AllUnitsFrom(_buildingBlock, ["Path2|Container2|Parameter2", "Path3|Container3|Parameter3"]);
   }

   [Observation]
   public void should_return_unit_names_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeEqualTo("min");
      _result[1].ShouldBeEqualTo("mol/l");
   }
}

internal class When_getting_all_dimensions_from_building_block_without_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = new Dimension(new BaseDimensionRepresentation(), "Time", "min")
      });
   }

   protected override void Because()
   {
      _result = sut.AllDimensionsFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_all_dimension_names()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result.ShouldContain(Constants.Dimension.NO_DIMENSION.Name);
      _result.ShouldContain("Time");
   }
}

internal class When_getting_all_dimensions_with_duplicate_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllDimensionsFrom(_buildingBlock, ["Path1|Container1|Parameter1", "Path1|Container1|Parameter1"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_dimensions_with_non_existent_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllDimensionsFrom(_buildingBlock, ["Path1|Container1|Parameter1", "NonExistent|Path"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_value_origins_from_building_block_with_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      var pv1 = new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      };
      pv1.ValueOrigin.UpdateFrom(new ValueOrigin { Source = ValueOriginSources.Database, Method = ValueOriginDeterminationMethods.InVitro });
      _buildingBlock.Add(pv1);

      var pv2 = new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      };
      pv2.ValueOrigin.UpdateFrom(new ValueOrigin { Source = ValueOriginSources.Publication, Method = ValueOriginDeterminationMethods.Assumption });
      _buildingBlock.Add(pv2);

      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path3", "Container3", "Parameter3"),
         Value = 300.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllValueOriginsFrom(_buildingBlock, "Path1|Container1|Parameter1", "Path2|Container2|Parameter2");
   }

   [Observation]
   public void should_return_value_origins_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeEqualTo(new ValueOrigin { Source = ValueOriginSources.Database, Method = ValueOriginDeterminationMethods.InVitro }.Display);
      _result[1].ShouldBeEqualTo(new ValueOrigin { Source = ValueOriginSources.Publication, Method = ValueOriginDeterminationMethods.Assumption }.Display);
   }
}

internal class When_getting_all_value_origins_from_building_block_without_specified_paths : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path1", "Container1", "Parameter1"),
         Value = 100.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Path2", "Container2", "Parameter2"),
         Value = 200.0,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllValueOriginsFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_all_value_origins()
   {
      _result.Length.ShouldBeEqualTo(2);
   }
}

internal class When_exporting_parameter_values_to_pkml : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private string _filePath;

   protected override void Context()
   {
      base.Context();
      _filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pkml");
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Export Test");
      _buildingBlock.Add(new ParameterValue { Path = new ObjectPath("Root", "Container", "Parameter"), Value = 42.0 });
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

internal class When_adding_protein_expression_parameters_for_specified_organs : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private string[] _result;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });

      var topContainer = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organism).WithName("Organism");
      var liver = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Liver");
      liver.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      var kidney = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Kidney");
      kidney.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      topContainer.Add(liver);
      topContainer.Add(kidney);
      _spatialStructure.AddTopContainer(topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddProteinExpressionParameters(_buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure, "Organism|Liver");
   }

   [Observation]
   public void should_return_path_for_specified_organ()
   {
      _result.ShouldContain("Organism|Liver|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_not_return_path_for_unspecified_organ()
   {
      _result.ShouldNotContain("Organism|Kidney|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_add_parameter_value_to_building_block_for_specified_organ()
   {
      _buildingBlock.FindByPath("Organism|Liver|Intracellular|Protein1|RelExp").ShouldNotBeNull();
   }

   [Observation]
   public void should_not_add_parameter_value_to_building_block_for_unspecified_organ()
   {
      _buildingBlock.FindByPath("Organism|Kidney|Intracellular|Protein1|RelExp").ShouldBeNull();
   }
}

internal class When_adding_protein_expression_parameters_with_no_organ_paths : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private string[] _result;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });

      var topContainer = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organism).WithName("Organism");
      var liver = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Liver");
      liver.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      var kidney = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Kidney");
      kidney.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      topContainer.Add(liver);
      topContainer.Add(kidney);
      _spatialStructure.AddTopContainer(topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddProteinExpressionParameters(_buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure);
   }

   [Observation]
   public void should_return_paths_for_all_organs()
   {
      _result.ShouldContain("Organism|Liver|Intracellular|Protein1|RelExp");
      _result.ShouldContain("Organism|Kidney|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_add_parameter_values_to_building_block_for_all_organs()
   {
      _buildingBlock.FindByPath("Organism|Liver|Intracellular|Protein1|RelExp").ShouldNotBeNull();
      _buildingBlock.FindByPath("Organism|Kidney|Intracellular|Protein1|RelExp").ShouldNotBeNull();
   }
}

internal class When_adding_protein_expression_parameters_with_pre_existing_paths : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private string[] _result;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });

      var topContainer = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organism).WithName("Organism");
      var liver = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Liver");
      liver.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      var kidney = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Kidney");
      kidney.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      topContainer.Add(liver);
      topContainer.Add(kidney);
      _spatialStructure.AddTopContainer(topContainer);

      // Pre-populate with Liver entry
      _buildingBlock.Add(new ParameterValue
      {
         Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"),
         Value = 99.0
      });

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddProteinExpressionParameters(_buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure);
   }

   [Observation]
   public void should_not_return_pre_existing_paths()
   {
      _result.ShouldNotContain("Organism|Liver|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_return_only_newly_added_paths()
   {
      _result.ShouldContain("Organism|Kidney|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_add_new_parameter_value_to_building_block()
   {
      _buildingBlock.FindByPath("Organism|Kidney|Intracellular|Protein1|RelExp").ShouldNotBeNull();
   }

   [Observation]
   public void should_retain_pre_existing_parameter_value_in_building_block()
   {
      _buildingBlock.FindByPath("Organism|Liver|Intracellular|Protein1|RelExp").ShouldNotBeNull();
   }
}

internal class When_adding_protein_expression_parameters_with_mismatched_molecule_name : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoBiSpatialStructure _spatialStructure;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();
      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "ProteinA|Human|Default" };
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AddProteinExpressionParameters(
         _buildingBlock, _expressionProfile, _moleculeBuildingBlock, "ProteinB", _spatialStructure
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_adding_protein_expression_parameters_with_molecule_not_in_building_block : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoBiSpatialStructure _spatialStructure;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();
      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AddProteinExpressionParameters(
         _buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_adding_protein_expression_parameters_with_organ_not_in_spatial_structure : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoBiSpatialStructure _spatialStructure;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });

      var topContainer = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organism).WithName("Organism");
      var liver = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Liver");
      liver.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      topContainer.Add(liver);
      _spatialStructure.AddTopContainer(topContainer);
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AddProteinExpressionParameters(
         _buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure, "Organism|NonExistentOrgan"
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_adding_protein_expression_parameters_with_duplicate_organ_paths : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private string[] _result;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });

      var topContainer = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organism).WithName("Organism");
      var liver = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Liver");
      liver.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      topContainer.Add(liver);
      _spatialStructure.AddTopContainer(topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddProteinExpressionParameters(_buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure, "Organism|Liver", "Organism|Liver");
   }

   [Observation]
   public void should_return_only_a_single_path()
   {
      _result.Length.ShouldBeEqualTo(1);
      _result.ShouldContain("Organism|Liver|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_add_only_a_single_parameter_value_to_building_block()
   {
      _buildingBlock.Count().ShouldBeEqualTo(1);
   }
}

internal class When_adding_protein_expression_parameters_with_multiple_expression_parameters : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private string[] _result;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(2.0),
         Name = "RelExp norm"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Intracellular", "Protein1", "RelExp norm"), Name = "RelExp norm", Value = 0.8 });

      var topContainer = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organism).WithName("Organism");
      var liver = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Liver");
      liver.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      var kidney = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Kidney");
      kidney.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      topContainer.Add(liver);
      topContainer.Add(kidney);
      _spatialStructure.AddTopContainer(topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddProteinExpressionParameters(_buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure, "Organism|Liver", "Organism|Kidney");
   }

   [Observation]
   public void should_add_both_expression_parameters_for_each_organ()
   {
      _result.ShouldContain("Organism|Liver|Intracellular|Protein1|RelExp");
      _result.ShouldContain("Organism|Liver|Intracellular|Protein1|RelExp norm");
      _result.ShouldContain("Organism|Kidney|Intracellular|Protein1|RelExp");
      _result.ShouldContain("Organism|Kidney|Intracellular|Protein1|RelExp norm");
   }

   [Observation]
   public void should_add_all_parameter_values_to_building_block()
   {
      _buildingBlock.Count().ShouldBeEqualTo(4);
      _buildingBlock.FindByPath("Organism|Liver|Intracellular|Protein1|RelExp").ShouldNotBeNull();
      _buildingBlock.FindByPath("Organism|Liver|Intracellular|Protein1|RelExp norm").ShouldNotBeNull();
      _buildingBlock.FindByPath("Organism|Kidney|Intracellular|Protein1|RelExp").ShouldNotBeNull();
      _buildingBlock.FindByPath("Organism|Kidney|Intracellular|Protein1|RelExp norm").ShouldNotBeNull();
   }
}

internal class When_adding_protein_expression_parameters_for_top_container_with_parent_path : concern_for_ParameterValuesTask_with_project
{
   private ParameterValuesBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private ExpressionProfileBuildingBlock _expressionProfile;
   private string[] _result;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new ParameterValuesBuildingBlock().WithName("PV Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule = new MoleculeBuilder().WithName("Protein1").WithDimension(Constants.Dimension.NO_DIMENSION);
      molecule.AddParameter(new Parameter
      {
         BuildMode = ParameterBuildMode.Local,
         Formula = new ConstantFormula(1.0),
         Name = "RelExp"
      });
      _moleculeBuildingBlock.Add(molecule);

      _expressionProfile = new ExpressionProfileBuildingBlock { Name = "Protein1|Human|Default" };
      _expressionProfile.Add(new ExpressionParameter { Path = new ObjectPath("Organism", "Liver", "Tumor", "Intracellular", "Protein1", "RelExp"), Name = "RelExp", Value = 0.5 });

      var tumor = new Container().WithMode(ContainerMode.Logical).WithContainerType(ContainerType.Organ).WithName("Tumor");
      tumor.ParentPath = new ObjectPath("Organism", "Liver");
      tumor.Add(new Container().WithMode(ContainerMode.Physical).WithContainerType(ContainerType.Compartment).WithName("Intracellular"));
      _spatialStructure.AddTopContainer(tumor);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.AddProteinExpressionParameters(_buildingBlock, _expressionProfile, _moleculeBuildingBlock, "Protein1", _spatialStructure, "Organism|Liver|Tumor");
   }

   [Observation]
   public void should_resolve_the_top_container_using_its_parent_path()
   {
      _result.ShouldContain("Organism|Liver|Tumor|Intracellular|Protein1|RelExp");
   }

   [Observation]
   public void should_add_the_parameter_value_to_the_building_block()
   {
      _buildingBlock.FindByPath("Organism|Liver|Tumor|Intracellular|Protein1|RelExp").ShouldNotBeNull();
   }
}

internal class When_loading_parameter_values_from_pkml : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _result;

   protected override void Because()
   {
      _result = sut.LoadFromPKML(HelperForSpecs.DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void should_return_the_parameter_values_building_block_from_the_file()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("P2");
   }
}

internal class When_loading_parameter_values_from_pkml_that_contains_multiple : concern_for_ParameterValuesTask
{
   private string _tempFile;

   protected override void Context()
   {
      base.Context();
      _tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".pkml");
      var xmlSerializer = OSPSuite.R.Api.Container.Resolve<MoBi.Core.Serialization.Xml.Services.IXmlSerializationService>();
      var first = xmlSerializer.SerializeModelPart(new ParameterValuesBuildingBlock().WithName("First"));
      var second = xmlSerializer.SerializeModelPart(new ParameterValuesBuildingBlock().WithName("Second"));

      var root = new XElement("ParameterValuesContainer", first, second);
      root.PermissiveSave(_tempFile);
   }

   [Observation]
   public void should_throw_mobi_exception()
   {
      The.Action(() => sut.LoadFromPKML(_tempFile)).ShouldThrowAn<MoBi.Core.Exceptions.MoBiException>();
   }

   public override void Cleanup()
   {
      base.Cleanup();
      File.Delete(_tempFile);
   }
}

internal class When_creating_an_empty_parameter_values_building_block : concern_for_ParameterValuesTask
{
   private ParameterValuesBuildingBlock _result;

   protected override void Because()
   {
      _result = sut.CreateBuildingBlock("My PV BB");
   }

   [Observation]
   public void should_return_a_named_empty_building_block()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("My PV BB");
      _result.Count().ShouldBeEqualTo(0);
   }
}