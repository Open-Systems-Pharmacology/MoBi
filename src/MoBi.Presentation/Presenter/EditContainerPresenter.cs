using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.Presentation.Presenter
{
   public interface IEditContainerPresenter : ICanEditPropertiesPresenter, IPresenterWithFormulaCache, IEditPresenterWithParameters<IContainer>
   {
      EditParameterMode EditMode { set; }
      bool ReadOnly { set; }
      void SetName(string name);
      void SetParentPath(string parentPath);
      void UpdateParentPath();
      string ContainerModeDisplayFor(ContainerMode mode);
      IReadOnlyList<ContainerMode> AllContainerModes { get; }
      bool ConfirmAndSetContainerMode(ContainerMode newContainerMode);
      IReadOnlyList<ContainerType> AllContainerTypes { get; }
   }

   public class EditContainerPresenter : AbstractContainerEditPresenterWithParameters<IEditContainerView, IEditContainerPresenter, IContainer>, IEditContainerPresenter
   {
      private IContainer _container;
      private readonly IContainerToContainerDTOMapper _containerMapper;
      private readonly IEditTaskForContainer _editTasks;
      private ContainerDTO _containerDTO;
      private readonly ITagsPresenter _tagsPresenter;
      private readonly IApplicationController _applicationController;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IDialogCreator _dialogCreator;
      private bool _isNewEntity;
      private IInteractionTasksForSpatialStructure _interactionTasksForSpatialStructure;

      public EditContainerPresenter(
         IEditContainerView view,
         IContainerToContainerDTOMapper containerMapper,
         IEditTaskForContainer editTasks,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         IMoBiContext context,
         ITagsPresenter tagsPresenter,
         IApplicationController applicationController,
         IObjectPathFactory objectPathFactory,
         IDialogCreator dialogCreator,
         IInteractionTasksForSpatialStructure interactionTasksForSpatialStructure)
         : base(view, editParametersInContainerPresenter, context, editTasks)
      {
         _dialogCreator = dialogCreator;
         _containerMapper = containerMapper;
         _tagsPresenter = tagsPresenter;
         _applicationController = applicationController;
         _editTasks = editTasks;
         _objectPathFactory = objectPathFactory;
         _interactionTasksForSpatialStructure = interactionTasksForSpatialStructure;
         _view.AddParameterView(editParametersInContainerPresenter.BaseView);
         _view.AddTagsView(_tagsPresenter.BaseView);
         AddSubPresenters(_tagsPresenter);
      }

      public void UpdateParentPath()
      {
         using (var presenter = _applicationController.Start<ISelectContainerPresenter>())
         {
            var objectPath = presenter.Select(_objectPathFactory.CreateAbsoluteObjectPath(_container));
            if (objectPath == null) return;
            SetParentPath(objectPath);
         }
      }

      public string ContainerModeDisplayFor(ContainerMode mode)
      {
         switch (mode)
         {
            case ContainerMode.Physical:
               return $"{mode} {ToolTips.Container.PhysicalContainer}";
            case ContainerMode.Logical:
               return $"{mode} {ToolTips.Container.LogicalContainer}";
            default:
               throw new ArgumentOutOfRangeException(nameof(mode));
         }
      }

      public IReadOnlyList<ContainerMode> AllContainerModes => EnumHelper.AllValuesFor<ContainerMode>().ToList();

      public void SetParentPath(string parentPath)
      {
         var objectPath = new ObjectPath(parentPath.ToPathArray());
         AddCommand(new SetParentPathCommand(_container, objectPath, BuildingBlock).RunCommand(_context));

         //rebind the view
         _containerDTO.ParentPath = parentPath;
         _view.BindTo(_containerDTO);
      }

      public bool ConfirmAndSetContainerMode(ContainerMode newContainerMode)
      {
         if (_isNewEntity)
         {
            _container.Mode = newContainerMode;
            return true;
         }

         if (newContainerMode == ContainerMode.Logical)
         {
            var ans = _dialogCreator.MessageBoxYesNo("This action will remove all MoleculeProperties of this container, are you sure?");
            if (ans == ViewResult.No)
               return false;
         }

         var oldContainerMode = _container.Mode;

         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = ObjectTypes.Container,
            Description = AppConstants.Commands.EditDescription(ObjectTypes.Container, AppConstants.Captions.ContainerMode, oldContainerMode.ToString(), newContainerMode.ToString(), _container.Name),
            CommandType = AppConstants.Commands.UpdateCommand,
         };

         macroCommand.Add(_editTasks.SetContainerMode(BuildingBlock, _container, newContainerMode));

         if (newContainerMode == ContainerMode.Logical)
         {
            var moleculeProperties = GetMoleculePropertiesForContainer(_container);

            if (moleculeProperties.Any())
               macroCommand.Add(new RemoveContainerFromSpatialStructureCommand(_container, moleculeProperties.FirstOrDefault(), (MoBiSpatialStructure)BuildingBlock).RunCommand(_context));
         }
         else
         {
            var moleculeProperties = _context.Create<IContainer>()
               .WithName(Constants.MOLECULE_PROPERTIES)
               .WithMode(ContainerMode.Logical);

            macroCommand.Add(new AddContainerToSpatialStructureCommand(_container, moleculeProperties, (MoBiSpatialStructure)BuildingBlock).RunCommand(_context));
         }

         AddCommand(macroCommand);
         return true;
      }

      public IReadOnlyList<ContainerType> AllContainerTypes { get; } = new[]
      {
         ContainerType.Application,
         ContainerType.Compartment,
         ContainerType.Event,
         ContainerType.EventGroup,
         ContainerType.Model,
         ContainerType.Neighborhood,
         ContainerType.Organ,
         ContainerType.Other,
         ContainerType.Organism,
      };

      public override IBuildingBlock BuildingBlock
      {
         set
         {
            base.BuildingBlock = value;
            _tagsPresenter.BuildingBlock = value;
         }
      }

      protected override IContainer SubjectContainer => _container;

      public override void Edit(IContainer container, IReadOnlyList<IObjectBase> existingObjectsInParent)
      {
         _container = container;
         //an unnamed container means it is being created now, since the name is mandatory over the creation.
         _isNewEntity = string.IsNullOrEmpty(_container.Name);
         base.Edit(container, existingObjectsInParent);
         _containerDTO = _containerMapper.MapFrom(_container);
         _containerDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(container, existingObjectsInParent));
         _view.BindTo(_containerDTO);
         _tagsPresenter.Edit(container);
         _view.ContainerPropertiesEditable = !container.IsMoleculeProperties();
      }

      public override object Subject => _container;

      public EditParameterMode EditMode
      {
         set => _editParametersInContainerPresenter.EditMode = value;
      }

      public bool ReadOnly
      {
         set => _view.ReadOnly = value;
      }

      public void SetName(string name)
      {
         SetPropertyValueFromView(_container.PropertyName(x => x.Name), name, string.Empty);
         _containerDTO.Name = name;
      }
   }
}