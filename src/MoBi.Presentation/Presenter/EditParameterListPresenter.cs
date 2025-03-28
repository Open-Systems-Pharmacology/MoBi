using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using IParameterToParameterDTOMapper = MoBi.Presentation.Mappers.IParameterToParameterDTOMapper;

namespace MoBi.Presentation.Presenter
{
   public class EditParameterListPresenter : AbstractParameterBasePresenter<IEditParameterListView, IEditParameterListPresenter>,
      IEditParameterListPresenter
   {
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;
      private readonly List<ParameterDTO> _parameterDTOs = new List<ParameterDTO>();
      public IEnumerable<IParameter> EditedParameters { get; private set; }

      public EditParameterListPresenter(
         IEditParameterListView view,
         IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext,
         IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IInteractionTasksForParameter parameterTask,
         IFavoriteTask favoriteTask,
         IViewItemContextMenuFactory viewItemContextMenuFactory,
         IParameterToParameterDTOMapper parameterDTOMapper) :
         base(view, quantityTask, interactionTaskContext, formulaMapper, parameterTask, favoriteTask)
      {
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _parameterDTOMapper = parameterDTOMapper;
      }

      public void ShowContextMenu(IViewItem viewItem, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(viewItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void GoTo(ParameterDTO parameterDTO)
      {
         if (parameterDTO == null)
            return;

         var parameter = ParameterFrom(parameterDTO);
         _interactionTaskContext.Context.PublishEvent(new EntitySelectedEvent(parameter, this));
      }

      public void Edit(IEnumerable<IParameter> parameters)
      {
         releaseParameters();

         EditedParameters = parameters;
         _parameterDTOs.AddRange(EditedParameters.MapAllUsing(_parameterDTOMapper));

         _view.BindTo(_parameterDTOs);

         EnumHelper.AllValuesFor<PathElementId>().Each(updateColumnVisibility);
      }

      private void updateColumnVisibility(PathElementId pathElement)
      {
         SetVisibility(pathElement, !_parameterDTOs.HasOnlyEmptyValuesAt(pathElement));
      }

      public void SetVisibility(PathElementId pathElement, bool isVisible)
      {
         View.SetVisibility(pathElement, isVisible);
      }

      public virtual IReadOnlyList<IParameter> SelectedParameters
      {
         get => ParametersFrom(_view.SelectedParameters).ToList();
         set
         {
            if (value == null)
               return;

            _view.SelectedParameters = _parameterDTOs.Where(x => value.Contains(x.Parameter)).ToList();
         }
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         releaseParameters();
      }

      private void releaseParameters()
      {
         _parameterDTOs.Each(dto => dto.Release());
         _parameterDTOs.Clear();
      }
   }
}