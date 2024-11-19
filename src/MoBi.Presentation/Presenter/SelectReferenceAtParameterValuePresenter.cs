using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtParameterValuePresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtParameterValuePresenter : SelectReferencePresenterBase, ISelectReferenceAtParameterValuePresenter
   {
      private readonly Cache<IBuildingBlock, IContainer> _containers;

      public SelectReferenceAtParameterValuePresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreatorAtParameter objectPathCreator,
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly, buildingBlockRepository)
      {
         view.EnableMultiSelect = true;
         _containers = new Cache<IBuildingBlock, IContainer>();
      }

      public override IEnumerable<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto)
      {
         var children = base.GetChildObjects(dto).ToList();

         var container = containerFrom(dto);
         if (_context.ObjectRepository.ContainsObjectWithId(dto.Id))
         {
            addExpressionChildren(dto, children);
            addIndividualChildren(dto, children);
         }
         else
         {
            container?.Children.Each(x => children.Add(_objectBaseDTOMapper.MapFrom(x)));
         }

         return children;
      }

      public override IReadOnlyList<ObjectPath> GetAllSelections()
      {
         var selectedItems = base.GetAllSelections();
         replaceMoleculePropertiesWithModelParentName(selectedItems);
         return selectedItems;
      }

      private void replaceMoleculePropertiesWithModelParentName(IReadOnlyList<ObjectPath> selectedItems)
      {
         var parentName = getParentNameFromSelection();
         if (parentName != string.Empty)
         {
            foreach (var item in selectedItems.Where(x => x.PathAsString.Contains(Constants.MOLECULE_PROPERTIES)))
            {
               item.ReplaceWith(item.PathAsString.Replace(Constants.MOLECULE_PROPERTIES, parentName).ToPathArray());
            }
         }
      }

      private string getParentNameFromSelection() =>
         _view.AllSelectedDTOs
            .OfType<DummyParameterDTO>()
            .FirstOrDefault()?.ModelParentName ?? string.Empty;

      private IContainer containerFrom(ObjectBaseDTO dto)
      {
         if (dto.ObjectBase is IContainer container && _containers.Any(x => x.GetAllContainersAndSelf<IContainer>().Contains(container)))
            return container;

         return null;
      }

      private void addIndividualChildren(ObjectBaseDTO dto, List<ObjectBaseDTO> children)
      {
         var buildingBlock = _context.Get<IndividualBuildingBlock>(dto.Id);
         addChildrenFromPathAndValueBuildingBlock<IndividualBuildingBlock, IndividualParameter>(children, buildingBlock);
      }

      private void addExpressionChildren(ObjectBaseDTO dto, List<ObjectBaseDTO> children)
      {
         var buildingBlock = _context.Get<ExpressionProfileBuildingBlock>(dto.Id);
         // We need the TBuilder type because ExpressionProfileBuildingBlock contains two types of PathAndValueEntity, and we only want to map ExpressionParameters
         addChildrenFromPathAndValueBuildingBlock<ExpressionProfileBuildingBlock, ExpressionParameter>(children, buildingBlock);
      }

      private void addChildrenFromPathAndValueBuildingBlock<TBuildingBlock, TEntity>(List<ObjectBaseDTO> children, TBuildingBlock pathAndValueEntities) where TBuildingBlock : PathAndValueEntityBuildingBlock<TEntity> where TEntity : PathAndValueEntity
      {
         if (pathAndValueEntities == null)
            return;

         _containers[pathAndValueEntities] = getGroups<TBuildingBlock, TEntity>(pathAndValueEntities);

         pathAndValueEntities.Where(x => x.DistributionType == null).Each(x => addToContainer(x, _containers[pathAndValueEntities]));

         addPathAndValuesFromContainer(children, _containers[pathAndValueEntities]);
      }

      private void addPathAndValuesFromContainer(List<ObjectBaseDTO> children, IContainer container)
      {
         // first containers
         children.AddRange(
            container.GetChildrenSortedByName<IContainer>()
               .MapAllUsing(_objectBaseDTOMapper));

         //then parameters
         children.AddRange(container.GetChildrenSortedByName<PathAndValueEntity>()
            .MapAllUsing(_objectBaseDTOMapper));
      }

      private static void addToContainer(PathAndValueEntity x, IContainer container)
      {
         var parentContainer = x.ContainerPath.TryResolve<IContainer>(container);
         if (parentContainer != null)
         {
            parentContainer.Add(x);
         }
      }

      private IContainer getGroups<TBuildingBlock, TEntity>(TBuildingBlock pathAndValueEntities) where TBuildingBlock : PathAndValueEntityBuildingBlock<TEntity> where TEntity : PathAndValueEntity
      {
         var rootContainer = new Container();

         // construct a new object path to avoid changing the original object path
         pathAndValueEntities.Select(x => new ObjectPath(x.ContainerPath)).ToList().Where(x => x.Any()).GroupBy(x => x.First()).Each(x => addContainersFor(x, rootContainer));

         return rootContainer;
      }

      private void addContainersFor(IGrouping<string, ObjectPath> group, Container rootContainer)
      {
         if (string.IsNullOrEmpty(group.Key))
            return;

         var groupContainer = new Container().WithName(group.Key);
         group.Each(x => x.RemoveFirst());
         group.Where(x => x.Any()).GroupBy(x => x.First()).Each(x => addContainersFor(x, groupContainer));
         rootContainer.Add(groupContainer);
      }

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();
         addIndividuals();
         addExpressions();
         _view.ChangeLocalisationAllowed = false;
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