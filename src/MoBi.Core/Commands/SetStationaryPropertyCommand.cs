using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class SetStationaryPropertyCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private MoleculeBuilder _moleculeBuilder;
      private readonly bool _newValue;
      private readonly bool _oldValue;
      private readonly string _moleculeBuilderId;

      public SetStationaryPropertyCommand(MoleculeBuilder moleculeBuilder, bool newValue, bool oldValue, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _moleculeBuilder = moleculeBuilder;
         _moleculeBuilderId = _moleculeBuilder.Id;
         _newValue = newValue;
         _oldValue = oldValue;
         ObjectType = ObjectTypes.Molecule;
         CommandType = AppConstants.Commands.EditCommand;
         _oldValue = oldValue;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.IsStationary, _oldValue.ToString(), newValue.ToString(), moleculeBuilder.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _moleculeBuilder = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         //ObjectModel is inverse to user view
         _moleculeBuilder.IsFloating = !_newValue;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _moleculeBuilder = context.Get<MoleculeBuilder>(_moleculeBuilderId);
      }


      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetStationaryPropertyCommand(_moleculeBuilder,_oldValue,_newValue,_buildingBlock).AsInverseFor(this);
      }
   }
}