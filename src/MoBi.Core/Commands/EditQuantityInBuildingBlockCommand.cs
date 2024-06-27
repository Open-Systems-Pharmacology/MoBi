using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class EditQuantityInBuildingBlockCommand<TQuantity> : BuildingBlockChangeCommandBase<IBuildingBlock> where TQuantity : class, IQuantity
   {
      protected TQuantity _quantity;
      private readonly string _quantityId;

      protected EditQuantityInBuildingBlockCommand(TQuantity quantity, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _quantity = quantity;
         _quantityId = quantity.Id;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(quantity);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _quantity = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _quantity = context.Get<TQuantity>(_quantityId);
      }
   }
}