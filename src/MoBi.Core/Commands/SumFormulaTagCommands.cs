using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class AddParameterMatchTagConditionToSumFormulaCommand : AddMatchTagConditionCommandBase<SumFormula>
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

   public class RemoveParameterMatchTagConditionFromSumFormulaCommand : RemoveMatchTagConditionCommandBase<SumFormula>
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

   public class AddParameterNotMatchTagConditionToSumFormulaCommand : AddNotMatchTagConditionCommandBase<SumFormula>
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

   public class RemoveParameterNotMatchTagConditionFromSumFormulaCommand : RemoveNotMatchTagConditionCommandBase<SumFormula>
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