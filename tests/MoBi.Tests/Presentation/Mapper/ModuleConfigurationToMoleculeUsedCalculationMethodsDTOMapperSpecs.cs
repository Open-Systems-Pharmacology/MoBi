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
   internal class concern_for_ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper : ContextSpecification<ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper>
   {
      protected override void Context()
      {
         sut = new ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper();
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

      protected ModuleConfiguration CreateModuleConfiguration(params MoleculeBuilder[] molecules)
      {
         var moleculeBuildingBlock = new MoleculeBuildingBlock();
         foreach (var molecule in molecules)
            moleculeBuildingBlock.Add(molecule);
         var module = new Module { moleculeBuildingBlock };
         return new ModuleConfiguration(module);
      }
   }

   internal class When_mapping_a_module_configuration_with_floating_molecules : concern_for_ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         base.Context();
         var drug = CreateFloatingMolecule("Drug", ("Partition", "MethodA"), ("Permeability", "MethodB"));
         var drug2 = CreateFloatingMolecule("Drug2", ("Distribution", "MethodC"));
         _moduleConfiguration = CreateModuleConfiguration(drug, drug2);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moduleConfiguration);
      }

      [Observation]
      public void should_return_a_dto_for_each_floating_molecule()
      {
         _result.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_map_molecule_names()
      {
         _result.Select(x => x.MoleculeName).ShouldContain("Drug", "Drug2");
      }

      [Observation]
      public void should_map_calculation_methods_for_each_molecule()
      {
         var drugDTO = _result.Single(x => x.MoleculeName == "Drug");
         drugDTO.UsedCalculationMethods.Count.ShouldBeEqualTo(2);
         drugDTO.UsedCalculationMethods[0].Category.ShouldBeEqualTo("Partition");
         drugDTO.UsedCalculationMethods[0].CalculationMethodName.ShouldBeEqualTo("MethodA");
         drugDTO.UsedCalculationMethods[1].Category.ShouldBeEqualTo("Permeability");
         drugDTO.UsedCalculationMethods[1].CalculationMethodName.ShouldBeEqualTo("MethodB");
      }
   }

   internal class When_mapping_a_module_configuration_with_stationary_molecules : concern_for_ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         base.Context();
         var stationary = CreateStationaryMolecule("StationaryMol", ("Partition", "MethodA"));
         _moduleConfiguration = CreateModuleConfiguration(stationary);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moduleConfiguration);
      }

      [Observation]
      public void should_not_include_stationary_molecules()
      {
         _result.ShouldBeEmpty();
      }
   }

   internal class When_mapping_a_module_configuration_with_floating_molecule_without_calculation_methods : concern_for_ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         base.Context();
         var molecule = CreateFloatingMolecule("Drug");
         _moduleConfiguration = CreateModuleConfiguration(molecule);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moduleConfiguration);
      }

      [Observation]
      public void should_not_include_molecules_without_calculation_methods()
      {
         _result.ShouldBeEmpty();
      }
   }

   internal class When_mapping_a_module_configuration_with_mixed_molecules : concern_for_ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         base.Context();
         var floatingWithMethods = CreateFloatingMolecule("Drug", ("Partition", "MethodA"));
         var floatingWithoutMethods = CreateFloatingMolecule("EmptyDrug");
         var stationary = CreateStationaryMolecule("StationaryMol", ("Distribution", "MethodB"));
         _moduleConfiguration = CreateModuleConfiguration(floatingWithMethods, floatingWithoutMethods, stationary);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moduleConfiguration);
      }

      [Observation]
      public void should_only_include_floating_molecules_with_calculation_methods()
      {
         _result.Count.ShouldBeEqualTo(1);
         _result[0].MoleculeName.ShouldBeEqualTo("Drug");
      }
   }

   internal class When_mapping_a_module_configuration_without_a_molecule_building_block : concern_for_ModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper
   {
      private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _result;
      private ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         base.Context();
         var module = new Module();
         _moduleConfiguration = new ModuleConfiguration(module);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moduleConfiguration);
      }

      [Observation]
      public void should_return_an_empty_list()
      {
         _result.ShouldBeEmpty();
      }
   }
}
