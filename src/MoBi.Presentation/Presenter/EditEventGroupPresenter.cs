using System;
using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
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
   public interface IEditEventGroupPresenter : IEditPresenterWithParameters<IEventGroupBuilder>, ICanEditPropertiesPresenter, IPresenterWithFormulaCache
   {
   }

   internal class EditEventGroupPresenter : AbstractEntityEditPresenter<IEditEventGroupView, IEditEventGroupPresenter, IEventGroupBuilder>, IEditEventGroupPresenter
   {
      private IEventGroupBuilder _eventGroupBuilder;
      private readonly IEditTaskFor<IEventGroupBuilder> _editTask;
      private readonly IEventGroupBuilderToDTOEventGroupBuilderMapper _eventGroupBuilderToDTOEventGroupBuilderMapper;
      private readonly IMoBiContext _context;
      private readonly IDescriptorConditionListPresenter<IEventGroupBuilder> _descriptorConditionListPresenter;
      private readonly IEditParameterListPresenter _parameterListPresenter;
      private IBuildingBlock _buildingBlock;

      public EditEventGroupPresenter(IEditEventGroupView view, IEditTaskFor<IEventGroupBuilder> editTask, IEditParameterListPresenter parameterListPresenter,
         IEventGroupBuilderToDTOEventGroupBuilderMapper eventGroupBuilderToDTOEventGroupBuilderMapper, IMoBiContext context,
         IDescriptorConditionListPresenter<IEventGroupBuilder> descriptorConditionListPresenter)
         : base(view)
      {
         _descriptorConditionListPresenter = descriptorConditionListPresenter;
         _context = context;
         _eventGroupBuilderToDTOEventGroupBuilderMapper = eventGroupBuilderToDTOEventGroupBuilderMapper;
         _parameterListPresenter = parameterListPresenter;
         _view.AddParametersView(parameterListPresenter.BaseView);
         _view.AddDescriptorConditionListView(_descriptorConditionListPresenter.View);
         _editTask = editTask;
         AddSubPresenters(_parameterListPresenter, _descriptorConditionListPresenter);
      }

      public override void Edit(IEventGroupBuilder eventGroupBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _eventGroupBuilder = eventGroupBuilder;
         _parameterListPresenter.Edit(eventGroupBuilder);
         _view.EnableDescriptors = eventGroupBuilder.ParentContainer == null;
         var dto = _eventGroupBuilderToDTOEventGroupBuilderMapper.MapFrom(_eventGroupBuilder);
         dto.AddUsedNames(_editTask.GetForbiddenNamesWithoutSelf(eventGroupBuilder, existingObjectsInParent));
         _view.BindTo(dto);
         _descriptorConditionListPresenter.Edit(eventGroupBuilder,x => x.SourceCriteria, _buildingBlock);
      }

      public void SelectParameter(IParameter parameter)
      {
         _view.ShowParameters();
         _parameterListPresenter.Select(parameter);
      }

      public override object Subject => _eventGroupBuilder;

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _eventGroupBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTask.Rename(_eventGroupBuilder, BuildingBlock);
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         //TODO probably interface IPresenterWithFormulaCache should be split into two if this method  is not required here
         throw new NotSupportedException();
      }

      public IBuildingBlock BuildingBlock
      {
         get { return _buildingBlock; }
         set
         {
            _buildingBlock = value;
            _parameterListPresenter.BuildingBlock = value;
         }
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;
   }
}