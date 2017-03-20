using System;
using OSPSuite.Utility.Container;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks
{
   public interface IUnitTasks
   {
      void Initialise(IDimension dimension);
      Unit Add();
      void Remove(Unit unit);
   }


   public class UnitTasks : IUnitTasks
   {
      private IDimension _dimension;


      public void Initialise(IDimension dimension)
      {
         _dimension = dimension;
      }

      public Unit Add()
      {
         var createPresenter = IoC.Resolve<ICreateUnitPresenter>();
         return createPresenter.GetNew();
      }

      public void Remove(Unit unit)
      {
         _dimension.RemoveUnit(unit.Name);
      }
   }
}