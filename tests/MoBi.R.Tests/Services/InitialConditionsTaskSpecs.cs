using System;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.R.Tests.Services;

internal class concern_for_InitialConditionsTask : ContextForIntegration<IInitialConditionsTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetInitialConditionsTask();
   }
}

internal class concern_for_InitialConditionsTask_with_project : concern_for_InitialConditionsTask
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

internal class When_removing_initial_conditions_from_a_building_block_in_a_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _initialCondition1;
   private InitialCondition _initialCondition2;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _initialCondition1 = new InitialCondition
      {
         Path = new ObjectPath("IC1", "Path1", "MOLECULE")
      };
      _initialCondition2 = new InitialCondition
      {
         Path = new ObjectPath("IC2", "Path2", "MOLECULE")
      };
      _buildingBlock.Add(_initialCondition1);
      _buildingBlock.Add(_initialCondition2);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.DeleteInitialConditions(_buildingBlock, ["IC1|Path1|MOLECULE", "IC2|Path2|MOLECULE"]);
   }

   [Observation]
   public void commands_were_used_to_remove_them()
   {
      var command = _context.HistoryManager.History.Last().Command;
      command.ShouldBeAnInstanceOf<MoBiMacroCommand>();
      var macroCommand = command as MoBiMacroCommand;
      macroCommand.All().Count(c => c is RemovePathAndValueEntityFromBuildingBlockCommand<InitialCondition>).ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_remove_the_initial_condition_from_the_building_block()
   {
      _buildingBlock.ShouldNotContain(_initialCondition1);
      _buildingBlock.ShouldNotContain(_initialCondition2);
   }
}

internal class When_removing_initial_conditions_from_a_building_block_not_in_a_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _initialCondition1;
   private InitialCondition _initialCondition2;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _initialCondition1 = new InitialCondition
      {
         Path = new ObjectPath("IC1", "Path1", "MOLECULE")
      };
      _initialCondition2 = new InitialCondition
      {
         Path = new ObjectPath("IC2", "Path2", "MOLECULE")
      };
      _buildingBlock.Add(_initialCondition1);
      _buildingBlock.Add(_initialCondition2);
   }

   protected override void Because()
   {
      sut.DeleteInitialConditions(_buildingBlock, ["IC1|Path1|MOLECULE", "IC2|Path2|MOLECULE"]);
   }

   [Observation]
   public void should_remove_the_initial_condition_from_the_building_block()
   {
      _buildingBlock.ShouldNotContain(_initialCondition1);
      _buildingBlock.ShouldNotContain(_initialCondition2);
   }
}

internal class When_extending_initial_conditions_with_all_molecules_for_building_block_in_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);

      _topContainer = new Container().WithMode(ContainerMode.Logical).WithName("TopContainer");
      _spatialStructure.AddTopContainer(_topContainer);
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
   }

   [Observation]
   public void should_add_initial_conditions_for_all_molecules()
   {
      _buildingBlock.Count().ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule1()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule1").ShouldBeTrue();
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule2()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule2").ShouldBeTrue();
   }

   [Observation]
   public void commands_were_used_to_add_them()
   {
      var command = _context.HistoryManager.History.Last().Command;
      command.ShouldBeAnInstanceOf<IMacroCommand>();
      var macroCommand = command as IMacroCommand;

      macroCommand.All().SelectMany(x => (x as IMacroCommand).All()).Count(c => c is AddInitialConditionToBuildingBlockCommand).ShouldBeEqualTo(2);
   }
}

internal class When_extending_initial_conditions_with_all_molecules_for_building_block_not_in_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IContainer _topContainer;
   private int _initialCount;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      _initialCount = _buildingBlock.Count();
   }

   protected override void Because()
   {
      sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
   }

   [Observation]
   public void should_add_initial_conditions_for_all_molecules()
   {
      _buildingBlock.Count().ShouldBeGreaterThan(_initialCount);
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule1()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule1").ShouldBeTrue();
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule2()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule2").ShouldBeTrue();
   }
}

internal class When_extending_initial_conditions_with_specific_molecules_for_building_block_in_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoleculeBuilder _molecule3;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule3 = new MoleculeBuilder().WithName("Molecule3").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);
      _moleculeBuildingBlock.Add(_molecule3);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, ["Molecule1", "Molecule3"]);
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule1()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule1").ShouldBeTrue();
   }

   [Observation]
   public void should_not_create_initial_conditions_for_molecule2()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule2").ShouldBeFalse();
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule3()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule3").ShouldBeTrue();
   }
}

internal class When_extending_initial_conditions_with_specific_molecules_for_building_block_not_in_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoleculeBuilder _molecule3;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule3 = new MoleculeBuilder().WithName("Molecule3").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);
      _moleculeBuildingBlock.Add(_molecule3);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);
   }

   protected override void Because()
   {
      sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, ["Molecule2"]);
   }

   [Observation]
   public void should_not_create_initial_conditions_for_molecule1()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule1").ShouldBeFalse();
   }

   [Observation]
   public void should_create_initial_conditions_for_molecule2()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule2").ShouldBeTrue();
   }

   [Observation]
   public void should_not_create_initial_conditions_for_molecule3()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule3").ShouldBeFalse();
   }
}

internal class When_extending_initial_conditions_with_empty_molecule_names_array : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IContainer _topContainer;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      _molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(_molecule1);
      _moleculeBuildingBlock.Add(_molecule2);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, []);
   }

   [Observation]
   public void should_treat_empty_array_as_all_molecules_and_create_initial_conditions_for_molecule1()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule1").ShouldBeTrue();
   }

   [Observation]
   public void should_treat_empty_array_as_all_molecules_and_create_initial_conditions_for_molecule2()
   {
      _buildingBlock.Any(ic => ic.MoleculeName == "Molecule2").ShouldBeTrue();
   }
}

internal class When_extending_initial_conditions_returns_only_added_paths_for_building_block_in_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private IContainer _topContainer;
   private string[] _result;
   private string _preExistingPath;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      var molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(molecule1);
      _moleculeBuildingBlock.Add(molecule2);

      _topContainer = new Container().WithMode(ContainerMode.Logical).WithName("TopContainer");
      _spatialStructure.AddTopContainer(_topContainer);
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));

      // Pre-populate the building block with one initial condition that will also be generated by extend
      _preExistingPath = "TopContainer|physical|Molecule1";
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "physical", "Molecule1"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      AddBuildingBlocksToProject(_buildingBlock, _moleculeBuildingBlock, _spatialStructure);
   }

   protected override void Because()
   {
      _result = sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
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
      _result.ShouldContain("TopContainer|physical|Molecule2");
   }
}

internal class When_extending_initial_conditions_returns_only_added_paths_for_building_block_not_in_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private IContainer _topContainer;
   private string[] _result;
   private string _preExistingPath;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _spatialStructure = new MoBiSpatialStructure();
      _moleculeBuildingBlock = new MoleculeBuildingBlock();

      var molecule1 = new MoleculeBuilder().WithName("Molecule1").WithDimension(Constants.Dimension.NO_DIMENSION);
      var molecule2 = new MoleculeBuilder().WithName("Molecule2").WithDimension(Constants.Dimension.NO_DIMENSION);
      _moleculeBuildingBlock.Add(molecule1);
      _moleculeBuildingBlock.Add(molecule2);

      _topContainer = new Container().WithName("TopContainer");
      _topContainer.Add(new Container().WithMode(ContainerMode.Physical).WithName("physical"));
      _spatialStructure.AddTopContainer(_topContainer);

      // Pre-populate the building block with one initial condition that will also be generated by extend
      _preExistingPath = "TopContainer|physical|Molecule1";
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "physical", "Molecule1"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.ExtendInitialConditions(_buildingBlock, _spatialStructure, _moleculeBuildingBlock, null);
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
      _result.ShouldContain("TopContainer|physical|Molecule2");
   }
}

internal class When_setting_initial_conditions_with_inconsistent_array_lengths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["Path1|Container1|Molecule1", "Path2|Container2|Molecule2"],
         dimensionNames: ["Amount"], // Only 1 element - inconsistent!
         quantityValues: [100, 200],
         scaleDivisors: [1, 1],
         isPresent: [true, true],
         negativeAllowed: [false, false]
      )).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_setting_initial_conditions_that_all_already_exist_with_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _initialCondition1;
   private InitialCondition _initialCondition2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");

      // Create initial conditions first
      _initialCondition1 = new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "Physical", "Molecule1"),
         Value = 50,
         IsPresent = true
      };

      _initialCondition2 = new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "Physical", "Molecule2"),
         Value = 75,
         IsPresent = true
      };

      _buildingBlock.Add(_initialCondition1);
      _buildingBlock.Add(_initialCondition2);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Molecule1", "TopContainer|Physical|Molecule2"],
         dimensionNames: ["Amount", "Amount"],
         quantityValues: [100, 200],
         scaleDivisors: [1.5, 10.5],
         isPresent: [true, false],
         negativeAllowed: [false, true]
      );
   }

   [Observation]
   public void should_update_the_value_of_first_initial_condition()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_update_the_value_of_second_initial_condition()
   {
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic2.Value.ShouldBeEqualTo(200);
   }

   [Observation]
   public void should_update_is_present_flag()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.IsPresent.ShouldBeTrue();
      ic2.IsPresent.ShouldBeFalse();
   }

   [Observation]
   public void should_update_scale_divisor()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.ScaleDivisor.ShouldBeEqualTo(1.5);
      ic2.ScaleDivisor.ShouldBeEqualTo(10.5);
   }

   [Observation]
   public void should_update_negative_allowed_flag()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.NegativeValuesAllowed.ShouldBeFalse();
      ic2.NegativeValuesAllowed.ShouldBeTrue();
   }
}

internal class When_setting_initial_conditions_that_dont_exist_with_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Molecule1", "TopContainer|Physical|Molecule2"],
         dimensionNames: ["Amount", "Amount"],
         quantityValues: [150, 250],
         scaleDivisors: [1.5, 10.5],
         isPresent: [true, true],
         negativeAllowed: [false, false]
      );
   }

   [Observation]
   public void should_add_new_initial_conditions_to_building_block()
   {
      _buildingBlock.Count().ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_update_scale_divisor()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.ScaleDivisor.ShouldBeEqualTo(1.5);
      ic2.ScaleDivisor.ShouldBeEqualTo(10.5);
   }

   [Observation]
   public void should_create_initial_condition_for_molecule1_with_correct_value()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.ShouldNotBeNull();
      ic1.Value.ShouldBeEqualTo(150);
   }

   [Observation]
   public void should_create_initial_condition_for_molecule2_with_correct_value()
   {
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic2.ShouldNotBeNull();
      ic2.Value.ShouldBeEqualTo(250);
   }

   [Observation]
   public void should_set_is_present_flags()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.IsPresent.ShouldBeTrue();
      ic2.IsPresent.ShouldBeTrue();
   }
}

internal class When_setting_initial_conditions_with_mix_of_existing_and_new_with_project : concern_for_InitialConditionsTask_with_project
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _existingCondition;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");

      var topContainer = new Container().WithName("TopContainer");
      var physicalContainer = new Container().WithMode(ContainerMode.Physical).WithName("Physical");
      topContainer.Add(physicalContainer);

      // Add one existing initial condition
      _existingCondition = new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "Physical", "Molecule1"),
         Value = 50,
         IsPresent = false,
         NegativeValuesAllowed = false
      };
      _buildingBlock.Add(_existingCondition);

      AddBuildingBlocksToProject(_buildingBlock);
   }

   protected override void Because()
   {
      sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Molecule1", "TopContainer|Physical|Molecule2", "TopContainer|Physical|Molecule3"],
         dimensionNames: ["Amount", "Amount", "Amount"],
         quantityValues: [100, 200, 300],
         scaleDivisors: [1.0, 2.0, 3.0],
         isPresent: [true, true, false],
         negativeAllowed: [true, false, true]
      );
   }

   [Observation]
   public void should_update_existing_initial_condition_value()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_update_existing_initial_condition_flags()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.IsPresent.ShouldBeTrue();
      ic1.NegativeValuesAllowed.ShouldBeTrue();
      ic1.ScaleDivisor.ShouldBeEqualTo(1.0);
   }

   [Observation]
   public void should_add_new_initial_condition_for_molecule2()
   {
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic2.ShouldNotBeNull();
      ic2.Value.ShouldBeEqualTo(200);
      ic2.IsPresent.ShouldBeTrue();
      ic2.NegativeValuesAllowed.ShouldBeFalse();
      ic2.ScaleDivisor.ShouldBeEqualTo(2.0);
   }

   [Observation]
   public void should_add_new_initial_condition_for_molecule3()
   {
      var ic3 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule3");
      ic3.ShouldNotBeNull();
      ic3.Value.ShouldBeEqualTo(300);
      ic3.IsPresent.ShouldBeFalse();
      ic3.NegativeValuesAllowed.ShouldBeTrue();
      ic3.ScaleDivisor.ShouldBeEqualTo(3.0);
   }

   [Observation]
   public void should_increase_building_block_count()
   {
      _buildingBlock.Count().ShouldBeEqualTo(3);
   }
}

internal class When_setting_initial_conditions_that_all_already_exist : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _initialCondition1;
   private InitialCondition _initialCondition2;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");

      // Create initial conditions first
      _initialCondition1 = new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "Physical", "Molecule1"),
         Value = 50,
         IsPresent = true
      };

      _initialCondition2 = new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "Physical", "Molecule2"),
         Value = 75,
         IsPresent = true
      };

      _buildingBlock.Add(_initialCondition1);
      _buildingBlock.Add(_initialCondition2);
   }

   protected override void Because()
   {
      sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Molecule1", "TopContainer|Physical|Molecule2"],
         dimensionNames: ["Amount", "Amount"],
         quantityValues: [100, 200],
         scaleDivisors: [1.5, 10.5],
         isPresent: [true, false],
         negativeAllowed: [false, true]
      );
   }

   [Observation]
   public void should_update_scale_divisor()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.ScaleDivisor.ShouldBeEqualTo(1.5);
      ic2.ScaleDivisor.ShouldBeEqualTo(10.5);
   }

   [Observation]
   public void should_update_the_value_of_first_initial_condition()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_update_the_value_of_second_initial_condition()
   {
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic2.Value.ShouldBeEqualTo(200);
   }

   [Observation]
   public void should_update_is_present_flag()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.IsPresent.ShouldBeTrue();
      ic2.IsPresent.ShouldBeFalse();
   }

   [Observation]
   public void should_update_negative_allowed_flag()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.NegativeValuesAllowed.ShouldBeFalse();
      ic2.NegativeValuesAllowed.ShouldBeTrue();
   }
}

internal class When_setting_initial_conditions_that_dont_exist : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
   }

   protected override void Because()
   {
      sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Molecule1", "TopContainer|Physical|Molecule2"],
         dimensionNames: ["Amount", "Amount"],
         quantityValues: [150, 250],
         scaleDivisors: [1, 5],
         isPresent: [true, true],
         negativeAllowed: [false, false]
      );
   }

   [Observation]
   public void should_add_new_initial_conditions_to_building_block()
   {
      _buildingBlock.Count().ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_create_initial_condition_for_molecule1_with_correct_value()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.ShouldNotBeNull();
      ic1.Value.ShouldBeEqualTo(150);
      ic1.ScaleDivisor.ShouldBeEqualTo(1.0);
   }

   [Observation]
   public void should_create_initial_condition_for_molecule2_with_correct_value()
   {
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic2.ShouldNotBeNull();
      ic2.Value.ShouldBeEqualTo(250);
      ic2.ScaleDivisor.ShouldBeEqualTo(5.0);
   }

   [Observation]
   public void should_set_is_present_flags()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic1.IsPresent.ShouldBeTrue();
      ic2.IsPresent.ShouldBeTrue();
   }
}

internal class When_setting_initial_conditions_with_mix_of_existing_and_new : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _existingCondition;

   protected override void Context()
   {
      base.Context();

      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");

      var topContainer = new Container().WithName("TopContainer");
      var physicalContainer = new Container().WithMode(ContainerMode.Physical).WithName("Physical");
      topContainer.Add(physicalContainer);

      // Add one existing initial condition
      _existingCondition = new InitialCondition
      {
         Path = new ObjectPath("TopContainer", "Physical", "Molecule1"),
         Value = 50,
         IsPresent = false,
         NegativeValuesAllowed = false
      };
      _buildingBlock.Add(_existingCondition);
   }

   protected override void Because()
   {
      sut.SetInitialConditions(
         _buildingBlock,
         quantityPaths: ["TopContainer|Physical|Molecule1", "TopContainer|Physical|Molecule2", "TopContainer|Physical|Molecule3"],
         dimensionNames: ["Amount", "Amount", "Amount"],
         quantityValues: [100, 200, 300],
         scaleDivisors: [1, 1, 1],
         isPresent: [true, true, false],
         negativeAllowed: [true, false, true]
      );
   }

   [Observation]
   public void should_update_existing_initial_condition_value()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.Value.ShouldBeEqualTo(100);
   }

   [Observation]
   public void should_update_existing_initial_condition_flags()
   {
      var ic1 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule1");
      ic1.IsPresent.ShouldBeTrue();
      ic1.NegativeValuesAllowed.ShouldBeTrue();
      ic1.ScaleDivisor.ShouldBeEqualTo(1.0);
   }

   [Observation]
   public void should_add_new_initial_condition_for_molecule2()
   {
      var ic2 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule2");
      ic2.ShouldNotBeNull();
      ic2.Value.ShouldBeEqualTo(200);
      ic2.IsPresent.ShouldBeTrue();
      ic2.NegativeValuesAllowed.ShouldBeFalse();
      ic2.ScaleDivisor.ShouldBeEqualTo(1.0);
   }

   [Observation]
   public void should_add_new_initial_condition_for_molecule3()
   {
      var ic3 = _buildingBlock.FindByPath("TopContainer|Physical|Molecule3");
      ic3.ShouldNotBeNull();
      ic3.Value.ShouldBeEqualTo(300);
      ic3.IsPresent.ShouldBeFalse();
      ic3.NegativeValuesAllowed.ShouldBeTrue();
      ic3.ScaleDivisor.ShouldBeEqualTo(1.0);
   }

   [Observation]
   public void should_increase_building_block_count()
   {
      _buildingBlock.Count().ShouldBeEqualTo(3);
   }
}

internal class When_getting_all_molecule_names_from_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllMoleculeNamesFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_all_molecule_names()
   {
      _result.Length.ShouldBeEqualTo(3);
      _result.ShouldOnlyContainInOrder("Molecule1", "Molecule2", "Molecule3");
   }
}

internal class When_getting_molecule_names_for_specified_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllMoleculeNamesFrom(_buildingBlock, ["Path1|Container1|Molecule1", "Path3|Container3|Molecule3"]);
   }

   [Observation]
   public void should_return_molecule_names_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeEqualTo("Molecule1");
      _result[1].ShouldBeEqualTo("Molecule3");
   }
}

internal class When_getting_molecule_names_with_duplicate_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllMoleculeNamesFrom(_buildingBlock, ["Path1|Container1|Molecule1", "Path1|Container1|Molecule1"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_molecule_names_with_non_existent_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllMoleculeNamesFrom(_buildingBlock, ["Path1|Container1|Molecule1", "NonExistent|Path"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_scale_divisors_from_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private double[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         ScaleDivisor = 1.5,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         ScaleDivisor = 2.5,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         ScaleDivisor = 3.75,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllScaleDivisorsFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_all_scale_divisors()
   {
      _result.Length.ShouldBeEqualTo(3);
      _result.ShouldContain(1.5);
      _result.ShouldContain(2.5);
      _result.ShouldContain(3.75);
   }
}

internal class When_getting_scale_divisors_for_specified_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private double[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         ScaleDivisor = 1.5,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         ScaleDivisor = 2.5,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         ScaleDivisor = 3.75,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllScaleDivisorsFrom(_buildingBlock, ["Path2|Container2|Molecule2", "Path3|Container3|Molecule3"]);
   }

   [Observation]
   public void should_return_scale_divisors_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeEqualTo(2.5);
      _result[1].ShouldBeEqualTo(3.75);
   }
}

internal class When_getting_scale_divisors_with_duplicate_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         ScaleDivisor = 1.5,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllScaleDivisorsFrom(_buildingBlock, ["Path1|Container1|Molecule1", "Path1|Container1|Molecule1"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_scale_divisors_with_non_existent_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         ScaleDivisor = 1.5,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllScaleDivisorsFrom(_buildingBlock, ["Path1|Container1|Molecule1", "NonExistent|Path"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_is_present_flags_from_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private bool[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         IsPresent = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         IsPresent = false,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         IsPresent = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllIsPresentFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_all_is_present_flags()
   {
      _result.Length.ShouldBeEqualTo(3);
      _result.Count(x => x).ShouldBeEqualTo(2);
      _result.Count(x => !x).ShouldBeEqualTo(1);
   }
}

internal class When_getting_is_present_flags_for_specified_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private bool[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         IsPresent = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         IsPresent = false,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         IsPresent = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllIsPresentFrom(_buildingBlock, ["Path1|Container1|Molecule1", "Path2|Container2|Molecule2"]);
   }

   [Observation]
   public void should_return_is_present_flags_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeTrue();
      _result[1].ShouldBeFalse();
   }
}

internal class When_getting_is_present_flags_with_duplicate_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         IsPresent = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllIsPresentFrom(_buildingBlock, ["Path1|Container1|Molecule1", "Path1|Container1|Molecule1"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_is_present_flags_with_non_existent_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         IsPresent = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllIsPresentFrom(_buildingBlock, ["Path1|Container1|Molecule1", "NonExistent|Path"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_all_negative_values_allowed_flags_from_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private bool[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         NegativeValuesAllowed = false,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         NegativeValuesAllowed = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         NegativeValuesAllowed = false,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllNegativeValuesAllowedFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_all_negative_values_allowed_flags()
   {
      _result.Length.ShouldBeEqualTo(3);
      _result.Count(x => x).ShouldBeEqualTo(1);
      _result.Count(x => !x).ShouldBeEqualTo(2);
   }
}

internal class When_getting_negative_values_allowed_flags_for_specified_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private bool[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         NegativeValuesAllowed = false,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path2", "Container2", "Molecule2"),
         NegativeValuesAllowed = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });

      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path3", "Container3", "Molecule3"),
         NegativeValuesAllowed = false,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   protected override void Because()
   {
      _result = sut.AllNegativeValuesAllowedFrom(_buildingBlock, ["Path2|Container2|Molecule2", "Path3|Container3|Molecule3"]);
   }

   [Observation]
   public void should_return_negative_values_allowed_flags_for_specified_paths_in_order()
   {
      _result.Length.ShouldBeEqualTo(2);
      _result[0].ShouldBeTrue();
      _result[1].ShouldBeFalse();
   }
}

internal class When_getting_negative_values_allowed_flags_with_duplicate_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         NegativeValuesAllowed = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllNegativeValuesAllowedFrom(_buildingBlock, ["Path1|Container1|Molecule1", "Path1|Container1|Molecule1"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_negative_values_allowed_flags_with_non_existent_paths : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
      _buildingBlock.Add(new InitialCondition
      {
         Path = new ObjectPath("Path1", "Container1", "Molecule1"),
         NegativeValuesAllowed = true,
         Dimension = Constants.Dimension.NO_DIMENSION
      });
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.AllNegativeValuesAllowedFrom(_buildingBlock, ["Path1|Container1|Molecule1", "NonExistent|Path"]))
         .ShouldThrowAn<ArgumentException>();
   }
}

internal class When_getting_molecule_names_from_empty_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
   }

   protected override void Because()
   {
      _result = sut.AllMoleculeNamesFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_empty_array()
   {
      _result.ShouldBeEmpty();
   }
}

internal class When_getting_scale_divisors_from_empty_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private double[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
   }

   protected override void Because()
   {
      _result = sut.AllScaleDivisorsFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_empty_array()
   {
      _result.ShouldBeEmpty();
   }
}

internal class When_getting_is_present_flags_from_empty_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private bool[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
   }

   protected override void Because()
   {
      _result = sut.AllIsPresentFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_empty_array()
   {
      _result.ShouldBeEmpty();
   }
}

internal class When_getting_negative_values_allowed_flags_from_empty_building_block : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private bool[] _result;

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new InitialConditionsBuildingBlock().WithName("IC Building Block");
   }

   protected override void Because()
   {
      _result = sut.AllNegativeValuesAllowedFrom(_buildingBlock, null);
   }

   [Observation]
   public void should_return_empty_array()
   {
      _result.ShouldBeEmpty();
   }
}