using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper : ContextSpecification<SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper>
   {
      protected override void Context()
      {
         sut = new SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper();
      }

      protected MoleculeBuilder CreateFloatingMolecule(string name, params (string category, string method)[] calculationMethods)
      {
         var molecule = new MoleculeBuilder().WithName(name);
         molecule.IsFloating = true;
         foreach (var (category, method) in calculationMethods)
            molecule.AddUsedCalculationMethod(new UsedCalculationMethod(category, method));
         return molecule;
      }

      protected MoleculeBuilder CreateStationaryMolecule(string name, params (string category, string method)[] calculationMethods)
      {
         var molecule = new MoleculeBuilder().WithName(name);
         molecule.IsFloating = false;
         foreach (var (category, method) in calculationMethods)
            molecule.AddUsedCalculationMethod(new UsedCalculationMethod(category, method));
         return molecule;
      }

      protected SimulationConfiguration CreateSimulationConfiguration(IReadOnlyList<MoleculeBuilder> molecules)
      {
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         foreach (var molecule in molecules)
            moleculeBuildingBlock.Add(molecule);
         var module = new Module { moleculeBuildingBlock };
         var simulationConfiguration = new SimulationConfiguration();
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));
         return simulationConfiguration;
      }
   }

   internal class When_mapping_a_simulation_configuration_with_molecule_calculation_method_overrides : concern_for_SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         var drug = CreateFloatingMolecule("Drug", ("Partition", "MethodA"));
         _simulationConfiguration = CreateSimulationConfiguration([drug]);
         _simulationConfiguration.AddCalculationMethodsOverridesFor("Drug", new List<UsedCalculationMethod>
         {
            new("Partition", "OverriddenMethod")
         });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simulationConfiguration);
      }

      [Observation]
      public void should_return_the_overridden_calculation_methods()
      {
         _result.Count.ShouldBeEqualTo(1);
         _result[0].MoleculeName.ShouldBeEqualTo("Drug");
         _result[0].UsedCalculationMethods[0].Category.ShouldBeEqualTo("Partition");
         _result[0].UsedCalculationMethods[0].CalculationMethodName.ShouldBeEqualTo("OverriddenMethod");
      }
   }

   internal class When_mapping_a_simulation_configuration_without_overrides : concern_for_SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         var drug = CreateFloatingMolecule("Drug", ("Partition", "MethodA"));
         _simulationConfiguration = CreateSimulationConfiguration([drug]);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simulationConfiguration);
      }

      [Observation]
      public void should_not_include_molecules_without_overrides()
      {
         _result.ShouldBeEmpty();
      }
   }

   internal class When_mapping_a_simulation_configuration_with_stationary_molecules : concern_for_SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         var stationary = CreateStationaryMolecule("StationaryMol", ("Partition", "MethodA"));
         _simulationConfiguration = CreateSimulationConfiguration([stationary]);
         _simulationConfiguration.AddCalculationMethodsOverridesFor("StationaryMol", new List<UsedCalculationMethod>
         {
            new("Partition", "OverriddenMethod")
         });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simulationConfiguration);
      }

      [Observation]
      public void should_not_include_stationary_molecules()
      {
         _result.ShouldBeEmpty();
      }
   }

   internal class When_mapping_a_simulation_configuration_with_multiple_modules : concern_for_SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         var drug = CreateFloatingMolecule("Drug", ("Partition", "MethodA"));
         var drug2 = CreateFloatingMolecule("Drug2", ("Distribution", "MethodB"));

         var moleculeBuildingBlock1 = new MoleculeBuildingBlock { drug };
         var moleculeBuildingBlock2 = new MoleculeBuildingBlock { drug2 };

         var module1 = new Module { moleculeBuildingBlock1 };
         var module2 = new Module { moleculeBuildingBlock2 };

         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2));

         _simulationConfiguration.AddCalculationMethodsOverridesFor("Drug", new List<UsedCalculationMethod>
         {
            new("Partition", "OverriddenA")
         });
         _simulationConfiguration.AddCalculationMethodsOverridesFor("Drug2", new List<UsedCalculationMethod>
         {
            new("Distribution", "OverriddenB")
         });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simulationConfiguration);
      }

      [Observation]
      public void should_return_dtos_for_molecules_across_all_modules()
      {
         _result.Count.ShouldBeEqualTo(2);
         _result.Select(x => x.MoleculeName).ShouldContain("Drug", "Drug2");
      }

      [Observation]
      public void should_map_overridden_calculation_methods_for_each_molecule()
      {
         var drugDTO = _result.Single(x => x.MoleculeName == "Drug");
         drugDTO.UsedCalculationMethods[0].CalculationMethodName.ShouldBeEqualTo("OverriddenA");

         var drug2DTO = _result.Single(x => x.MoleculeName == "Drug2");
         drug2DTO.UsedCalculationMethods[0].CalculationMethodName.ShouldBeEqualTo("OverriddenB");
      }
   }

   internal class When_mapping_a_simulation_configuration_with_module_without_molecule_building_block : concern_for_SimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         base.Context();
         var module = new Module();
         _simulationConfiguration = new SimulationConfiguration();
         _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simulationConfiguration);
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _result.ShouldBeEmpty();
      }
   }
}