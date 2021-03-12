using System;
using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Presenter
{
   public interface IEditContainerPresenter : ICanEditPropertiesPresenter, IPresenterWithFormulaCache, IEditPresenterWithParameters<IContainer>
   {
      EditParameterMode EditMode { set; }
      bool ReadOnly { set; }
      void SetInitialName(string initialName);
      string ContainerModeDisplayFor(ContainerMode mode);
      IEnumerable<ContainerMode> AllContainerModes();
      void SetContainerMode(ContainerMode newContainerMode);
      IEnumerable<ContainerType> AllContainerTypes();
   }

   public class EditContainerPresenter : AbstractEntityEditPresenter<IEditContainerView, IEditContainerPresenter, IContainer>, IEditContainerPresenter
   {
      private IContainer _container;
      private readonly IContainerToContainerDTOMapper _containerToDTOContainerMapper;
      private readonly IEditTaskForContainer _editTasks;
      private ContainerDTO _containerDTO;
      private readonly IMoBiContext _context;
      private readonly ITagsPresenter _tagsPresenter;
      private readonly IEditParametersInContainerPresenter _editParametersInContainerPresenter;

      public EditContainerPresenter(
         IEditContainerView view, 
         IContainerToContainerDTOMapper containerToDtoContainerMapper, 
         IEditTaskForContainer editTasks,
         IEditParametersInContainerPresenter editParametersInContainerPresenter, 
         IMoBiContext context, 
         ITagsPresenter tagsPresenter)
         : base(view)
      {
         _containerToDTOContainerMapper = containerToDtoContainerMapper;
         _context = context;
         _tagsPresenter = tagsPresenter;
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

      public string ContainerModeDisplayFor(ContainerMode mode)
      {
         switch (mode)
         {
            case ContainerMode.Physical:
               return $"{mode} {ToolTips.Container.PhysicalContainer}";
            case ContainerMode.Logical:
               return $"{mode} {ToolTips.Container.LogicalContainer}";
            default:
               throw new ArgumentOutOfRangeException("mode");
         }
      }

      public IEnumerable<ContainerMode> AllContainerModes()
      {
         return EnumHelper.AllValuesFor<ContainerMode>();
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _container, BuildingBlock).Run(_context));
      }

      public void SetContainerMode(ContainerMode newContainerMode)
      {
         AddCommand(_editTasks.SetContainerMode(BuildingBlock, _container, newContainerMode));
      }

      public IEnumerable<ContainerType> AllContainerTypes()
      {
         yield return ContainerType.Application;
         yield return ContainerType.Compartment;
         yield return ContainerType.Event;
         yield return ContainerType.EventGroup;
         yield return ContainerType.Model;
         yield return ContainerType.Neighborhood;
         yield return ContainerType.Organ;
         yield return ContainerType.Other;
         yield return ContainerType.Organism;
      }

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
         _containerDTO = _containerToDTOContainerMapper.MapFrom(_container);
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