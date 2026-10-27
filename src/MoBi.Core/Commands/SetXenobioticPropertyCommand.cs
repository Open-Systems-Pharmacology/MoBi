using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SetXenobioticPropertyCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private MoleculeBuilder _moleculeBuilder;
      private readonly bool _newValue;
      private readonly bool _oldValue;
      private readonly string _moleculeBuilderId;

      public SetXenobioticPropertyCommand(MoleculeBuilder moleculeBuilder, bool newValue, bool oldValue, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _moleculeBuilder = moleculeBuilder;
         _moleculeBuilderId = _moleculeBuilder.Id;
         _newValue = newValue;
         _oldValue = oldValue;
         ObjectType = ObjectTypes.Molecule;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.IsXenobiotic, _oldValue.ToString(), _newValue.ToString(), moleculeBuilder.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _moleculeBuilder = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _moleculeBuilder.IsXenobiotic = _newValue;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _moleculeBuilder = context.Get<MoleculeBuilder>(_moleculeBuilderId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetXenobioticPropertyCommand(_moleculeBuilder, _oldValue, _newValue, _buildingBlock).AsInverseFor(this);
      }
   }
}
