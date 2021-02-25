using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ChangeMoleculeNameAtMoleculeDependentBuilderCommandBase : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      protected IMoleculeDependentBuilder _moleculeDependentBuilder;

      public ChangeMoleculeNameAtMoleculeDependentBuilderCommandBase(string newMoleculeName, string oldMoleculeName, IMoleculeDependentBuilder moleculeDependentBuilder, IBuildingBlock buildingBlock):base(buildingBlock)
      {
         CommandType = AppConstants.Commands.EditCommand;
         _moleculeDependentBuilder = moleculeDependentBuilder;
         ObserverBuilderId = moleculeDependentBuilder.Id;
         NewMoleculeName = newMoleculeName;
         OldMoleculeName = oldMoleculeName;
      }

      public string OldMoleculeName { get; set; }
      public string NewMoleculeName { get; set; }
      public string ObserverBuilderId { get; set; }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         var inverseCommand = CommandExtensions.AsInverseFor(new ChangeMoleculeNameAtMoleculeDependentBuilderCommand(OldMoleculeName,NewMoleculeName,_moleculeDependentBuilder,_buildingBlock), this);
         inverseCommand.Visible = Visible;
         return inverseCommand;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _moleculeDependentBuilder = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _moleculeDependentBuilder = context.Get<IMoleculeDependentBuilder>(ObserverBuilderId);
      }
   }

   public class ChangeExcludeMoleculeNameAtMoleculeDependentBuilderCommand : ChangeMoleculeNameAtMoleculeDependentBuilderCommandBase
   {
      public ChangeExcludeMoleculeNameAtMoleculeDependentBuilderCommand(string newMoleculeName, string oldMoleculeName, IMoleculeDependentBuilder moleculeDependentBuilder, IBuildingBlock buildingBlock) : base(newMoleculeName, oldMoleculeName, moleculeDependentBuilder, buildingBlock)
      {
         Description = AppConstants.Commands.EditDescription(ObjectType, "Excluded Molecules", OldMoleculeName, NewMoleculeName, _moleculeDependentBuilder.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _moleculeDependentBuilder.RemoveMoleculeNameToExclude(OldMoleculeName);
         _moleculeDependentBuilder.AddMoleculeNameToExclude(NewMoleculeName);
      }
   }

   public class ChangeMoleculeNameAtMoleculeDependentBuilderCommand:ChangeMoleculeNameAtMoleculeDependentBuilderCommandBase
   {
      public ChangeMoleculeNameAtMoleculeDependentBuilderCommand(string newMoleculeName, string oldMoleculeName, IMoleculeDependentBuilder moleculeDependentBuilder, IBuildingBlock buildingBlock): base(newMoleculeName, oldMoleculeName, moleculeDependentBuilder, buildingBlock)
      {
         Description = AppConstants.Commands.EditDescription(ObjectType, "Molecules", OldMoleculeName, NewMoleculeName, _moleculeDependentBuilder.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _moleculeDependentBuilder.RemoveMoleculeName(OldMoleculeName);
         _moleculeDependentBuilder.AddMoleculeName(NewMoleculeName);
      }
   }
}