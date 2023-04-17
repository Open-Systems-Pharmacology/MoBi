using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;


namespace MoBi.Presentation.Presenter
{
   public interface IDropAllowedTypesFor<T> : IEnumerable<Type>
   {
      bool IsDropAllowed(Type type);
   }

   public class DropAllowedTypesFor<T> : List<Type>, IDropAllowedTypesFor<T>
   {
      public DropAllowedTypesFor() : this(new List<Type>())
      {
      }

      public DropAllowedTypesFor(IEnumerable<Type> list) : base(list)
      {
      }

      public bool IsDropAllowed(Type type)
      {
         foreach (var item in this)
         {
            if (item.IsAssignableFrom(type))
            {
               return true;
            }
         }
         return false;
      }
   }

   public class DropAllowedTypesForExplicitFormula : DropAllowedTypesFor<ExplicitFormula>
   {
      public DropAllowedTypesForExplicitFormula()
         : base(new List<Type> {typeof (IFormulaUsable)})
      {
      }
   }

   public class DropAllowedTypesForSwitch : DropAllowedTypesFor<Event>
   {
      public DropAllowedTypesForSwitch()
         : base(new List<Type> {typeof (IFormulaUsable), typeof (IEntity)})
      {
      }
   }
}