using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Presenter
{
   public interface IEditUnitPresenter : IPresenter<IEditUnitView>, IEditPresenter<Unit>
   {
      void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue);
   }

   public interface ICreationPresenter<T> : IPresenter
   {
      T GetNew();
   }

   public interface ICreateUnitPresenter : IEditUnitPresenter, ICreationPresenter<Unit>
   {
   }

   public class CreateUnitPresenter : AbstractEditPresenter<IEditUnitView, IEditUnitPresenter, Unit>, ICreateUnitPresenter
   {
      private readonly IUnitToDTOUnitMapper _unitToDtoUnitMapper;
      private Unit _dto;
      private Unit _subject;

      public CreateUnitPresenter(IEditUnitView view, IUnitToDTOUnitMapper unitToDtoUnitMapper) : base(view)
      {
         _unitToDtoUnitMapper = unitToDtoUnitMapper;
      }

      public Unit GetNew()
      {
         Edit(new Unit(string.Empty, 1, 0));
         using (var presenter = IoC.Resolve<IModalPresenter>())
         {
            presenter.Encapsulate(this);
            if (!presenter.Show())
               return null;

            return new Unit(_dto.Name, _dto.Factor, _dto.Offset);
         }
      }

      public override void Edit(Unit objectToEdit)
      {
         _subject = objectToEdit;
         _dto = _unitToDtoUnitMapper.MapFrom(objectToEdit);
         _view.Show(_dto);
      }

      public void SetPropertyValueFromView<T>(string propertyName, T newValue, T oldValue)
      {
         //Nothing To Do in creation because no command necessary.
      }

      public override object Subject => _subject;
   }
}