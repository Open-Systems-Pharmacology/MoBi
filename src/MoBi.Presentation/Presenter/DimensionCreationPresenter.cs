using System;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Presenter
{
   public interface IDimensionCreationPresenter : IEditDimensionPresenter, ICreationPresenter<IDimension>
   {
   }

   public class DimensionCreationPresenter : AbstractEditPresenter<IEditDimensionView, IEditDimensionPresenter, IDimension>, IDimensionCreationPresenter
   {
      private readonly IUnitTasks _unitTasks;
      private readonly IUnitToDTOUnitMapper _unitToDTOUnitMapper;
      private DimensionDTO _dto;

      public DimensionCreationPresenter(IEditDimensionView view, IUnitTasks unitTasks, IUnitToDTOUnitMapper unitToDtoUnitMapper)
         : base(view)
      {
         _unitToDTOUnitMapper = unitToDtoUnitMapper;
         _unitTasks = unitTasks;
         unitTasks.Initialise(new Dimension()); // just for temp actions
      }

      public IDimension GetNew()
      {
         _dto = new DimensionDTO();
         _view.BindToSource(_dto);

         using (var presenter = IoC.Resolve<IModalPresenter>())
         {
            presenter.Encapsulate(this);
            if (presenter.Show())
               return dimensionFromDTO();
         }
         return null;
      }

      public override void Edit(IDimension objectToEdit)
      {
         throw new NotSupportedException();
      }

      public override object Subject
      {
         get { throw new NotSupportedException(); }
      }

      public void AddUnit()
      {
         Unit newUnit = _unitTasks.Add();
         if (newUnit != null)
         {
            _dto.Units.Add(_unitToDTOUnitMapper.MapFrom(newUnit));
         }
      }

      public void RemoveUnit(Unit unit)
      {
         _dto.Units.Remove(unit);
      }

      private Dimension dimensionFromDTO()
      {
         return new Dimension(new BaseDimensionRepresentation
         {
            LengthExponent = _dto.Length,
            MassExponent = _dto.Mass,
            TimeExponent = _dto.Time,
            ElectricCurrentExponent = _dto.ElectricCurrent,
            TemperatureExponent = _dto.Temperature,
            AmountExponent = _dto.Amount,
            LuminousIntensityExponent = _dto.LuminousIntensity
         }, _dto.Name, _dto.BaseUnit);
      }
   }
}