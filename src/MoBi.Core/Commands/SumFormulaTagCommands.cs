using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class AddParameterMatchTagConditionToSumFormulaCommand : AddMatchTagConditionCommand<SumFormula>
   {
      public AddParameterMatchTagConditionToSumFormulaCommand(string tag, SumFormula sumFormula, IBuildingBlock buildingBlock)
         : base(tag, sumFormula, buildingBlock, x => x.Criteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveParameterMatchTagConditionFromSumFormulaCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveParameterMatchTagConditionFromSumFormulaCommand : RemoveMatchTagConditionCommand<SumFormula>
   {
      public RemoveParameterMatchTagConditionFromSumFormulaCommand(string tag, SumFormula sumFormula, IBuildingBlock buildingBlock)
         : base(tag, sumFormula, buildingBlock, x => x.Criteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddParameterMatchTagConditionToSumFormulaCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class AddParameterNotMatchTagConditionToSumFormulaCommand : AddNotMatchTagConditionCommand<SumFormula>
   {
      public AddParameterNotMatchTagConditionToSumFormulaCommand(string tag, SumFormula sumFormula, IBuildingBlock buildingBlock)
         : base(tag, sumFormula, buildingBlock, x => x.Criteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveParameterNotMatchTagConditionFromSumFormulaCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveParameterNotMatchTagConditionFromSumFormulaCommand : RemoveNotMatchTagConditionCommand<SumFormula>
   {
      public RemoveParameterNotMatchTagConditionFromSumFormulaCommand(string tag, SumFormula sumFormula, IBuildingBlock buildingBlock)
         : base(tag, sumFormula, buildingBlock, x => x.Criteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddParameterNotMatchTagConditionToSumFormulaCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }
}