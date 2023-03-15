using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility;

namespace MoBi.Presentation.Presenter
{
   public interface IEditContainerPresenter : ICanEditPropertiesPresenter, IPresenterWithFormulaCache, IEditPresenterWithParameters<IContainer>
   {
      EditParameterMode EditMode { set; }
      bool ReadOnly { set; }
      void SetInitialName(string initialName);
      void SetParentPath(string parentPath);
      void UpdateParentPath();
      string ContainerModeDisplayFor(ContainerMode mode);
      IReadOnlyList<ContainerMode> AllContainerModes { get; }
      void SetContainerMode(ContainerMode newContainerMode);
      IReadOnlyList<ContainerType> AllContainerTypes { get; }
   }

   public class EditContainerPresenter : AbstractEntityEditPresenter<IEditContainerView, IEditContainerPresenter, IContainer>, IEditContainerPresenter
   {
      private IContainer _container;
      private readonly IContainerToContainerDTOMapper _containerMapper;
      private readonly IEditTaskForContainer _editTasks;
      private ContainerDTO _containerDTO;
      private readonly IMoBiContext _context;
      private readonly ITagsPresenter _tagsPresenter;
      private readonly IApplicationController _applicationController;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;

      public EditContainerPresenter(
         IEditContainerView view,
         IContainerToContainerDTOMapper containerMapper,
         IEditTaskForContainer editTasks,
         IEditParametersInContainerPresenter editParametersInContainerPresenter,
         IMoBiContext context,
         ITagsPresenter tagsPresenter,
         IApplicationController applicationController)
         : base(view)
      {
         _containerMapper = containerMapper;
         _context = context;
         _tagsPresenter = tagsPresenter;
         _applicationController = applicationController;
         _editParametersInContainerPresenter = editParametersInContainerPresenter;
         _editTasks = editTasks;
         _view.AddParameterView(editParametersInContainerPresenter.BaseView);
         _view.AddTagsView(_tagsPresenter.BaseView);
         AddSubPresenters(_editParametersInContainerPresenter, _tagsPresenter);
         initParameterListPresenter();
      }

      private void initParameterListPresenter()
      {
         _editParametersInContainerPresenter.BlackBoxAllowed = true;
         _editParametersInContainerPresenter.ChangeLocalisationAllowed = false;
      }

      public void UpdateParentPath()
      {
         using (var presenter = _applicationController.Start<ISelectContainerPresenter>())
         {
            var objectPath = presenter.Select();
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

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _container, BuildingBlock).Run(_context));
      }

      public void SetParentPath(string parentPath)
      {
         var objectPath = new ObjectPath(parentPath.ToPathArray());
         AddCommand(new SetParentPathCommand(_container, objectPath, BuildingBlock).Run(_context));

         //rebind the view
         _containerDTO.ParentPath = parentPath;
         _view.BindTo(_containerDTO);
      }

      public void SetContainerMode(ContainerMode newContainerMode)
      {
         AddCommand(_editTasks.SetContainerMode(BuildingBlock, _container, newContainerMode));
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

      public IBuildingBlock BuildingBlock
      {
         get => _editParametersInContainerPresenter.BuildingBlock;
         set
         {
            _editParametersInContainerPresenter.BuildingBlock = value;
            _tagsPresenter.BuildingBlock = value;
         }
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void SelectParameter(IParameter childParameter)
      {
         _view.ShowParameters();
         _editParametersInContainerPresenter.Select(childParameter);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return _editParametersInContainerPresenter.GetFormulas();
      }

      public void RenameSubject()
      {
         _editTasks.Rename(_container, _container.ParentContainer, BuildingBlock);
      }

      public override void Edit(IContainer container, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _container = container;
         _containerDTO = _containerMapper.MapFrom(_container);
         _containerDTO.AddUsedNames(_editTasks.GetForbiddenNamesWithoutSelf(container, existingObjectsInParent));
         _editParametersInContainerPresenter.Edit(_container);
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

      public void SetInitialName(string initialName)
      {
         SetPropertyValueFromView(_container.PropertyName(x => x.Name), initialName, string.Empty);
         _containerDTO.Name = initialName;
      }
   }
}