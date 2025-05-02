using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Helper
{
   /// <summary>
   ///    Encapsulates a setter and getter method providing read and write access to an IFormula field
   /// </summary>
   public abstract class FormulaDecoder
   {
      /// <summary>
      ///    Extracts the name of the property being used to supply the formula
      /// </summary>
      /// <returns>A string representing the property name</returns>
      public abstract string PropertyName { get; }

      public IFormula GetFormula(object formulaOwner)
      {
         var property = formulaOwner.GetType().GetProperty(PropertyName);
         return property?.GetValue(formulaOwner, null) as IFormula;
      }
   }

   /// <summary>
   ///    Encapsulates a setter and getter method providing read and write access to an IFormula field
   /// </summary>
   /// <typeparam name="T">The type that has a property of type IFormula that should be accessed</typeparam>
   public abstract class FormulaDecoder<T> : FormulaDecoder
   {
      public abstract IFormula GetFormula(T objectWithFormula);
      public abstract void SetFormula(IFormula formula, T objectWithFormula);

   }

   /// <summary>
   ///    Encapsulates read and write access to RHSFormula on a parameter
   /// </summary>
   public class RHSFormulaDecoder : FormulaDecoder<IParameter>
   {
      public override string PropertyName { get; } = MoBiReflectionHelper.PropertyName<IParameter>(x => x.RHSFormula);

      public override IFormula GetFormula(IParameter parameter) => parameter.RHSFormula;

      public override void SetFormula(IFormula formula, IParameter parameter) => parameter.RHSFormula = formula;
   }

   /// <summary>
   ///    Encapsulates read and write access to DefaultStartFormula on a molecule builder
   /// </summary>
   public class DefaultStartFormulaDecoder : FormulaDecoder<MoleculeBuilder>
   {
      public override string PropertyName { get; } = MoBiReflectionHelper.PropertyName<MoleculeBuilder>(x => x.DefaultStartFormula);

      public override IFormula GetFormula(MoleculeBuilder moleculeBuilder) => moleculeBuilder.DefaultStartFormula;

      public override void SetFormula(IFormula formula, MoleculeBuilder moleculeBuilder) => moleculeBuilder.DefaultStartFormula = formula;
   }

   /// <summary>
   ///    Encapsulates read and write access to Formula on an object implementing IUsingFormula
   /// </summary>
   public class UsingFormulaDecoder : FormulaDecoder<IUsingFormula>
   {
      public override string PropertyName { get; } = MoBiReflectionHelper.PropertyName<IUsingFormula>(x => x.Formula);

      public override IFormula GetFormula(IUsingFormula usingFormula) => usingFormula.Formula;

      public override void SetFormula(IFormula formula, IUsingFormula usingFormula) => usingFormula.Formula = formula;
   }
}