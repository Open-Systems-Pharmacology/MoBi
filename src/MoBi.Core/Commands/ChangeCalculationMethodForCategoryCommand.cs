using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ChangeCalculationMethodForCategoryCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private UsedCalculationMethod _changedUsedCalculationMethod;
      private IMoleculeBuilder _moleculeBuilder;

      public ChangeCalculationMethodForCategoryCommand(IMoleculeBuilder moleculeBuilder, string category, string newValue, string oldValue,IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         MoleculeBuilderId = moleculeBuilder.Id;
         _moleculeBuilder = moleculeBuilder;
         _changedUsedCalculationMethod = getUsedCalulationMethod(moleculeBuilder, category, oldValue);
         NewCalculationMethod = newValue;
         OldCalculationMethod = oldValue;
         Category = category;
      }

      public string Category { get; set; }
      public string NewCalculationMethod { get; set; }
      public string OldCalculationMethod { get; set; }
      public string MoleculeBuilderId { get; set; }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _changedUsedCalculationMethod.CalculationMethod = NewCalculationMethod;
         context.PublishEvent(new ChangedCalculationMethodEvent(_moleculeBuilder));
         context.ProjectChanged();
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _moleculeBuilder = null;
         _changedUsedCalculationMethod = null;
      }
      
      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeCalculationMethodForCategoryCommand(_moleculeBuilder,Category,OldCalculationMethod,NewCalculationMethod,_buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _moleculeBuilder = context.Get<IMoleculeBuilder>(MoleculeBuilderId);
         _changedUsedCalculationMethod = getUsedCalulationMethod(_moleculeBuilder, Category, NewCalculationMethod);
      }

      private UsedCalculationMethod getUsedCalulationMethod(IMoleculeBuilder moleculeBuilder, string category, string calulationMethod)
      {
         return moleculeBuilder.UsedCalculationMethods
            .Where(ucm => calulationMethod.Equals(calulationMethod)).FirstOrDefault(ucm => ucm.Category.Equals(category));
      }
   }
}