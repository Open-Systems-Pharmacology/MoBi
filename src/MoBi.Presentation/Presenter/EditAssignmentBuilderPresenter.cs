using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditAssignmentBuilderPresenter : IEditPresenter<IEventAssignmentBuilder>,
      IPresenter<IEditEventAssignmentBuilderView>,
      IPresenterWithFormulaCache,
      ICanEditPropertiesPresenter,
      ICreatePresenter<IEventAssignmentBuilder>
   {
      void SelectPath();
   }

   public class EditAssignmentBuilderPresenter : AbstractEntityEditPresenter<IEditEventAssignmentBuilderView, IEditAssignmentBuilderPresenter, IEventAssignmentBuilder>, IEditAssignmentBuilderPresenter
   {
      private IEventAssignmentBuilder _eventAssignmentBuilder;
      private readonly IEventAssignmentBuilderToDTOEventAssignmentMapper _eventAssingnmentToDTOAssignmentMapper;
      private readonly IEditTaskFor<IEventAssignmentBuilder> _editTasksForAssignment;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IEditFormulaPresenter _editFormulaPresenter;
      private readonly IMoBiContext _context;
      private readonly ISelectReferenceAtEventAssignmentPresenter _selectReferencePresenter;
      private readonly IContextSpecificReferencesRetriever _contextSpecificReferencesRetriever;
      private readonly IMoBiApplicationController _applicationController;
      public IBuildingBlock BuildingBlock { get; set; }

      public EditAssignmentBuilderPresenter(IEditEventAssignmentBuilderView view, IEventAssignmentBuilderToDTOEventAssignmentMapper eventAssingnmentToDTOAssignmentMapper,
         IEditTaskFor<IEventAssignmentBuilder> editTasksForAssignment, IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper,
         IEditFormulaPresenter editFormulaPresenter, IMoBiContext context,
         ISelectReferenceAtEventAssignmentPresenter selectReferencePresenter,
         IContextSpecificReferencesRetriever contextSpecificReferencesRetriever, IMoBiApplicationController applicationController) : base(view)
      {
         _contextSpecificReferencesRetriever = contextSpecificReferencesRetriever;
         _applicationController = applicationController;
         _selectReferencePresenter = selectReferencePresenter;
         _context = context;
         _editFormulaPresenter = editFormulaPresenter;
         _editFormulaPresenter.ReferencePresenter = _selectReferencePresenter;
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _editTasksForAssignment = editTasksForAssignment;
         _view.SetFormulaView(_editFormulaPresenter.BaseView);
         _eventAssingnmentToDTOAssignmentMapper = eventAssingnmentToDTOAssignmentMapper;
         AddSubPresenters(_editFormulaPresenter, selectReferencePresenter);
      }

      public override void Edit(IEventAssignmentBuilder eventAssignmentBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         _eventAssignmentBuilder = eventAssignmentBuilder;
         bindToFormula();
         _selectReferencePresenter.Init(eventAssignmentBuilder, _contextSpecificReferencesRetriever.RetrieveFor(_eventAssignmentBuilder), eventAssignmentBuilder);
         var dto = _eventAssingnmentToDTOAssignmentMapper.MapFrom(_eventAssignmentBuilder);
         dto.AddUsedNames(_editTasksForAssignment.GetForbiddenNamesWithoutSelf(eventAssignmentBuilder, existingObjectsInParent));
         _view.Show(dto);
      }

      private void bindToFormula()
      {
         _editFormulaPresenter.Init(_eventAssignmentBuilder, BuildingBlock);
      }

      public override object Subject => _eventAssignmentBuilder;

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.MapAllUsing(_formulaToDTOFormulaMapper);
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(propertyName, newValue, oldValue, _eventAssignmentBuilder, BuildingBlock).Run(_context));
      }

      public void RenameSubject()
      {
         _editTasksForAssignment.Rename(_eventAssignmentBuilder, BuildingBlock);
      }

      public void SelectPath()
      {
         IFormulaUsablePath objectPath;
         using (var selectEventAssingmentTargetPresenter = _applicationController.Start<ISelectEventAssingmentTargetPresenter>())
         {
            selectEventAssingmentTargetPresenter.Init(_context.CurrentProject, _eventAssignmentBuilder.RootContainer);
            objectPath = selectEventAssingmentTargetPresenter.Select();
         }

         if (objectPath == null) return;

         AddCommand(new SetEventAssignmentObjectPathCommand(_eventAssignmentBuilder, objectPath, BuildingBlock).Run(_context));
         _view.TargetPath = objectPath.PathAsString;
         bindToFormula();
      }
   }
}