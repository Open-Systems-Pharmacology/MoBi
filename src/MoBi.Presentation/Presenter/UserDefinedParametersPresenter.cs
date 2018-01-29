using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IUserDefinedParametersPresenter : IEditParameterListPresenter
   {
      void ShowUserDefinedParametersIn(IContainer container);
      void ShowUserDefinedParametersIn(IEnumerable<IContainer> containers);
   }

   public class UserDefinedParametersPresenter : AbstractParameterBasePresenter<IEditParameterListView, IEditParameterListPresenter>, IUserDefinedParametersPresenter
   {
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;
      private IEnumerable<IContainer> _containers;

      public UserDefinedParametersPresenter(IEditParameterListView view,
         IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext,
         IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IInteractionTasksForParameter parameterTask,
         IFavoriteTask favoriteTask,
         IParameterToParameterDTOMapper parameterDTOMapper) : base(view, quantityTask, interactionTaskContext, formulaMapper, parameterTask, favoriteTask)
      {
         _parameterDTOMapper = parameterDTOMapper;
      }

      protected override void RefreshViewAndSelect(IParameterDTO parameterDTO)
      {
         refreshView();
      }

      public void ShowUserDefinedParametersIn(IContainer container)
      {
         ShowUserDefinedParametersIn(new[] {container});
      }

      public void ShowUserDefinedParametersIn(IEnumerable<IContainer> containers)
      {
         _containers = containers;
         refreshView();
      }

      private void refreshView()
      {
         var parameters = _containers.SelectMany(c => c.GetAllChildren<IParameter>(x => !x.IsDefault).MapAllUsing(_parameterDTOMapper)).Cast<ParameterDTO>().ToList();
         _view.BindTo(parameters);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         //TODO
      }

      public void GoTo(ParameterDTO parameterDTO)
      {
         //TODO
      }
   }
}