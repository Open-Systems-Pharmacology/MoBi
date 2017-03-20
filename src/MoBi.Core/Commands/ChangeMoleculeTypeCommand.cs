using System;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Repositories;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ChangeMoleculeTypeCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IMoleculeBuilder _moleculeBuilder;
      private readonly QuantityType _oldType;
      private readonly QuantityType _newType;
      private readonly string _moleculeBuilderId;

      public ChangeMoleculeTypeCommand(IMoleculeBuilder moleculeBuilder, QuantityType newType, QuantityType oldType, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _moleculeBuilder = moleculeBuilder;
         _moleculeBuilderId = _moleculeBuilder.Id;
         _newType = newType;
         _oldType = oldType;
         ObjectType = ObjectTypes.Molecule;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.MoleculeType, Enum.GetName(typeof (QuantityType), oldType), Enum.GetName(typeof (QuantityType), newType), moleculeBuilder.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeMoleculeTypeCommand(_moleculeBuilder, _oldType, _newType, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _moleculeBuilder = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _moleculeBuilder.QuantityType = _newType;
         var iconRepository = context.Resolve<IIconRepository>();
         // Reset Icon so Iconrepository can retrieve the new icon based on type
         _moleculeBuilder.Icon = string.Empty;
         _moleculeBuilder.Icon = iconRepository.IconFor(_moleculeBuilder);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _moleculeBuilder = context.Get<IMoleculeBuilder>(_moleculeBuilderId);
      }
   }
}