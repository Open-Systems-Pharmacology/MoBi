using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter;

public interface IEditMoleculeCalculationMethodsPresenter : ISimulationConfigurationItemPresenter
{
   void UpdateForSelectedMolecule(MoleculeUsedCalculationMethodsDTO molecule);
   IReadOnlyList<string> GetCalculationMethodsForCategory(string category);
   void SetCalculationMethod(UsedCalculationMethodDTO dto, string oldValue, string newValue);
   IReadOnlyList<string> MoleculeNames { get; }
   IReadOnlyList<UsedCalculationMethod> AllUsedCalculationMethodsFor(string moleculeName);
   void RefreshWith(IReadOnlyList<ModuleConfigurationDTO> moduleConfigurationDTOs);
}

public class EditMoleculeCalculationMethodsPresenter : AbstractSubPresenter<IEditMoleculeCalculationMethodsView, IEditMoleculeCalculationMethodsPresenter>, IEditMoleculeCalculationMethodsPresenter
{
   private readonly ISimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper;
   private readonly IModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper;
   private readonly ICoreCalculationMethodRepository _calculationMethodsRepository;
   private MoleculeUsedCalculationMethodsDTO _selectedMoleculeDTO;
   private readonly List<MoleculeUsedCalculationMethodsDTO> _moleculeDTOs = new();
   private SimulationConfiguration _simulationConfiguration;

   public EditMoleculeCalculationMethodsPresenter(
      IEditMoleculeCalculationMethodsView view,
      ISimulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper,
      IModuleConfigurationToMoleculeUsedCalculationMethodsDTOMapper moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper,
      ICoreCalculationMethodRepository calculationMethodsRepository) : base(view)
   {
      _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper = simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper;
      _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper = moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper;
      _calculationMethodsRepository = calculationMethodsRepository;
   }

   public void Edit(SimulationConfiguration simulationConfiguration)
   {
      _simulationConfiguration = simulationConfiguration;
   }

   public void UpdateForSelectedMolecule(MoleculeUsedCalculationMethodsDTO molecule)
   {
      _selectedMoleculeDTO = molecule;
      _view.BindTo(molecule.UsedCalculationMethods.Where(shouldShow).ToList());
   }

   private bool shouldShow(UsedCalculationMethodDTO usedCalculationMethodDTO) =>
      GetCalculationMethodsForCategory(usedCalculationMethodDTO.Category).Count > 1;

   public IReadOnlyList<string> GetCalculationMethodsForCategory(string category)
   {
      return _calculationMethodsRepository
         .GetAllCalculationMethodsFor(category)
         .Where(x => !string.Equals(x.Name, AppConstants.DefaultNames.EmptyCalculationMethod))
         .Select(cm => cm.Name).ToList();
   }

   public void SetCalculationMethod(UsedCalculationMethodDTO dto, string oldValue, string newValue)
   {
      _selectedMoleculeDTO.UsedCalculationMethods.Where(x => string.Equals(x.Category, dto.Category) && string.Equals(x.CalculationMethodName, oldValue)).Each(x => x.CalculationMethodName = newValue);
      UpdateForSelectedMolecule(_selectedMoleculeDTO);
   }

   public IReadOnlyList<string> MoleculeNames => _moleculeDTOs.AllNames();

   public IReadOnlyList<UsedCalculationMethod> AllUsedCalculationMethodsFor(string moleculeName)
   {
      var moleculeBuilderDTO = _moleculeDTOs.SingleOrDefault(x => string.Equals(x.Name, moleculeName));
      return moleculeBuilderDTO == null ? new List<UsedCalculationMethod>() : moleculeBuilderDTO.UsedCalculationMethods.Select(x => new UsedCalculationMethod(x.Category, x.CalculationMethodName)).ToList();
   }

   /// <summary>
   ///    Refreshes the internal list of molecules to reflect the provided module configurations, applying any relevant
   ///    overrides from the current simulation configuration.
   /// </summary>
   /// <param name="moduleConfigurationDTOs">
   ///    A read-only list of module configuration data transfer objects that specify which molecules should be included in
   ///    the refreshed list. Each configuration may influence the final set of molecules displayed.
   /// </param>
   public void RefreshWith(IReadOnlyList<ModuleConfigurationDTO> moduleConfigurationDTOs)
   {
      _moleculeDTOs.Clear();
      var simulationDTOs = _simulationConfigurationToMoleculeUsedCalculationMethodsDTOMapper.MapFrom(_simulationConfiguration);
      var moduleDTOs = moduleConfigurationDTOs.SelectMany(x => _moduleConfigurationToMoleculeUsedCalculationMethodsDTOMapper.MapFrom(x.ModuleConfiguration)).ToList();

      // moduleDTOs has the list of present molecules in the new configuration, simulationDTOs has the existing overrides
      // we only want to see the list of present molecules with any applicable overrides.
      // For example, if the overrides from the simulation has a molecule that is not in the module configuration, we don't want to show it.
      moduleDTOs.GroupBy(x => x.MoleculeName).Each(group =>
      {
         // Take the last moduleDTO that is present in the configuration. Those are the default UsedCalculationMethods
         var moduleDTO = group.Last();
         var simulationDTO = simulationDTOs.SingleOrDefault(x => string.Equals(x.Name, moduleDTO.Name));
         _moleculeDTOs.Add(simulationDTO ?? moduleDTO);
      });

      _view.Show(_moleculeDTOs);
      if(_moleculeDTOs.Any())
         UpdateForSelectedMolecule(_moleculeDTOs.First());
   }
}

public interface IEditMoleculeCalculationMethodsView : IView<IEditMoleculeCalculationMethodsPresenter>
{
   void Show(IReadOnlyList<MoleculeUsedCalculationMethodsDTO> molecules);
   void BindTo(IReadOnlyList<UsedCalculationMethodDTO> usedCalculationMethods);
}