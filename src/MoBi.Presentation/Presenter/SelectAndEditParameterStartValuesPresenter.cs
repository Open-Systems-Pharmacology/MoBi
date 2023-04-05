using System.Drawing;
using MoBi.Assets;
using OSPSuite.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectAndEditParameterStartValuesPresenter : ISelectAndEditStartValuesPresenter<ParameterStartValuesBuildingBlock>
   {
   }

   internal class SelectAndEditParameterStartValuesPresenter : SelectAndEditStartValuesPresenter<ParameterStartValuesBuildingBlock, ParameterStartValue>, ISelectAndEditParameterStartValuesPresenter
   {
      private readonly IParameterStartValuesPresenter _editPresenter;

      public SelectAndEditParameterStartValuesPresenter(ISelectAndEditContainerView view, IParameterStartValuesTask parameterStartValuesTask, ICloneManagerForBuildingBlock cloneManager,
         IObjectTypeResolver objectTypeResolver, IParameterStartValuesPresenter editPresenter, ILegendPresenter legendPresenter)
         : base(view, parameterStartValuesTask, cloneManager, objectTypeResolver, editPresenter, legendPresenter)
      {
         _editPresenter = editPresenter;
         View.Caption = AppConstants.Captions.ParameterStartValues;
         View.AddEditView(editPresenter.BaseView);
         _editPresenter.BackgroundColorRetriever = displayColorFor;
         _editPresenter.IsOriginalStartValue = isTemplate;
      }

      private bool isTemplate(ParameterStartValueDTO startValueDTO)
      {
         return IsTemplate(startValueDTO.ParameterStartValue);
      }

      private Color displayColorFor(ParameterStartValueDTO startValueDTO)
      {
         return DisplayColorFor(startValueDTO.ParameterStartValue);
      }

      public override void Refresh()
      {
         base.Refresh();
         // Refresh(_simulationConfiguration.ParameterStartValues);
         _editPresenter.Edit(StartValues);
      }

      public override ApplicationIcon Icon => ApplicationIcons.ParameterStartValues;
   }
}