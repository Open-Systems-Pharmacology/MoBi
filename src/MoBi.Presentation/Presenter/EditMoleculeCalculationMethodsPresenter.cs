using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.UsedCalculationMethods.Categories;

namespace MoBi.Presentation.Presenter;

public interface IEditMoleculeCalculationMethodsPresenter : ISimulationConfigurationItemPresenter
{
   void UpdateForSelectedMolecule(MoleculeBuilderDTO molecule);
   IReadOnlyList<string> GetCalculationMethodsForCategory(string category);
   void SetCalculationMethod(UsedCalculationMethodDTO dto, string oldValue, string newValue);
   Cache<string, IReadOnlyList<UsedCalculationMethod>> CalculationMethodOverrides { get; }
}

public class EditMoleculeCalculationMethodsPresenter : AbstractSubPresenter<IEditMoleculeCalculationMethodsView, IEditMoleculeCalculationMethodsPresenter>, IEditMoleculeCalculationMethodsPresenter
{
   private readonly IMoleculeBuilderToMoleculeBuilderDTOMapper _moleculeBuilderToDTOMoleculeBuilderMapper;
   private readonly ICoreCalculationMethodRepository _calculationMethodsRepository;
   private MoleculeBuilderDTO _molecule;
   private IReadOnlyList<MoleculeBuilderDTO> _moleculeDTOs;

   public EditMoleculeCalculationMethodsPresenter(IEditMoleculeCalculationMethodsView view, IMoleculeBuilderToMoleculeBuilderDTOMapper moleculeBuilderToDTOMoleculeBuilderMapper, ICoreCalculationMethodRepository calculationMethodsRepository) : base(view)
   {
      _moleculeBuilderToDTOMoleculeBuilderMapper = moleculeBuilderToDTOMoleculeBuilderMapper;
      _calculationMethodsRepository = calculationMethodsRepository;
   }

   public void Edit(SimulationConfiguration simulationConfiguration)
   {
      var molecules = nonStationaryMolecules(simulationConfiguration);
      _moleculeDTOs = molecules.MapAllUsing(_moleculeBuilderToDTOMoleculeBuilderMapper);
      _view.Show(_moleculeDTOs);
   }

   public void UpdateForSelectedMolecule(MoleculeBuilderDTO molecule)
   {
      _molecule = molecule;
      _view.BindTo(molecule.UsedCalculationMethods.Where(shouldShow).ToList());
   }

   private bool shouldShow(UsedCalculationMethodDTO usedCalculationMethodDTO)
   {
      return usedCalculationMethodDTO.Category.IsOneOf(DiffusionIntCell, DistributionCellular);
   }

   public IReadOnlyList<string> GetCalculationMethodsForCategory(string category)
   {
      return _calculationMethodsRepository
         .GetAllCalculationMethodsFor(category)
         .Where(x => !string.Equals(x.Name, AppConstants.DefaultNames.EmptyCalculationMethod))
         .Select(cm => cm.Name).ToList();
   }

   public void SetCalculationMethod(UsedCalculationMethodDTO dto, string oldValue, string newValue)
   {
      _molecule.UsedCalculationMethods.Where(x => string.Equals(x.Category, dto.Category) && string.Equals(x.CalculationMethodName, oldValue)).Each(x => x.CalculationMethodName = newValue);
      UpdateForSelectedMolecule(_molecule);
   }

   private static List<MoleculeBuilder> nonStationaryMolecules(SimulationConfiguration simulationConfiguration) =>
      simulationConfiguration.ModuleConfigurations.Select(x => x.BuildingBlock<MoleculeBuildingBlock>()).Where(x => x != null).SelectMany(x => x).Where(x => x.IsFloating).ToList();

   private IReadOnlyList<UsedCalculationMethod> usedCalculationMethodsForMolecule(string moleculeName) =>
      _moleculeDTOs.FindByName(moleculeName).UsedCalculationMethods.Select(dto => new UsedCalculationMethod(dto.Category, dto.CalculationMethodName)).ToList();

   private IReadOnlyList<string> moleculeNames => _moleculeDTOs.Select(x => x.Name).ToList();

   public Cache<string, IReadOnlyList<UsedCalculationMethod>> CalculationMethodOverrides
   {
      get
      {
         var overrides = new Cache<string, IReadOnlyList<UsedCalculationMethod>>();
         moleculeNames.Each(x => overrides[x] = usedCalculationMethodsForMolecule(x));
         return overrides;
      }
   }
}

public interface IEditMoleculeCalculationMethodsView : IView<IEditMoleculeCalculationMethodsPresenter>
{
   void Show(IReadOnlyList<MoleculeBuilderDTO> molecules);
   void BindTo(IReadOnlyList<UsedCalculationMethodDTO> usedCalculationMethods);
}