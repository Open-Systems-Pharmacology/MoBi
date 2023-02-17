using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectEventAssingmentTargetPresenter : IDisposablePresenter
   {
      void Init(IMoBiProject project, IContainer container);
      FormulaUsablePath Select();
      IEnumerable<IObjectBaseDTO> GetChildren(IObjectBaseDTO id);
      bool IsValidSelection(IObjectBaseDTO selectedDTO);
   }

   internal class SelectEventAssingmentTargetPresenter : AbstractDisposablePresenter<ISelectEventAssignmentTargetView, ISelectEventAssingmentTargetPresenter>, ISelectEventAssingmentTargetPresenter
   {
      private readonly IContainerToContainerDTOMapper _containerDTOMapper;
      private readonly IMoBiContext _context;
      private readonly IMoleculeBuilderToDummyMoleculeDTOMapper _dummyMoleculeDTOMapper;
      private readonly IReactionBuilderToDummyReactionDTOMapper _dummyReactionDTOMapper;
      private readonly IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IParameterToDummyParameterDTOMapper _dummyParameterDTOMapper;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private IReadOnlyList<IMoleculeBuilder> _molecules;
      private IReadOnlyList<IReactionBuilder> _reactions;

      public SelectEventAssingmentTargetPresenter(ISelectEventAssignmentTargetView view, IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper, IContainerToContainerDTOMapper containerDTOMapper,
         IReactionBuilderToDummyReactionDTOMapper dummyReactionDTOMapper,
         IMoleculeBuilderToDummyMoleculeDTOMapper dummyMoleculeDTOMapper,
         IObjectPathFactory objectPathFactory, IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IReactionDimensionRetriever dimensionRetriever)
         : base(view)
      {
         _context = context;
         _dimensionRetriever = dimensionRetriever;
         _objectPathFactory = objectPathFactory;
         _dummyMoleculeDTOMapper = dummyMoleculeDTOMapper;
         _dummyReactionDTOMapper = dummyReactionDTOMapper;
         _containerDTOMapper = containerDTOMapper;
         _dummyParameterDTOMapper = dummyParameterDTOMapper;
         _objectBaseDTOMapper = objectBaseDTOMapper;
      }

      public FormulaUsablePath Select()
      {
         _view.Display();

         return _view.Canceled ? null : generatePathFromDTO(_view.Selected);
      }

      public IEnumerable<IObjectBaseDTO> GetChildren(IObjectBaseDTO parentDTO)
      {
         var parent = getObjectFrom(parentDTO);
         if (parent.IsAnImplementationOf<IDistributedParameter>())
            return Enumerable.Empty<IObjectBaseDTO>();

         var container = parent as IContainer;
         if (container == null)
            return Enumerable.Empty<IObjectBaseDTO>();

         if (parent.IsAnImplementationOf<IMoleculeBuilder>() || parent.IsAnImplementationOf<IReactionBuilder>())
         {
            //Molecule builder and reaction builder are dummy entities at that stage=>add dummy parameters
            if (isDummy(parentDTO))
               return dummyLocalParametersUnder(parent, container, parentDTO);

            //not dummy, this is a top container that requires global parameters
            return map(globalParameterUnder(container));
         }
         
         //Real structural container. 
         var list = new List<IObjectBaseDTO>();

         //Add sub containers
         list.AddRange(map(container.GetChildrenSortedByName<IContainer>(x => !x.IsNamed(Constants.MOLECULE_PROPERTIES))));
         //Add local parameters
         list.AddRange(map(localParametersUnder(container)));
         if (container.Mode == ContainerMode.Physical)
         {
            list.AddRange(getLocalInformationForMolecules(container));
            list.AddRange(getLocalInformationForReaction(container));
         }
         return list;
      }

      private IReadOnlyList<IObjectBaseDTO> map(IEnumerable<IObjectBase> objectsToMap)
      {
         return objectsToMap.MapAllUsing(_objectBaseDTOMapper);
      } 

      private IEnumerable<IObjectBaseDTO> dummyLocalParametersUnder(IObjectBase parent, IContainer container, IObjectBaseDTO parentDTO)
      {
         return localParametersUnder(container).Select(x => _dummyParameterDTOMapper.MapFrom(x, container, parentDTO));
      }

      private IEnumerable<IParameter> localParametersUnder(IContainer container)
      {
         return container.GetChildrenSortedByName<IParameter>(para => para.BuildMode == ParameterBuildMode.Local);
      }

      private IEnumerable<IParameter> globalParameterUnder(IContainer container)
      {
         return container.GetChildrenSortedByName<IParameter>(para => para.BuildMode == ParameterBuildMode.Global);
      }

      public bool IsValidSelection(IObjectBaseDTO selectedDTO)
      {
         if (selectedDTO == null)
            return false;

         if (isDummy(selectedDTO))
            return true;

         var selection = _context.Get<IObjectBase>(selectedDTO.Id);
         return selection != null && selection.IsAnImplementationOf<IUsingFormula>();
      }

      private bool isDummy(IObjectBaseDTO selectedDTO)
      {
         return selectedDTO.IsAnImplementationOf<DummyMoleculeDTO>() ||
                selectedDTO.IsAnImplementationOf<DummyReactionDTO>() ||
                selectedDTO.IsAnImplementationOf<DummyParameterDTO>();
      }

      public void Init(IMoBiProject project, IContainer container)
      {
         _molecules = project.MoleculeBlockCollection.SelectMany(bb => bb.All()).Distinct(new NameComparer<IMoleculeBuilder>()).OrderBy(x => x.Name).ToList();
         _reactions = project.ReactionBlockCollection.SelectMany(bb => bb.All()).Distinct(new NameComparer<IReactionBuilder>()).OrderBy(x=>x.Name).ToList();
         var list = project.SpatialStructureCollection.Select(CreateSpatialStuctureDTOFrom).Cast<IObjectBaseDTO>().ToList();
         list.Add(_objectBaseDTOMapper.MapFrom(container));
         list.AddRange(globalReactionParameters());
         _view.BindTo(list);
      }

      private IEnumerable<IObjectBaseDTO> globalReactionParameters()
      {
         return _reactions.Where(x=>globalParameterUnder(x).Any()).Select(x => _containerDTOMapper.MapFrom(x));
      }

      private FormulaUsablePath generatePathFromDTO(IObjectBaseDTO dto)
      {
         if (dto.IsAnImplementationOf<DummyParameterDTO>())
         {
            var dummy = dto.DowncastTo<DummyParameterDTO>();
            var parent = getExistingParentContainerFromDTO(dto);
            var path = _objectPathFactory.CreateAbsoluteObjectPath(parent)
               .AndAdd(dummy.ModelParentName)
               .AndAdd(dummy.Name);

            return formulatUsablePathFrom(path, getDimensionForDummyParameter(dummy));
         }

         if (dto.IsAnImplementationOf<IDummyContainer>())
         {
            var parent = dto.DowncastTo<IDummyContainer>().StructureParent;
            var path = _objectPathFactory.CreateAbsoluteObjectPath(parent).AndAdd(dto.Name);
            return formulatUsablePathFrom(path, getDimensionFor(dto));
         }

         var selectedEntity = _context.Get<IEntity>(dto.Id);
         if (selectedEntity.IsAnImplementationOf<IUsingFormula>())
         {
            var usingFormula = selectedEntity.DowncastTo<IUsingFormula>();
            var path = _objectPathFactory.CreateAbsoluteObjectPath(usingFormula);
            return formulatUsablePathFrom(path, usingFormula.Dimension);
         }

         return null;
      }

      private FormulaUsablePath formulatUsablePathFrom(ObjectPath objectPath, IDimension dimension)
      {
         return _objectPathFactory.CreateFormulaUsablePathFrom(objectPath).WithDimension(dimension);
      }

      private IDimension getDimensionForDummyParameter(DummyParameterDTO dummy)
      {
         return _context.Get<IParameter>(dummy.ParameterToUse.Id).Dimension;
      }

      private IDimension getDimensionFor(IObjectBaseDTO dto)
      {
         return dto.IsAnImplementationOf<DummyMoleculeDTO>() ? _dimensionRetriever.MoleculeDimension : _dimensionRetriever.ReactionDimension;
      }

      private IEntity getExistingParentContainerFromDTO(IObjectBaseDTO dto)
      {
         var treeNode = _view.GetNode(dto.Id);
         var dtoParent = treeNode.ParentNode.ParentNode.TagAsObject.DowncastTo<IObjectBaseDTO>();
         return _context.Get<IEntity>(dtoParent.Id);
      }

      private IObjectBase getObjectFrom(IObjectBaseDTO dto)
      {
         if (dto.IsAnImplementationOf<DummyMoleculeDTO>())
            return dto.DowncastTo<DummyMoleculeDTO>().MoleculeBuilder;

         if (dto.IsAnImplementationOf<DummyReactionDTO>())
            return dto.DowncastTo<DummyReactionDTO>().ReactionBuilder;

         if (dto.IsAnImplementationOf<DummyParameterDTO>())
            return _context.Get<IObjectBase>((dto.DowncastTo<DummyParameterDTO>()).ParameterToUse.Id);

         return _context.Get<IObjectBase>(dto.Id);
      }

      private IEnumerable<IObjectBaseDTO> getLocalInformationForReaction(IContainer container)
      {
         return _reactions.Select(x => _dummyReactionDTOMapper.MapFrom(x, container));
      }

      private IEnumerable<IObjectBaseDTO> getLocalInformationForMolecules(IContainer container)
      {
         return _molecules.Select(x => _dummyMoleculeDTOMapper.MapFrom(x, container));
      }

      protected SpatialStructureDTO CreateSpatialStuctureDTOFrom(IMoBiSpatialStructure spatialStructure)
      {
         return new SpatialStructureDTO
         {
            Id = spatialStructure.Id,
            Name = spatialStructure.Name,
            Icon = spatialStructure.Icon,
            TopContainer = spatialStructure.TopContainers.MapAllUsing(_containerDTOMapper),
            Neighborhoods = _containerDTOMapper.MapFrom(spatialStructure.NeighborhoodsContainer)
         };
      }
   }
}