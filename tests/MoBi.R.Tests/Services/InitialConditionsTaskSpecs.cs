using System.Linq;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Services;

internal class concern_for_InitialConditionsTask : ContextForIntegration<IInitialConditionsTask>
{
   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetInitialConditionsTask();
   }
}

internal class When_removing_initial_conditions_from_a_building_block_in_a_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private InitialCondition _initialCondition1;
   private InitialCondition _initialCondition2;
   private MoBiProject _project;
   private IMoBiContext _context;

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

      var projectFile = HelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
      _project = Api.GetProjectTask().LoadProject(projectFile);

      var module = new Module { _buildingBlock };
      module.IsPKSimModule = true;
      _project.AddModule(module);

      _context = OSPSuite.R.Api.Container.Resolve<IMoBiContext>();
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

internal class When_extending_initial_conditions_with_all_molecules_for_building_block_in_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoBiProject _project;
   private IContainer _topContainer;
   private IMoBiContext _context;

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

      var projectFile = HelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
      _project = Api.GetProjectTask().LoadProject(projectFile);

      var module = new Module { _buildingBlock, _spatialStructure, _moleculeBuildingBlock };
      _project.AddModule(module);

      _context = OSPSuite.R.Api.Container.Resolve<IMoBiContext>();
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
   public void commands_were_used_to_remove_them()
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

internal class When_extending_initial_conditions_with_specific_molecules_for_building_block_in_project : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoleculeBuilder _molecule3;
   private MoBiProject _project;
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

      var projectFile = HelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
      _project = Api.GetProjectTask().LoadProject(projectFile);

      var module = new Module { _buildingBlock, _spatialStructure, _moleculeBuildingBlock };
      _project.AddModule(module);
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

internal class When_extending_initial_conditions_with_empty_molecule_names_array : concern_for_InitialConditionsTask
{
   private InitialConditionsBuildingBlock _buildingBlock;
   private MoBiSpatialStructure _spatialStructure;
   private MoleculeBuildingBlock _moleculeBuildingBlock;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private MoBiProject _project;
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

      var projectFile = HelperForSpecs.DataTestFileFullPath("SampleProject.mbp3");
      _project = Api.GetProjectTask().LoadProject(projectFile);

      var module = new Module { _buildingBlock, _spatialStructure, _moleculeBuildingBlock };
      _project.AddModule(module);
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