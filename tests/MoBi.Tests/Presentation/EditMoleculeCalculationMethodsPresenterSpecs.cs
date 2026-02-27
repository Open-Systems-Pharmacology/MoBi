using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Presentation;

internal class concern_for_EditMoleculeCalculationMethodsPresenter : ContextSpecification<EditMoleculeCalculationMethodsPresenter>
{
   protected IEditMoleculeCalculationMethodsView _view;
   protected IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderToDTOMapper;
   protected ICoreCalculationMethodRepository _calculationMethodsRepository;

   protected override void Context()
   {
      _view = A.Fake<IEditMoleculeCalculationMethodsView>();
      _moleculeBuilderToDTOMapper = A.Fake<IMoleculeBuilderToMoleculeBuilderDTOMapper>();
      _calculationMethodsRepository = A.Fake<ICoreCalculationMethodRepository>();

      sut = new EditMoleculeCalculationMethodsPresenter(_view, _moleculeBuilderToDTOMapper, _calculationMethodsRepository);
   }

   protected MoleculeBuilder CreateFloatingMolecule(string name, params (string category, string method)[] calculationMethods)
   {
      var molecule = new MoleculeBuilder().WithName(name);
      molecule.IsFloating = true;
      foreach (var (category, method) in calculationMethods)
         molecule.AddUsedCalculationMethod(new UsedCalculationMethod(category, method));
      return molecule;
   }

   protected MoleculeBuilderDTO CreateMoleculeDTO(string name, params (string category, string method)[] calculationMethods)
   {
      var molecule = CreateFloatingMolecule(name, calculationMethods);
      var dto = new MoleculeBuilderDTO(molecule).WithName(name);
      dto.UsedCalculationMethods = calculationMethods.Select(cm => new UsedCalculationMethodDTO { Category = cm.category, CalculationMethodName = cm.method }).ToList();
      return dto;
   }
}

internal class When_editing_a_simulation_configuration : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private SimulationConfiguration _simulationConfiguration;
   private MoleculeBuilder _floatingMolecule;
   private MoleculeBuilder _stationaryMolecule;
   private MoleculeBuilderDTO _floatingMoleculeDTO;

   protected override void Context()
   {
      base.Context();

      _floatingMolecule = CreateFloatingMolecule("Drug", ("Absorption", "Method1"));

      _stationaryMolecule = new MoleculeBuilder().WithName("Enzyme");
      _stationaryMolecule.IsFloating = false;

      var moleculeBuildingBlock = new MoleculeBuildingBlock { _floatingMolecule, _stationaryMolecule };
      var module = new Module { moleculeBuildingBlock };
      _simulationConfiguration = new SimulationConfiguration();
      _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));

      _floatingMoleculeDTO = CreateMoleculeDTO("Drug", ("Absorption", "Method1"));
      A.CallTo(() => _moleculeBuilderToDTOMapper.MapFrom(_floatingMolecule)).Returns(_floatingMoleculeDTO);
   }

   protected override void Because()
   {
      sut.Edit(_simulationConfiguration);
   }

   [Observation]
   public void should_show_only_non_stationary_molecules_in_view()
   {
      A.CallTo(() => _view.Show(A<IReadOnlyList<MoleculeBuilderDTO>>.That.Matches(list => list.Count == 1 && list[0] == _floatingMoleculeDTO))).MustHaveHappened();
   }

   [Observation]
   public void should_not_map_stationary_molecules()
   {
      A.CallTo(() => _moleculeBuilderToDTOMapper.MapFrom(_stationaryMolecule)).MustNotHaveHappened();
   }
}

internal class When_updating_for_a_selected_molecule : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private MoleculeBuilderDTO _moleculeDTO;

   protected override void Context()
   {
      base.Context();
      _moleculeDTO = CreateMoleculeDTO("Drug", ("Absorption", "Method1"));
   }

   protected override void Because()
   {
      sut.UpdateForSelectedMolecule(_moleculeDTO);
   }

   [Observation]
   public void should_bind_the_used_calculation_methods_to_the_view()
   {
      A.CallTo(() => _view.BindTo(_moleculeDTO.UsedCalculationMethods)).MustHaveHappened();
   }
}

internal class When_getting_calculation_methods_for_a_category : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private IReadOnlyList<string> _result;
   private CoreCalculationMethod _method1;
   private CoreCalculationMethod _method2;

   protected override void Context()
   {
      base.Context();
      _method1 = new CoreCalculationMethod{Name = "MethodA", Category = "Absorption"};
      _method2 = new CoreCalculationMethod{Name = "MethodB", Category = "Absorption" };

      A.CallTo(() => _calculationMethodsRepository.GetAllCalculationMethodsFor("Absorption"))
         .Returns(new List<CoreCalculationMethod> { _method1, _method2 });
   }

   protected override void Because()
   {
      _result = sut.GetCalculationMethodsForCategory("Absorption");
   }

   [Observation]
   public void should_return_the_names_of_all_calculation_methods_in_the_category()
   {
      _result.ShouldContain("MethodA", "MethodB");
   }
}

internal class When_setting_a_calculation_method : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private MoleculeBuilderDTO _moleculeDTO;

   protected override void Context()
   {
      base.Context();
      _moleculeDTO = CreateMoleculeDTO("Drug", ("Absorption", "OldMethod"));
      sut.UpdateForSelectedMolecule(_moleculeDTO);
   }

   protected override void Because()
   {
      sut.SetCalculationMethod(new UsedCalculationMethodDTO { Category = "Absorption", CalculationMethodName = "OldMethod" }, "OldMethod", "NewMethod");
   }

   [Observation]
   public void should_update_the_calculation_method_name_on_the_dto()
   {
      _moleculeDTO.UsedCalculationMethods.Single(x => x.Category == "Absorption").CalculationMethodName.ShouldBeEqualTo("NewMethod");
   }

   [Observation]
   public void should_rebind_the_view_with_updated_calculation_methods()
   {
      // Once during initial UpdateForSelectedMolecule, once after SetCalculationMethod
      A.CallTo(() => _view.BindTo(_moleculeDTO.UsedCalculationMethods)).MustHaveHappenedTwiceExactly();
   }
}

internal class When_setting_a_calculation_method_with_non_matching_category : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private MoleculeBuilderDTO _moleculeDTO;

   protected override void Context()
   {
      base.Context();
      _moleculeDTO = CreateMoleculeDTO("Drug", ("Absorption", "OldMethod"));
      sut.UpdateForSelectedMolecule(_moleculeDTO);
   }

   protected override void Because()
   {
      sut.SetCalculationMethod(new UsedCalculationMethodDTO { Category = "NonExistentCategory", CalculationMethodName = "OldMethod" }, "OldMethod", "NewMethod");
   }

   [Observation]
   public void should_not_change_existing_calculation_methods()
   {
      _moleculeDTO.UsedCalculationMethods.Single(x => x.Category == "Absorption").CalculationMethodName.ShouldBeEqualTo("OldMethod");
   }
}

internal class When_retrieving_calculation_method_overrides : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private Cache<string, IReadOnlyList<UsedCalculationMethod>> _result;
   private MoleculeBuilderDTO _drugADTO;
   private MoleculeBuilderDTO _drugBDTO;

   protected override void Context()
   {
      base.Context();

      var moleculeA = CreateFloatingMolecule("DrugA", ("Absorption", "AbsMethod"));
      var moleculeB = CreateFloatingMolecule("DrugB", ("Elimination", "ElimMethod"), ("Distribution", "DistMethod"));

      _drugADTO = CreateMoleculeDTO("DrugA", ("Absorption", "AbsMethod"));
      _drugBDTO = CreateMoleculeDTO("DrugB", ("Elimination", "ElimMethod"), ("Distribution", "DistMethod"));

      A.CallTo(() => _moleculeBuilderToDTOMapper.MapFrom(moleculeA)).Returns(_drugADTO);
      A.CallTo(() => _moleculeBuilderToDTOMapper.MapFrom(moleculeB)).Returns(_drugBDTO);

      var moleculeBuildingBlock = new MoleculeBuildingBlock { moleculeA, moleculeB };
      var module = new Module { moleculeBuildingBlock };
      var simulationConfiguration = new SimulationConfiguration();
      simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));

      sut.Edit(simulationConfiguration);
   }

   protected override void Because()
   {
      _result = sut.CalculationMethodOverrides;
   }

   [Observation]
   public void should_contain_overrides_for_each_molecule()
   {
      _result.Contains("DrugA").ShouldBeTrue();
      _result.Contains("DrugB").ShouldBeTrue();
   }

   [Observation]
   public void should_map_calculation_methods_correctly_for_first_molecule()
   {
      var drugAOverrides = _result["DrugA"];
      drugAOverrides.Count.ShouldBeEqualTo(1);
      drugAOverrides[0].Category.ShouldBeEqualTo("Absorption");
      drugAOverrides[0].CalculationMethod.ShouldBeEqualTo("AbsMethod");
   }

   [Observation]
   public void should_map_calculation_methods_correctly_for_second_molecule()
   {
      var drugBOverrides = _result["DrugB"];
      drugBOverrides.Count.ShouldBeEqualTo(2);
      drugBOverrides.Any(x => x.Category == "Elimination" && x.CalculationMethod == "ElimMethod").ShouldBeTrue();
      drugBOverrides.Any(x => x.Category == "Distribution" && x.CalculationMethod == "DistMethod").ShouldBeTrue();
   }
}

internal class When_retrieving_calculation_method_overrides_after_a_change : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private Cache<string, IReadOnlyList<UsedCalculationMethod>> _result;
   private MoleculeBuilderDTO _moleculeDTO;

   protected override void Context()
   {
      base.Context();

      var molecule = CreateFloatingMolecule("Drug", ("Absorption", "OldMethod"));
      _moleculeDTO = CreateMoleculeDTO("Drug", ("Absorption", "OldMethod"));

      A.CallTo(() => _moleculeBuilderToDTOMapper.MapFrom(molecule)).Returns(_moleculeDTO);

      var moleculeBuildingBlock = new MoleculeBuildingBlock { molecule };
      var module = new Module { moleculeBuildingBlock };
      var simulationConfiguration = new SimulationConfiguration();
      simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module));

      sut.Edit(simulationConfiguration);
      sut.UpdateForSelectedMolecule(_moleculeDTO);
      sut.SetCalculationMethod(new UsedCalculationMethodDTO { Category = "Absorption", CalculationMethodName = "OldMethod" }, "OldMethod", "NewMethod");
   }

   protected override void Because()
   {
      _result = sut.CalculationMethodOverrides;
   }

   [Observation]
   public void should_reflect_the_updated_calculation_method()
   {
      var overrides = _result["Drug"];
      overrides.Single(x => x.Category == "Absorption").CalculationMethod.ShouldBeEqualTo("NewMethod");
   }
}