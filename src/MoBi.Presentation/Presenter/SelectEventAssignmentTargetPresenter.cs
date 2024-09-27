using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectEventAssignmentTargetPresenter : ISelectObjectPathPresenter
   {
      void Init(IContainer container, ICache<IObjectBase, string> forbiddenAssignees);
      FormulaUsablePath Select();
   }

   public class SelectEventAssignmentTargetPresenter : AbstractDisposablePresenter<ISelectObjectPathView, ISelectObjectPathPresenter>, ISelectEventAssignmentTargetPresenter
   {
      private readonly IContainerToContainerDTOMapper _containerDTOMapper;
      private readonly IMoBiContext _context;
      private readonly IMoleculeBuilderToDummyMoleculeDTOMapper _dummyMoleculeDTOMapper;
      private readonly IReactionBuilderToDummyReactionDTOMapper _dummyReactionDTOMapper;
      private readonly IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IParameterToDummyParameterDTOMapper _dummyParameterDTOMapper;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private readonly ISelectEntityInTreePresenter _selectEntityInTreePresenter;
      private readonly ISpatialStructureToSpatialStructureDTOMapper _spatialStructureDTOMapper;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private IReadOnlyList<MoleculeBuilder> _molecules;
      private IReadOnlyList<ReactionBuilder> _reactions;
      private ICache<IObjectBase, string> _forbiddenAssignees;

      public SelectEventAssignmentTargetPresenter(
         ISelectObjectPathView view, IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IContainerToContainerDTOMapper containerDTOMapper,
         IReactionBuilderToDummyReactionDTOMapper dummyReactionDTOMapper,
         IMoleculeBuilderToDummyMoleculeDTOMapper dummyMoleculeDTOMapper,
         IObjectPathFactory objectPathFactory,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IReactionDimensionRetriever dimensionRetriever,
         ISelectEntityInTreePresenter selectEntityInTreePresenter,
         ISpatialStructureToSpatialStructureDTOMapper spatialStructureDTOMapper,
         IBuildingBlockRepository buildingBlockRepository
      )
         : base(view)
      {
         _context = context;
         _dimensionRetriever = dimensionRetriever;
         _selectEntityInTreePresenter = selectEntityInTreePresenter;
         _spatialStructureDTOMapper = spatialStructureDTOMapper;
         _buildingBlockRepository = buildingBlockRepository;
         _objectPathFactory = objectPathFactory;
         _dummyMoleculeDTOMapper = dummyMoleculeDTOMapper;
         _dummyReactionDTOMapper = dummyReactionDTOMapper;
         _containerDTOMapper = containerDTOMapper;
         _dummyParameterDTOMapper = dummyParameterDTOMapper;
         _objectBaseDTOMapper = objectBaseDTOMapper;
         AddSubPresenters(_selectEntityInTreePresenter);
         _selectEntityInTreePresenter.GetChildren = GetChildren;
         _selectEntityInTreePresenter.OnSelectedEntityChanged += (o, e) => ViewChanged();
         _view.AddSelectionView(_selectEntityInTreePresenter.View);
      }

      public FormulaUsablePath Select()
      {
         _view.Display();
         return _view.Canceled ? null : generatePathFromDTO(_selectEntityInTreePresenter.SelectedDTO);
      }

      public override void ViewChanged()
      {
         base.ViewChanged();
         _view.OkEnabled = CanClose;
      }

      //make this method public so that it can be tested
      public IReadOnlyList<ObjectBaseDTO> GetChildren(ObjectBaseDTO parentDTO)
      {
         var parent = parentDTO.ObjectBase;
         if (parent.IsAnImplementationOf<IDistributedParameter>())
            return Array.Empty<ObjectBaseDTO>();

         var container = parent as IContainer;
         if (container == null)
            return Array.Empty<ObjectBaseDTO>();

         if (parent.IsAnImplementationOf<MoleculeBuilder>() || parent.IsAnImplementationOf<ReactionBuilder>())
         {
            //Molecule builder and reaction builder are dummy entities at that stage=>add dummy parameters
            if (isDummy(parentDTO))
               return dummyLocalParametersUnder(container, parentDTO);

            //not dummy, this is a top container that requires global parameters
            return map(globalParameterUnder(container));
         }

         //Real structural container. 
         var list = new List<ObjectBaseDTO>();

         //Add sub containers
         list.AddRange(map(container.GetChildrenSortedByName<IContainer>(x => !x.IsNamed(Constants.MOLECULE_PROPERTIES))));
         //Add local parameters
         list.AddRange(map(localParametersUnder(container)));
         if (container.Mode == ContainerMode.Physical)
         {
            list.AddRange(getLocalInformationForMolecules(container));
            list.AddRange(getLocalInformationForReaction(container));
         }

         list.Where(objectSelectionIsForbidden).Each(x => cannotSelectDescription(x, _forbiddenAssignees[x.ObjectBase]));

         return list;
      }

      private bool objectSelectionIsForbidden(ObjectBaseDTO x)
      {
         return x.ObjectBase != null && _forbiddenAssignees.Keys.Contains(x.ObjectBase);
      }

      private void cannotSelectDescription(ObjectBaseDTO objectBaseDTO, string forbiddenReason)
      {
         objectBaseDTO.Description += Environment.NewLine + AppConstants.Captions.ObjectCannotBeSelected(forbiddenReason);
         objectBaseDTO.Description = objectBaseDTO.Description.Trim();
      }

      private IReadOnlyList<ObjectBaseDTO> map(IEnumerable<IObjectBase> objectsToMap) => objectsToMap.MapAllUsing(_objectBaseDTOMapper);

      private IReadOnlyList<ObjectBaseDTO> dummyLocalParametersUnder(IContainer container, ObjectBaseDTO parentDTO)
      {
         return localParametersUnder(container).Select(x => _dummyParameterDTOMapper.MapFrom(x, container, parentDTO)).ToList();
      }

      private IEnumerable<IParameter> localParametersUnder(IContainer container)
      {
         return container.GetChildrenSortedByName<IParameter>(para => para.BuildMode == ParameterBuildMode.Local);
      }

      private IEnumerable<IParameter> globalParameterUnder(IContainer container)
      {
         return container.GetChildrenSortedByName<IParameter>(para => para.BuildMode == ParameterBuildMode.Global);
      }

      public override bool CanClose => base.CanClose && isValidSelection(_selectEntityInTreePresenter.SelectedDTO);

      private bool isValidSelection(ObjectBaseDTO selectedDTO)
      {
         if (selectedDTO == null)
            return false;

         if (isDummy(selectedDTO))
            return true;

         var selection = _context.Get<IObjectBase>(selectedDTO.Id);
         return selection != null && selection.IsAnImplementationOf<IUsingFormula>() && !_forbiddenAssignees.Keys.Contains(selection);
      }

      private bool isDummy(ObjectBaseDTO selectedDTO)
      {
         return selectedDTO.IsAnImplementationOf<DummyMoleculeDTO>() ||
                selectedDTO.IsAnImplementationOf<DummyReactionDTO>() ||
                selectedDTO.IsAnImplementationOf<DummyParameterDTO>();
      }

      public void Init(IContainer container, ICache<IObjectBase, string> forbiddenAssignees)
      {
         _molecules = _buildingBlockRepository.MoleculeBlockCollection.SelectMany(bb => bb.All()).Distinct(new NameComparer<MoleculeBuilder>()).OrderBy(x => x.Name).ToList();
         _reactions = _buildingBlockRepository.ReactionBlockCollection.SelectMany(bb => bb.All()).Distinct(new NameComparer<ReactionBuilder>()).OrderBy(x => x.Name).ToList();
         _forbiddenAssignees = forbiddenAssignees;
         var list = new List<ObjectBaseDTO>();
         list.AddRange(_buildingBlockRepository.SpatialStructureCollection.MapAllUsing(_spatialStructureDTOMapper));
         list.Add(_objectBaseDTOMapper.MapFrom(container));
         list.AddRange(globalReactionParameters());

         _selectEntityInTreePresenter.InitTreeStructure(list);
      }

      private IReadOnlyList<ObjectBaseDTO> globalReactionParameters() => _reactions.Where(x => globalParameterUnder(x).Any()).MapAllUsing(_containerDTOMapper);

      private FormulaUsablePath generatePathFromDTO(ObjectBaseDTO dto)
      {
         if (dto.IsAnImplementationOf<DummyParameterDTO>())
         {
            var dummy = dto.DowncastTo<DummyParameterDTO>();
            var parent = getExistingParentContainerFromDTO(dto);
            var path = _objectPathFactory.CreateAbsoluteObjectPath(parent)
               .AndAdd(dummy.ModelParentName)
               .AndAdd(dummy.Name);

            return formulaUsablePathFrom(path, getDimensionForDummyParameter(dummy));
         }

         if (dto.IsAnImplementationOf<IDummyContainer>())
         {
            var parent = dto.DowncastTo<IDummyContainer>().StructureParent;
            var path = _objectPathFactory.CreateAbsoluteObjectPath(parent).AndAdd(dto.Name);
            return formulaUsablePathFrom(path, getDimensionFor(dto));
         }

         var selectedEntity = _context.Get<IEntity>(dto.Id);
         if (selectedEntity.IsAnImplementationOf<IUsingFormula>())
         {
            var usingFormula = selectedEntity.DowncastTo<IUsingFormula>();
            var path = _objectPathFactory.CreateAbsoluteObjectPath(usingFormula);
            return formulaUsablePathFrom(path, usingFormula.Dimension);
         }

         return null;
      }

      private FormulaUsablePath formulaUsablePathFrom(ObjectPath objectPath, IDimension dimension)
      {
         return _objectPathFactory.CreateFormulaUsablePathFrom(objectPath).WithDimension(dimension);
      }

      private IDimension getDimensionForDummyParameter(DummyParameterDTO dummy) => dummy.Parameter.Dimension;

      private IDimension getDimensionFor(ObjectBaseDTO dto)
      {
         return dto.IsAnImplementationOf<DummyMoleculeDTO>() ? _dimensionRetriever.MoleculeDimension : _dimensionRetriever.ReactionDimension;
      }

      private IEntity getExistingParentContainerFromDTO(ObjectBaseDTO dto)
      {
         var treeNode = _selectEntityInTreePresenter.TreeNodeFor(dto);
         var dtoParent = treeNode.ParentNode.ParentNode.TagAsObject.DowncastTo<ObjectBaseDTO>();
         return dtoParent.ObjectBase as IEntity;
      }

   

      private IEnumerable<ObjectBaseDTO> getLocalInformationForReaction(IContainer container)
      {
         return _reactions.Select(x => _dummyReactionDTOMapper.MapFrom(x, container));
      }

      private IEnumerable<ObjectBaseDTO> getLocalInformationForMolecules(IContainer container)
      {
         return _molecules.Select(x => _dummyMoleculeDTOMapper.MapFrom(x, container));
      }
   }
}