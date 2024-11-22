using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public abstract class SetQuantityPropertyInSimulationCommandBase<TQuantity> : SimulationChangeCommandBase where TQuantity : class, IQuantity
   {
      protected TQuantity _quantity;
      private readonly string _quantityId;

      protected SetQuantityPropertyInSimulationCommandBase(TQuantity quantity, IMoBiSimulation simulation)
         : base(simulation)
      {
         _quantity = quantity;
         ObjectType = new ObjectTypeResolver().TypeFor(quantity);
         _quantityId = _quantity.Id;
         _simulation = simulation;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var changeTracker = context.Resolve<IQuantityValueInSimulationChangeTracker>();
         changeTracker.TrackQuantityChange(_quantity, _simulation, x => base.ExecuteWith(context));
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