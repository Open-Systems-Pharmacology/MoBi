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
   public interface ISelectAndEditMoleculesStartValuesPresenter : ISelectAndEditStartValuesPresenter<IMoleculeStartValuesBuildingBlock>
   {
   }

   internal class SelectAndEditMoleculesStartValuesPresenter : SelectAndEditStartValuesPresenter<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>, ISelectAndEditMoleculesStartValuesPresenter
   {
      private readonly IMoleculeStartValuesPresenter _editPresenter;

      public SelectAndEditMoleculesStartValuesPresenter(
         ISelectAndEditContainerView view,
         IMoleculeStartValuesTask moleculeStartValuesTask,
         ICloneManagerForBuildingBlock cloneManager,
         IObjectTypeResolver objectTypeResolver,
         IMoleculeStartValuesPresenter editPresenter, ILegendPresenter legendPresenter)
         : base(view, moleculeStartValuesTask, cloneManager, objectTypeResolver, editPresenter, legendPresenter)
      {
         _editPresenter = editPresenter;
         View.AddEditView(editPresenter.BaseView);
         View.Caption = AppConstants.Captions.MoleculeStartValues;
         _editPresenter.BackgroundColorRetriever = displayColorFor;
         _editPresenter.IsOriginalStartValue = isTemplate;
      }

      private bool isTemplate(MoleculeStartValueDTO startValueDTO)
      {
         return IsTemplate(startValueDTO.MoleculeStartValue);
      }

      private Color displayColorFor(MoleculeStartValueDTO startValueDTO)
      {
         return DisplayColorFor(startValueDTO.MoleculeStartValue);
      }

      public override void Refresh()
      {
         base.Refresh();
         Refresh(_buildConfiguration.MoleculeStartValuesInfo);
         _editPresenter.Edit(StartValues);
      }

      public override ApplicationIcon Icon => ApplicationIcons.MoleculeStartValues;
   }
}