using MoBi.Assets;
using OSPSuite.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface IEditDimensionPresenter : IEditPresenter<IDimension>
   {
      void AddUnit();
      void RemoveUnit(Unit unit);
   }

   public class EditDimensionPresenter : AbstractEditPresenter<IEditDimensionView, IEditDimensionPresenter, IDimension>, IEditDimensionPresenter
   {
      private readonly IDimensionToDTODimensionMapper _dimenionToDTODimensionMapper;
      private readonly IDialogCreator _dialogCreator;
      private readonly IUnitTasks _unitTasks;
      private IDimension _dimension;

      public EditDimensionPresenter(IEditDimensionView view, IUnitTasks unitTasks, IDimensionToDTODimensionMapper dimenionToDtoDimensionMapper, IDialogCreator dialogCreator) : base(view)
      {
         _dimenionToDTODimensionMapper = dimenionToDtoDimensionMapper;
         _dialogCreator = dialogCreator;
         _unitTasks = unitTasks;
      }

      public void AddUnit()
      {
         _unitTasks.Initialise(_dimension);
         _unitTasks.Add();
         Edit(_dimension);
      }

      public void RemoveUnit(Unit unit)
      {
         var res = _dialogCreator.MessageBoxYesNo(AppConstants.Dialog.Remove(ObjectTypes.Unit, unit.Name, _dimension.Name));
         if (res == ViewResult.No) return;

         _dimension.RemoveUnit(unit.Name);
      }

      public override void Edit(IDimension objectToEdit)
      {
         _dimension = objectToEdit;
         SetViewDataFromModelData();
      }

      public override object Subject
      {
         get { return _dimension; }
      }

      public void SetViewDataFromModelData()
      {
         var dto = _dimenionToDTODimensionMapper.MapFrom(_dimension);
         _view.BindToSource(dto);
      }
   }
}