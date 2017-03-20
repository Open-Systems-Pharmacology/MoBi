using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SetForAllCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IMoleculeDependentBuilder _moleculeDependentBuilder;
      private readonly bool _forAll;
      private readonly string _objectId;

      public SetForAllCommand(IMoleculeDependentBuilder moleculeDependentBuilder, bool forAll,IBuildingBlock buildingBlock):base(buildingBlock)
      {
         _moleculeDependentBuilder = moleculeDependentBuilder;
         _forAll = forAll;
         _objectId = _moleculeDependentBuilder.Id;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(_moleculeDependentBuilder);
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.ForAll, (!_forAll).ToString(), forAll.ToString(), moleculeDependentBuilder.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetForAllCommand(_moleculeDependentBuilder, !_forAll,_buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _moleculeDependentBuilder = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _moleculeDependentBuilder.ForAll = _forAll;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _moleculeDependentBuilder = context.Get<IMoleculeDependentBuilder>(_objectId);
      }
   }
}