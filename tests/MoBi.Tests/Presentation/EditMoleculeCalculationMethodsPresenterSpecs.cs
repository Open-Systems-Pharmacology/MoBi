using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation;

internal class concern_for_EditMoleculeCalculationMethodsPresenter : ContextSpecification<EditMoleculeCalculationMethodsPresenter>
{
   protected IEditMoleculeCalculationMethodsView _view;
   protected ISimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper;
   protected IModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper;
   private ICoreCalculationMethodRepository _calculationMethodsRepository;

   protected override void Context()
   {
      _view = A.Fake<IEditMoleculeCalculationMethodsView>();
      _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper = A.Fake<ISimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper>();
      _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper = A.Fake<IModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper>();
      _calculationMethodsRepository = A.Fake<ICoreCalculationMethodRepository>();

      sut = new EditMoleculeCalculationMethodsPresenter(_view, _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper, _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper, _calculationMethodsRepository);
   }

   protected MoleculeUsedCalculationMethodsDTO CreateMoleculeUsedCalculationMethodsDTO(string name, params (string category, string method)[] calculationMethods)
   {
      var dto = new MoleculeUsedCalculationMethodsDTO { MoleculeName = name };
      foreach (var (category, method) in calculationMethods)
         dto.AddUsedCalculationMethod(new UsedCalculationMethodDTO { Category = category, CalculationMethodName = method });
      return dto;
   }

   protected void SetupCalculationMethodsForCategory(string category, params string[] methodNames)
   {
      var methods = methodNames.Select(n => new CoreCalculationMethod { Name = n, Category = category }).ToList();
      A.CallTo(() => _calculationMethodsRepository.GetAllCalculationMethodsFor(category)).Returns(methods);
   }
}

internal class When_getting_calculation_methods_for_category : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private IReadOnlyList<string> _result;

   protected override void Context()
   {
      base.Context();
      SetupCalculationMethodsForCategory("Partition", "MethodA", "MethodB", AppConstants.DefaultNames.EmptyCalculationMethod);
   }

   protected override void Because()
   {
      _result = sut.GetCalculationMethodsForCategory("Partition");
   }

   [Observation]
   public void should_return_methods_excluding_the_empty_calculation_method()
   {
      _result.ShouldOnlyContain("MethodA", "MethodB");
   }
}

internal class When_updating_for_selected_molecule : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private MoleculeUsedCalculationMethodsDTO _moleculeDTO;
   private IReadOnlyList<UsedCalculationMethodDTO> _boundCalculationMethods;

   protected override void Context()
   {
      base.Context();
      // "MultiOptionCategory" has 2 methods => should be shown
      SetupCalculationMethodsForCategory("MultiOptionCategory", "Method1", "Method2");
      // "SingleOptionCategory" has only 1 method => should be filtered out
      SetupCalculationMethodsForCategory("SingleOptionCategory", "OnlyMethod");

      _moleculeDTO = CreateMoleculeUsedCalculationMethodsDTO("Drug",
         ("MultiOptionCategory", "Method1"),
         ("SingleOptionCategory", "OnlyMethod"));

      A.CallTo(() => _view.BindTo(A<IReadOnlyList<UsedCalculationMethodDTO>>._))
         .Invokes(call => _boundCalculationMethods = call.GetArgument<IReadOnlyList<UsedCalculationMethodDTO>>(0));
   }

   protected override void Because()
   {
      sut.UpdateForSelectedMolecule(_moleculeDTO);
   }

   [Observation]
   public void should_only_bind_calculation_methods_with_more_than_one_option()
   {
      _boundCalculationMethods.Count.ShouldBeEqualTo(1);
      _boundCalculationMethods[0].Category.ShouldBeEqualTo("MultiOptionCategory");
   }
}

internal class When_setting_calculation_method : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private MoleculeUsedCalculationMethodsDTO _moleculeDTO;

   protected override void Context()
   {
      base.Context();
      SetupCalculationMethodsForCategory("Partition", "MethodA", "MethodB");

      _moleculeDTO = CreateMoleculeUsedCalculationMethodsDTO("Drug", ("Partition", "MethodA"));

      // Select the molecule first so that _selectedMoleculeDTO is set
      sut.UpdateForSelectedMolecule(_moleculeDTO);
   }

   protected override void Because()
   {
      sut.SetCalculationMethod(_moleculeDTO.UsedCalculationMethods.First(), "MethodA", "MethodB");
   }

   [Observation]
   public void should_update_the_calculation_method_name()
   {
      _moleculeDTO.UsedCalculationMethods.First().CalculationMethodName.ShouldBeEqualTo("MethodB");
   }

   [Observation]
   public void should_rebind_the_view()
   {
      // Once in Context (UpdateForSelectedMolecule) and once in Because (SetCalculationMethod calls UpdateForSelectedMolecule)
      A.CallTo(() => _view.BindTo(A<IReadOnlyList<UsedCalculationMethodDTO>>._)).MustHaveHappenedTwiceExactly();
   }
}

internal class When_getting_all_used_calculation_methods_for_existing_molecule : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private IReadOnlyList<UsedCalculationMethod> _result;
   private SimulationConfiguration _simulationConfiguration;

   protected override void Context()
   {
      base.Context();
      _simulationConfiguration = new SimulationConfiguration();
      sut.Edit(_simulationConfiguration);

      var moduleMoleculeDTO = CreateMoleculeUsedCalculationMethodsDTO("Drug", ("Partition", "MethodA"));

      A.CallTo(() => _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper.MapFrom(_simulationConfiguration))
         .Returns(new List<MoleculeUsedCalculationMethodsDTO>());

      var moduleConfiguration = new ModuleConfiguration(new Module());
      A.CallTo(() => _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper.MapFrom(moduleConfiguration))
         .Returns(new List<MoleculeUsedCalculationMethodsDTO> { moduleMoleculeDTO });

      SetupCalculationMethodsForCategory("Partition", "MethodA", "MethodB");

      var moduleConfigDTO = new ModuleConfigurationDTO(moduleConfiguration);

      sut.RefreshWith(new List<ModuleConfigurationDTO> { moduleConfigDTO });
   }

   protected override void Because()
   {
      _result = sut.AllUsedCalculationMethodsFor("Drug");
   }

   [Observation]
   public void should_return_the_calculation_methods_for_the_molecule()
   {
      _result.Count.ShouldBeEqualTo(1);
      _result[0].Category.ShouldBeEqualTo("Partition");
      _result[0].CalculationMethod.ShouldBeEqualTo("MethodA");
   }
}

internal class When_getting_all_used_calculation_methods_for_unknown_molecule : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private IReadOnlyList<UsedCalculationMethod> _result;

   protected override void Because()
   {
      _result = sut.AllUsedCalculationMethodsFor("UnknownMolecule");
   }

   [Observation]
   public void should_return_an_empty_list()
   {
      _result.ShouldBeEmpty();
   }
}

internal class When_refreshing_with_module_configurations_and_simulation_overrides : concern_for_EditMoleculeCalculationMethodsPresenter
{
   private SimulationConfiguration _simulationConfiguration;
   private IReadOnlyList<MoleculeUsedCalculationMethodsDTO> _shownMolecules;
   private ModuleConfigurationDTO _moduleConfigDTO;

   protected override void Context()
   {
      base.Context();
      _simulationConfiguration = new SimulationConfiguration();
      sut.Edit(_simulationConfiguration);

      // Simulation has overrides for "Drug" with MethodB
      var simulationDrugDTO = CreateMoleculeUsedCalculationMethodsDTO("Drug", ("Partition", "MethodB"));

      // Simulation has another override, but the molecule "Drug3" is not present in the module configuration, so it should be ignored
      var simulationDrugDTO2 = CreateMoleculeUsedCalculationMethodsDTO("Drug3", ("Partition", "MethodB"));
      A.CallTo(() => _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper.MapFrom(_simulationConfiguration))
         .Returns(new List<MoleculeUsedCalculationMethodsDTO> { simulationDrugDTO, simulationDrugDTO2 });

      // Module configuration has "Drug" with MethodA (will be overridden) and "Drug2" with MethodC (no override)
      var moduleDrug1DTO = CreateMoleculeUsedCalculationMethodsDTO("Drug", ("Partition", "MethodA"));
      var moduleDrug2DTO = CreateMoleculeUsedCalculationMethodsDTO("Drug2", ("Permeability", "MethodC"));

      var moduleConfiguration = new ModuleConfiguration(new Module());
      A.CallTo(() => _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper.MapFrom(moduleConfiguration))
         .Returns(new List<MoleculeUsedCalculationMethodsDTO> { moduleDrug1DTO, moduleDrug2DTO });

      SetupCalculationMethodsForCategory("Partition", "MethodA", "MethodB");
      SetupCalculationMethodsForCategory("Permeability", "MethodC", "MethodD");

      _moduleConfigDTO = new ModuleConfigurationDTO(moduleConfiguration);

      A.CallTo(() => _view.Show(A<IReadOnlyList<MoleculeUsedCalculationMethodsDTO>>._))
         .Invokes(call => _shownMolecules = call.GetArgument<IReadOnlyList<MoleculeUsedCalculationMethodsDTO>>(0));
   }

   protected override void Because()
   {
      sut.RefreshWith(new List<ModuleConfigurationDTO> { _moduleConfigDTO });
   }

   [Observation]
   public void should_show_molecules_from_module_configurations()
   {
      _shownMolecules.Count.ShouldBeEqualTo(2);
   }

   [Observation]
   public void should_use_simulation_overrides_for_existing_molecules()
   {
      var drugDTO = _shownMolecules.First(x => string.Equals(x.Name, "Drug"));
      drugDTO.UsedCalculationMethods.First().CalculationMethodName.ShouldBeEqualTo("MethodB");
   }

   [Observation]
   public void should_use_module_values_for_molecules_without_simulation_overrides()
   {
      var drug2DTO = _shownMolecules.First(x => string.Equals(x.Name, "Drug2"));
      drug2DTO.UsedCalculationMethods.First().CalculationMethodName.ShouldBeEqualTo("MethodC");
   }

   [Observation]
   public void should_return_correct_molecule_names()
   {
      sut.MoleculeNames.ShouldContain("Drug", "Drug2");
   }
}