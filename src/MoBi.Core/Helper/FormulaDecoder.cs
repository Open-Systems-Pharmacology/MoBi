using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Helper
{
   /// <summary>
   /// Encapsulates a setter and getter method providing read and write access to an IFormula field
   /// </summary>
   /// <typeparam name="T">The type that has a property of type IFormula that should be accessed</typeparam>
   public abstract class FormulaDecoder<T>
   {
      public Func<T, IFormula> GetFormula { get; protected set; }

      public Action<IFormula, T> SetFormula { get; protected set; }

      /// <summary>
      /// Extracts the name of the property being used to supply the formula
      /// </summary>
      /// <returns>A string representing the property name</returns>
      public abstract string PropertyName();
   }

   /// <summary>
   /// Encapsulates read and write access to RHSFormula on a parameter
   /// </summary>
   public class RHSFormulaDecoder : FormulaDecoder<IParameter>
   {
      public RHSFormulaDecoder()
      {
         GetFormula = parameter => parameter.RHSFormula;
         SetFormula = (formula, parameter) => parameter.RHSFormula = formula;
      }

      public override string PropertyName()
      {
         return MoBiReflectionHelper.PropertyName<IParameter>(parameter => parameter.RHSFormula);
      }
   }

   /// <summary>
   /// Encapsulates read and write access to DefaultStartFormula on a molecule builder
   /// </summary>
   public class DefaultStartFormulaDecoder : FormulaDecoder<IMoleculeBuilder>
   {
      public DefaultStartFormulaDecoder()
      {
         GetFormula = builder => builder.DefaultStartFormula;
         SetFormula = (formula, builder) => builder.DefaultStartFormula = formula;
      }

      public override string PropertyName()
      {
         return MoBiReflectionHelper.PropertyName<IMoleculeBuilder>(builder => builder.DefaultStartFormula);
      }
   }

   /// <summary>
   /// Encapsulates read and write access to Formula on an object implementing IUsingFormula
   /// </summary>
   public class UsingFormulaDecoder : FormulaDecoder<IUsingFormula>
   {
      public UsingFormulaDecoder()
      {
         GetFormula = usingFormula => usingFormula.Formula;
         SetFormula = (formula, usingFormula) => usingFormula.Formula = formula;
      }

      public override string PropertyName()
      {
         return MoBiReflectionHelper.PropertyName<IUsingFormula>(usingF => usingF.Formula);
      }
   }
}