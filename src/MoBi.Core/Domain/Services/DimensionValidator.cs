using System.Collections.Generic;
using System.Threading.Tasks;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.FuncParser;

namespace MoBi.Core.Domain.Services
{
   public interface IDimensionValidator : IVisitor<IEntity>, IVisitor<IUsingFormula>
   {
      Task<ValidationResult> Validate(IContainer container, IBuildConfiguration buildConfiguration);
      Task<ValidationResult> Validate(IModel model, IBuildConfiguration buildConfiguration);
      Task<ValidationResult> Validate(IEnumerable<IContainer> containers, IBuildConfiguration buildConfiguration);
   }



   public static class DimensionInfoExtensions
   {
      public static bool AreEquals(this IDimensionInfo left, IDimensionInfo right)
      {
         return (left.Length == right.Length
                 && left.Mass == right.Mass
                 && left.Time == right.Time
                 && left.ElectricCurrent == right.ElectricCurrent
                 && left.Temperature == right.Temperature
                 && left.Amount == right.Amount
                 && left.LuminousIntensity == right.LuminousIntensity);
      }
   }
}