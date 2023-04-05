using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferencePresenter : IPresenter<ISelectReferenceView>,
      IListener<RemovedEvent>,
      IListener<AddedEvent>
   {
      void GetLocalisationReferences();

      void Init(IEntity localReferencePoint, IEnumerable<IObjectBase> contextSpecificEntitiesToAddToReferenceTree, IUsingFormula editedObject);

      IEnumerable<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto);
      ReferenceDTO GetReferenceObjectFrom(ObjectBaseDTO objectBaseDTO);
      ObjectPath GetSelection();
      void CheckPathCreationConfiguration();
      Func<IObjectBase, bool> SelectionPredicate { get; set; }
      bool LegalObjectSelected { get; }
      void SelectionChanged(ITreeNode treeNode);
      event Action SelectionChangedEvent;
   }

   public abstract class SelectReferencePresenterBase : AbstractCommandCollectorPresenter<ISelectReferenceView, ISelectReferencePresenter>, ISelectReferencePresenter
   {
      protected readonly IParameterToDummyParameterDTOMapper _dummyParameterDTOMapper;
      protected readonly IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      protected readonly IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      protected readonly IObjectBaseToDummyMoleculeDTOMapper _dummyMoleculeDTOMapper;
      protected readonly IObjectPathFactory _objectPathFactory;
      protected readonly IUserSettings _userSettings;
      protected readonly IMoBiContext _context;
      protected IEntity _refObject;
      private readonly IObjectPathCreator _objectPathCreator;
      private readonly Localisations _localisation;
      private IUsingFormula _editedObject;
      public Func<IObjectBase, bool> SelectionPredicate { get; set; }
      public event Action SelectionChangedEvent = delegate { };

      protected SelectReferencePresenterBase(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToDummyMoleculeDTOMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreator objectPathCreator,
         Localisations localisation)
         : base(view)
      {
         _objectPathCreator = objectPathCreator;
         _localisation = localisation;
         _dummyParameterDTOMapper = dummyParameterDTOMapper;
         _referenceMapper = referenceMapper;
         _dummyMoleculeDTOMapper = objectBaseToDummyMoleculeDTOMapper;
         _userSettings = userSettings;
         _context = context;
         _objectBaseDTOMapper = objectBaseDTOMapper;
         _objectPathFactory = _context.ObjectPathFactory;
         _view.ObjectPathType = _userSettings.ObjectPathType;

         _referenceMapper.Initialize(GetChildObjects);

         SelectionPredicate = parameter => true;
      }

      public virtual void Init(IEntity localReferencePoint, IEnumerable<IObjectBase> contextSpecificEntitiesToAddToReferenceTree, IUsingFormula editedObject)
      {
         if (localReferencePoint != null)
            _refObject = localReferencePoint;

         _view.Show(contextSpecificEntitiesToAddToReferenceTree.MapAllUsing(_referenceMapper));
         addInitialObjects();

         if (_refObject != null)
         {
            _view.Localisation = _objectPathFactory.CreateAbsoluteObjectPath(_refObject).PathAsString;
            selectInView(_refObject);
         }
         _editedObject = editedObject;
      }

      private void addInitialObjects()
      {
         AddSpecificInitialObjects();
      }

      public virtual IEnumerable<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto)
      {
         var children = new List<ObjectBaseDTO>();

         if (_context.ObjectRepository.ContainsObjectWithId(dto.Id))
         {
            var objectBase = _context.Get<IObjectBase>(dto.Id);

            // Distributed Parameter children are only for internal use
            if (objectBase.IsAnImplementationOf<IDistributedParameter>())
               return children;

            addChildrenFromSpatialStructure(children, objectBase as ISpatialStructure);
            addChildrenFromContainer(children, objectBase as IContainer);
            addChildrenFromNeighborhood(children, objectBase as NeighborhoodBuilder);
            addParametersFromParameterContainer(children, objectBase as IContainsParameters);
         }

         else if (dto.IsAnImplementationOf<DummyMoleculeContainerDTO>())
         {
            AddChildrenFromDummyMolecule(children, dto.DowncastTo<DummyMoleculeContainerDTO>());
         }

         return children;
      }

      /// <summary>
      ///    Creates the DTOReference from the chosen dto.
      /// </summary>
      /// <param name="objectBaseDTO">The dto object base for the object to reference</param>
      /// <returns>the usable Dto, null if object can't be referenced </returns>
      public ReferenceDTO GetReferenceObjectFrom(ObjectBaseDTO objectBaseDTO)
      {
         try
         {
            if (string.Equals(objectBaseDTO.Id, AppConstants.Time))
               return timeReference();

            var objectBase = _context.Get<IObjectBase>(objectBaseDTO.Id);
            if(objectBase!=null)
               return createPathsFromEntity(objectBase);

            if (objectBaseDTO.IsAnImplementationOf<DummyParameterDTO>())
               return createPathFromParameterDummy(objectBaseDTO);

            if (objectBaseDTO.IsAnImplementationOf<DummyMoleculeContainerDTO>())
               return _objectPathCreator.CreateMoleculePath((DummyMoleculeContainerDTO) objectBaseDTO, shouldCreateAbsolutePaths, _refObject);

            return null;
         }
         catch (InvalidCastException)
         {
            return null;
         }
      }

      private ReferenceDTO timeReference()
      {
         return new ReferenceDTO
         {
            Path = new TimePath {TimeDimension = _context.DimensionFactory.Dimension(Constants.Dimension.TIME)}
         };
      }

      public ObjectPath GetSelection()
      {
         var selection = getSelected<IEntity>();
         return _objectPathFactory.CreateRelativeObjectPath(_refObject, selection);
      }

      private T getSelected<T>() where T : class, IObjectBase
      {
         var dto = _view.SelectedDTO;
         return dto == null ? null : _context.Get<T>(dto.Id);
      }

      public void CheckPathCreationConfiguration()
      {
         if (_refObject == null && !shouldCreateAbsolutePaths)
         {
            GetLocalisationReferences();
            if (_refObject == null)
            {
               _view.ObjectPathType = ObjectPathType.Absolute;
            }
         }
      }

      public bool LegalObjectSelected
      {
         get
         {
            var para = getSelected<IObjectBase>();
            if (para == null) return false;
            return selectionPredicate(para);
         }
      }

      public void SelectionChanged(ITreeNode treeNode)
      {
         SelectionChangedEvent();
      }

      public virtual void GetLocalisationReferences()
      {
         using (var selectLocalisationPresenter = _context.Resolve<ISelectLocalisationPresenter>())
         {
            var localisation = selectLocalisationPresenter.Select(_localisation);
            if (localisation == null) return;

            _refObject = localisation;
            _view.Localisation = _objectPathFactory.CreateAbsoluteObjectPath(localisation).PathAsString;
            selectInView(_refObject);
         }
      }

      private void selectInView(IEntity entityToSelect)
      {
         if (entityToSelect == null) return;
         if (!_view.Shows(entityToSelect))
         {
            selectInView(entityToSelect.ParentContainer);
         }
         _view.Select(entityToSelect);
      }

      protected abstract void AddSpecificInitialObjects();

      protected void AddSpatialStructures()
      {
         var spatialStructures = _context.CurrentProject.SpatialStructureCollection;
         _view.AddNodes(spatialStructures.MapAllUsing(_referenceMapper));
      }

      private bool shouldCreateAbsolutePaths => _view.ObjectPathType.Equals(ObjectPathType.Absolute);

      protected void AddTimeReference()
      {
         var timeDTO = new ObjectBaseDTO
         {
            Id = AppConstants.Time,
            Name = AppConstants.Time,
            Icon = ApplicationIcons.Time
         };
         _view.AddNode(_referenceMapper.MapFrom(timeDTO));
      }

      private IEnumerable<T> getAllMoleculeChildren<T>(MoBiProject currentProject, DummyMoleculeContainerDTO dummyMolecule) where T : class, IEntity
      {
         IEnumerable<T> children = new List<T>();
         foreach (var moleculeBuildingBlock in currentProject.MoleculeBlockCollection)
         {
            var moleculeBuilder = moleculeBuildingBlock.FindByName(dummyMolecule.Name);
            if (moleculeBuilder == null)
               continue;

            children = moleculeBuilder.GetChildren<T>().Union(children);
            children = children.Distinct(new NameComparer<T>()).Where(selectionPredicate);
         }

         return children.OrderBy(x => x.Name);
      }

      protected virtual void AddChildrenFromDummyMolecule(List<ObjectBaseDTO> children, DummyMoleculeContainerDTO dummyMolecule)
      {
         var parameterDTOs = new List<ObjectBaseDTO>();
         var moleculePropertiesContainer = _context.Get<IContainer>(dummyMolecule.MoleculePropertiesContainer.Id);

         parameterDTOs.AddRange(moleculePropertiesContainer.GetChildren<IParameter>()
            .Where(selectionPredicate)
            .Select(x => _dummyParameterDTOMapper.MapFrom(x, moleculePropertiesContainer, dummyMolecule)));

         if (isGlobalMoleculePropertiesContainer(moleculePropertiesContainer))
         {
            //add Child Container
            children.AddRange(getAllMoleculeChildren<TransporterMoleculeContainer>(_context.CurrentProject, dummyMolecule)
               .MapAllUsing(_objectBaseDTOMapper));

            children.AddRange(getAllMoleculeChildren<InteractionContainer>(_context.CurrentProject, dummyMolecule)
               .MapAllUsing(_objectBaseDTOMapper));

            //add global Molecule Properties defined in Molecules BB because the molecule represents a global molecule container
            parameterDTOs.AddRange(getAllMoleculeChildren<IParameter>(_context.CurrentProject, dummyMolecule)
               .Where(para => para.BuildMode != ParameterBuildMode.Local)
               .Select(x => _dummyParameterDTOMapper.MapFrom(x, moleculePropertiesContainer, dummyMolecule)));
         }
         else
         {
            //This is a local molecule container . We add local Molecules Properties defined in Molecule BB only if container is defined in a physical cotnainer
            if (moleculePropertiesContainer.ParentContainer.Mode == ContainerMode.Physical)
            {
               parameterDTOs.AddRange(getAllMoleculeChildren<IParameter>(_context.CurrentProject, dummyMolecule)
                  .Where(para => para.BuildMode == ParameterBuildMode.Local)
                  .Select(x => _dummyParameterDTOMapper.MapFrom(x, moleculePropertiesContainer, dummyMolecule)));
            }
         }

         children.AddRange(parameterDTOs.OrderBy(x => x.Name));
      }

      private bool isGlobalMoleculePropertiesContainer(IContainer moleculePropertiesContainer)
      {
         return moleculePropertiesContainer.ParentContainer == null ||
                moleculePropertiesContainer.ParentContainer.IsAnImplementationOf<IMoleculeBuilder>();
      }

      private bool selectionPredicate<T>(T objectBase) where T : IObjectBase
      {
         if (SelectionPredicate == null)
            return true;

         return SelectionPredicate(objectBase);
      }

      private void addParametersFromParameterContainer(List<ObjectBaseDTO> children, IContainsParameters parameterContainer)
      {
         if (parameterContainer == null)
            return;

         children.AddRange(parameterContainer.Parameters
            .OrderBy(x => x.Name)
            .MapAllUsing(_objectBaseDTOMapper));
      }

      private void addChildrenFromNeighborhood(List<ObjectBaseDTO> children, NeighborhoodBuilder neighborhood)
      {
         if (neighborhood == null)
            return;

         var firstContainer = neighborhood.FirstNeighbor;
         var secondContainer = neighborhood.SecondNeighbor;

         if (firstContainer == null || secondContainer == null)
            return;

         if (!Equals(firstContainer.ParentContainer, secondContainer.ParentContainer))
         {
            firstContainer = firstContainer.ParentContainer;
            secondContainer = secondContainer.ParentContainer;
         }

         children.Add(_objectBaseDTOMapper.MapFrom(firstContainer));
         children.Add(_objectBaseDTOMapper.MapFrom(secondContainer));
      }

      private void addChildrenFromContainer(List<ObjectBaseDTO> children, IContainer container)
      {
         if (container == null)
            return;

         if (container.IsNamed(Constants.MOLECULE_PROPERTIES))
         {
            //Improve a "generic" Moelcule Layer
            var molecules = allMolecules
               .Distinct(new NameComparer<IMoleculeBuilder>())
               .ToEnumerable<IMoleculeBuilder, IObjectBase>();
            _dummyMoleculeDTOMapper.Initialise(container);
            children.AddRange(molecules.MapAllUsing(_dummyMoleculeDTOMapper));
         }
         else
         {
            // first containers
            children.AddRange(
               container.GetChildrenSortedByName<IContainer>()
                  .Where(c => !c.IsAnImplementationOf<IParameter>())
                  .Where(c => !c.Name.Equals(Constants.MOLECULE_PROPERTIES))
                  .MapAllUsing(_objectBaseDTOMapper));

            //Molecules
            addChildrenFromContainer(children, container.Container(Constants.MOLECULE_PROPERTIES));

            //then parameters
            children.AddRange(container.GetChildrenSortedByName<IParameter>(selectionPredicate)
               .MapAllUsing(_objectBaseDTOMapper));
         }
      }

      private void addChildrenFromSpatialStructure(List<ObjectBaseDTO> children, ISpatialStructure spatialStructure)
      {
         if (spatialStructure == null)
            return;

         children.AddRange(
            spatialStructure.TopContainers.ToEnumerable<IContainer, IObjectBase>()
               .OrderBy(x => x.Name)
               .MapAllUsing(_objectBaseDTOMapper));

         children.Add(_objectBaseDTOMapper.MapFrom(spatialStructure.NeighborhoodsContainer));
         addChildrenFromContainer(children, spatialStructure.GlobalMoleculeDependentProperties);
      }

      private ReferenceDTO createPathFromParameterDummy(ObjectBaseDTO dtoObjectBase)
      {
         return _objectPathCreator.CreatePathFromParameterDummy(dtoObjectBase, shouldCreateAbsolutePaths, _refObject, _editedObject);
      }

      private ReferenceDTO createPathsFromEntity(IObjectBase entity)
      {
         return _objectPathCreator.CreatePathsFromEntity(entity, shouldCreateAbsolutePaths, _refObject, _editedObject);
      }

      private IEnumerable<IMoleculeBuilder> allMolecules
      {
         get
         {
            var moleculeBuildingBlocks = _context.CurrentProject.MoleculeBlockCollection;
            return moleculeBuildingBlocks.SelectMany(x => x).OrderBy(x => x.Name);
         }
      }

      protected void AddReactions()
      {
         _view.AddNodes(getReactions.MapAllUsing(_referenceMapper));
      }

      private IEnumerable<IReactionBuilder> getReactions
      {
         get { return _context.CurrentProject.ReactionBlockCollection.SelectMany(x => x).OrderBy(x => x.Name); }
      }

      protected void AddMolecule()
      {
         var dummyMolecule = new DummyParameterDTO(null) {Name = ObjectPathKeywords.MOLECULE, Id = ObjectPathKeywords.MOLECULE};
         _view.AddNode(_referenceMapper.MapFrom(dummyMolecule));
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         foreach (var removedObject in eventToHandle.RemovedObjects)
         {
            if (_view.Shows(removedObject))
            {
               _view.Remove(removedObject);
            }
         }
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_view.Shows(eventToHandle.Parent))
         {
            var parentNodes = _view.GetNodes(eventToHandle.Parent);
            parentNodes.Each(parentNode =>
            {
               var node = _referenceMapper.MapFrom(eventToHandle.AddedObject);
               parentNode.AddChild(node);
               _view.AddNode(node);
            });
         }
      }
   }
}