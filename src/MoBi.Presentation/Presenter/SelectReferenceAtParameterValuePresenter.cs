using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtParameterValuePresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtParameterValuePresenter : SelectReferencePresenterBase, ISelectReferenceAtParameterValuePresenter
   {
      private readonly IPathAndValueContainerizingTask _containerizingTask;
      private readonly Cache<IBuildingBlock, IContainer> _pathAndValueContainers = new Cache<IBuildingBlock, IContainer>();

      public SelectReferenceAtParameterValuePresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreatorAtParameter objectPathCreator,
         IBuildingBlockRepository buildingBlockRepository,
         IPathAndValueContainerizingTask containerizingTask)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly, buildingBlockRepository)
      {
         _containerizingTask = containerizingTask;
         view.EnableMultiSelect = true;
         SelectionPredicate = filterObjects;
      }

      private bool filterObjects(IObjectBase objectBase)
      {
         if (objectBase is IParameter parameter && parameter.ParentContainer is ReactionBuilder)
            return parameter.BuildMode == ParameterBuildMode.Global;

         return true;
      }

      public override IEnumerable<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto)
      {
         var children = base.GetChildObjects(dto).ToList();

         if (_context.ObjectRepository.ContainsObjectWithId(dto.Id))
         {
            addExpressionChildren(dto, children);
            addIndividualChildren(dto, children);
            addEventsChildren(dto, children);
         }
         else if (dto.ObjectBase is IContainer container && _containerizingTask.IsInCachedTree(container, _pathAndValueContainers))
         {
            children.AddRange(_containerizingTask.ChildrenFor(container, _pathAndValueContainers).MapAllUsing(_objectBaseDTOMapper));
         }

         return children;
      }

      private void addEventsChildren(ObjectBaseDTO dto, List<ObjectBaseDTO> children)
      {
         var buildingBlock = _context.Get<EventGroupBuildingBlock>(dto.Id);
         addChildrenFromEventBuildingBlock(children, buildingBlock);
      }

      private void addChildrenFromEventBuildingBlock(List<ObjectBaseDTO> children, EventGroupBuildingBlock eventGroupBuildingBlock)
      {
         if (eventGroupBuildingBlock == null)
            return;

         children.AddRange(eventGroupBuildingBlock.MapAllUsing(_objectBaseDTOMapper));
      }

      public override IReadOnlyList<ObjectPath> GetAllSelections()
      {
         var selectedItems = GetAllSelected<IEntity>().ToList();

         var selectedDummyDtos = _view.AllSelectedDTOs
            .OfType<DummyParameterDTO>()
            .ToList();

         return createObjectPathsForSelection(selectedItems, selectedDummyDtos);
      }

      private IReadOnlyList<ObjectPath> createObjectPathsForSelection(
         IReadOnlyList<IEntity> selectedItems,
         IReadOnlyList<DummyParameterDTO> selectedParameterDtos)
      {
         var newItems = new List<ObjectPath>();
         foreach (var item in selectedItems)
         {
            var matchingDto = selectedParameterDtos
               .FirstOrDefault(dto => dto.Id == item.Id);

            var itemAsPath = getObjectPath(item, matchingDto);

            if (itemAsPath.PathAsString.Contains(Constants.MOLECULE_PROPERTIES))
            {
               if (matchingDto != null && !string.IsNullOrEmpty(matchingDto.ModelParentName))
               {
                  itemAsPath.ReplaceWith(itemAsPath.PathAsString
                     .Replace(Constants.MOLECULE_PROPERTIES, matchingDto.ModelParentName)
                     .ToPathArray());
               }
            }

            newItems.Add(itemAsPath);
         }

         return newItems;
      }

      private ObjectPath getObjectPath(IEntity item, DummyParameterDTO matchingDto)
      {
         if (shouldUseParameterPath(item, matchingDto))
         {
            var returnPath = _objectPathFactory.CreateAbsoluteObjectPath(matchingDto.Parent);
            returnPath.Add(matchingDto.Name);
            return returnPath;
         }

         return CreatePathFor(item);
      }

      private bool shouldUseParameterPath(IEntity item, DummyParameterDTO matchingDto)
      {
         return item is Parameter itemParam
                && matchingDto != null
                && itemParam.BuildMode == ParameterBuildMode.Local;
      }

      private void addIndividualChildren(ObjectBaseDTO dto, List<ObjectBaseDTO> children)
      {
         var buildingBlock = _context.Get<IndividualBuildingBlock>(dto.Id);
         if (buildingBlock == null)
            return;
         children.AddRange(_containerizingTask.ChildrenFor<IndividualBuildingBlock, IndividualParameter>(buildingBlock, _pathAndValueContainers).MapAllUsing(_objectBaseDTOMapper));
      }

      private void addExpressionChildren(ObjectBaseDTO dto, List<ObjectBaseDTO> children)
      {
         var buildingBlock = _context.Get<ExpressionProfileBuildingBlock>(dto.Id);
         if (buildingBlock == null)
            return;
         // We need the TBuilder type because ExpressionProfileBuildingBlock contains two types of PathAndValueEntity, and we only want to map ExpressionParameters
         children.AddRange(_containerizingTask.ChildrenFor<ExpressionProfileBuildingBlock, ExpressionParameter>(buildingBlock, _pathAndValueContainers).MapAllUsing(_objectBaseDTOMapper));
      }

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();
         addIndividuals();
         addExpressions();
         addEvents();
         AddReactions();
         _view.ChangeLocalisationAllowed = true;
      }

      private void addEvents()
      {
         var events = _buildingBlockRepository.EventBlockCollection;
         var nodes = events.Select(x => _referenceMapper.MapFrom(x));

         View.AddNodes(nodes);
      }

      private void addExpressions()
      {
         var expressions = _buildingBlockRepository.ExpressionProfileCollection;
         var nodes = expressions.Select(x => _referenceMapper.MapFrom(x));

         View.AddNodes(nodes);
      }

      private void addIndividuals()
      {
         var individuals = _buildingBlockRepository.IndividualsCollection;
         var nodes = individuals.Select(x => _referenceMapper.MapFrom(x)).ToList();

         View.AddNodes(nodes);
      }

      public override void SelectionChanged()
      {
         base.SelectionChanged();
         OnStatusChanged(null, null);
      }

      public override bool CanClose => isSelectionParameterType();

      private bool isSelectionParameterType()
      {
         var allSelected = GetAllSelected<IObjectBase>().ToList();
         var allSelectedAreParameters = allSelected.All(isSelectable);
         return allSelectedAreParameters && allSelected.Any();
      }

      private static bool isSelectable(IObjectBase objectBase)
      {
         return objectBase is IParameter || objectBase is PathAndValueEntity;
      }
   }
}