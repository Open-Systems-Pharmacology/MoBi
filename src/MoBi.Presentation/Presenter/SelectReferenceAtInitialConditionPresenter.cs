using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtInitialConditionPresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtInitialConditionPresenter : SelectReferencePresenterBase, ISelectReferenceAtInitialConditionPresenter
   {
      private readonly IMoleculeBuilderToDummyMoleculeDTOMapper _moleculeInContainerMapper;

      public SelectReferenceAtInitialConditionPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreatorAtInitialCondition objectPathCreator,
         IBuildingBlockRepository buildingBlockRepository,
         IMoleculeBuilderToDummyMoleculeDTOMapper moleculeInContainerMapper)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.PhysicalContainerOnly, buildingBlockRepository)
      {
         _moleculeInContainerMapper = moleculeInContainerMapper;
         view.EnableMultiSelect = true;
         DisableTimeSelection();
      }

      protected override void AddSpecificInitialObjects() => AddSpatialStructures();

      public override IEnumerable<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto)
      {
         if (dto is DummyMoleculeDTO)
            return Enumerable.Empty<ObjectBaseDTO>();

         if (dto.ObjectBase is SpatialStructure spatialStructure)
            return spatialStructure.TopContainers.OrderBy(x => x.Name).MapAllUsing(_objectBaseDTOMapper);

         if (dto.ObjectBase is IContainer container)
            return subContainersAndMoleculesIn(container);

         return Enumerable.Empty<ObjectBaseDTO>();
      }

      private IEnumerable<ObjectBaseDTO> subContainersAndMoleculesIn(IContainer container)
      {
         var children = container.GetChildrenSortedByName<IContainer>(isStructuralContainer).MapAllUsing(_objectBaseDTOMapper).ToList();

         if (container.Mode == ContainerMode.Physical)
            children.AddRange(allMolecules.Select(x => _moleculeInContainerMapper.MapFrom(x, container)));

         return children;
      }

      private static bool isStructuralContainer(IContainer container) => !container.IsNamed(Constants.MOLECULE_PROPERTIES) && !(container is IParameter);

      private IEnumerable<MoleculeBuilder> allMolecules => _buildingBlockRepository.MoleculeBlockCollection.SelectMany(x => x).Distinct(new NameComparer<MoleculeBuilder>()).OrderBy(x => x.Name);

      public override IReadOnlyList<ObjectPath> GetAllSelections() =>
         selectedMolecules.Select(x => _objectPathFactory.CreateAbsoluteObjectPath(x.StructureParent).AndAdd(x.Name)).ToList();

      public override bool CanClose
      {
         get
         {
            var allSelected = _view.AllSelectedDTOs;
            return allSelected.Any() && allSelected.All(x => x is DummyMoleculeDTO);
         }
      }

      private IEnumerable<DummyMoleculeDTO> selectedMolecules => _view.AllSelectedDTOs.OfType<DummyMoleculeDTO>();
   }
}
