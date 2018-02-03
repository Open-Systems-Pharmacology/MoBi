using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateValueOriginInBuildingBlockCommand : EditQuantityInBuildingBlockCommand<IParameter>
   {
      private ValueOrigin _valueOrigin;
      private ValueOrigin _oldValueOrigin;

      public UpdateValueOriginInBuildingBlockCommand(IParameter quantity, ValueOrigin valueOrigin, IBuildingBlock buildingBlock) : base(quantity, buildingBlock)
      {
         _valueOrigin = valueOrigin;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldValueOrigin = _quantity.ValueOrigin.Clone();
         _quantity.ValueOrigin.UpdateFrom(_valueOrigin);
         Description = AppConstants.Commands.UpdateParameterValueOriginInSimulation(_quantity.EntityPath(), _oldValueOrigin.ToString(), _valueOrigin.ToString(), _buildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _valueOrigin = null;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateValueOriginInBuildingBlockCommand(_quantity, _oldValueOrigin, _buildingBlock).AsInverseFor(this);
      }
   }
}