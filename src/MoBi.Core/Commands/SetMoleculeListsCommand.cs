using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SetMoleculeListsCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IMoleculeDependentBuilder _targetBuilder;
      private MoleculeList _oldMoleculeList;
      private MoleculeList _newMoleculeList;
      private readonly string _targetBuilderId;

      public SetMoleculeListsCommand(IMoleculeDependentBuilder targetBuilder, MoleculeList newMoleculeList, IBuildingBlock targetBuildingBlock)
         : base(targetBuildingBlock)
      {
         _targetBuilder = targetBuilder;
         _targetBuilderId = _targetBuilder.Id;
         _newMoleculeList =newMoleculeList;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(targetBuilder);
         Description = AppConstants.Commands.EditDescriptionMoleculeList(ObjectType,newMoleculeList,_targetBuilder.Name);
      }


      protected override void ClearReferences()
      {
         base.ClearReferences();
         _targetBuilder = null;
         _newMoleculeList = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldMoleculeList = _targetBuilder.MoleculeList.Clone();
         _targetBuilder.MoleculeList.Update(_newMoleculeList);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _targetBuilder = context.Get<ITransportBuilder>(_targetBuilderId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetMoleculeListsCommand(_targetBuilder, _oldMoleculeList, _buildingBlock).AsInverseFor(this);
      }
   }
}