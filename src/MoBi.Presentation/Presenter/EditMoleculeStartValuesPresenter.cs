using MoBi.Assets;
using OSPSuite.Utility.Events;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditMoleculeStartValuesPresenter : ISingleStartPresenter<InitialConditionsBuildingBlock>, IListener<EntitySelectedEvent>
   {
      void ExtendStartValues();
      void AddNewEmptyStartValue();
   }

   public class EditMoleculeStartValuesPresenter : EditBuildingBlockPresenterBase<IEditMoleculeStartValuesView, IEditMoleculeStartValuesPresenter, InitialConditionsBuildingBlock, InitialCondition>,
      IEditMoleculeStartValuesPresenter
   {
      private readonly IMoleculeStartValuesPresenter _moleculeStartValuesPresenter;
      private InitialConditionsBuildingBlock _moleculeStartValues;

      public EditMoleculeStartValuesPresenter(IEditMoleculeStartValuesView view, IMoleculeStartValuesPresenter moleculeStartValuesPresenter, IFormulaCachePresenter formulaCachePresenter) :
            base(view, formulaCachePresenter)
      {
         _moleculeStartValuesPresenter = moleculeStartValuesPresenter;
         AddSubPresenters(moleculeStartValuesPresenter);
         view.AddMoleculeStartValuesView(_moleculeStartValuesPresenter.BaseView);
      }

      public override void Edit(InitialConditionsBuildingBlock moleculeStartValues)
      {
         if (moleculeStartValues == null) return;
         _moleculeStartValues = moleculeStartValues;
         _moleculeStartValuesPresenter.Edit(_moleculeStartValues);
         EditFormulas(moleculeStartValues);
         UpdateCaption();

         _view.Display();
      }

      public void ExtendStartValues()
      {
         _moleculeStartValuesPresenter.ExtendStartValues();
      }

      public void AddNewEmptyStartValue()
      {
         _moleculeStartValuesPresenter.AddNewEmptyStartValue();
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.MoleculeStartValuesBuildingBlockCaption(_moleculeStartValues.Name);
      }

      public override object Subject => _moleculeStartValues;
   }
}