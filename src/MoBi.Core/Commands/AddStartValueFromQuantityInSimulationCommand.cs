using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class AddStartValueFromQuantityInSimulationCommand<TQuantity, TStartValue> : MoBiReversibleCommand
      where TStartValue : class, IStartValue
      where TQuantity : class, IQuantity
   {
      protected TQuantity _quantity;
      private IStartValuesBuildingBlock<TStartValue> _startValuesBuildingBlock;
      private readonly string _startValuesBuildingBlockId;
      protected IObjectPath _objectPath;

      protected AddStartValueFromQuantityInSimulationCommand(TQuantity quantity, IStartValuesBuildingBlock<TStartValue> startValuesBuildingBlock)
      {
         _quantity = quantity;
         _startValuesBuildingBlock = startValuesBuildingBlock;
         _startValuesBuildingBlockId = startValuesBuildingBlock.Id;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var entityPathResolver = context.Resolve<IEntityPathResolver>();
         _objectPath = entityPathResolver.ObjectPathFor(_quantity);

         if (_startValuesBuildingBlock[_objectPath] != null)
            return;

         _startValuesBuildingBlock.Add(CreateNewStartValue(context));
      }

      protected abstract TStartValue CreateNewStartValue(IMoBiContext context);

      protected override void ClearReferences()
      {
         _startValuesBuildingBlock = null;
         _quantity = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveStartValueFromBuildingBlockInSimulationCommand<TStartValue>(_objectPath, _startValuesBuildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _startValuesBuildingBlock = context.Get<IStartValuesBuildingBlock<TStartValue>>(_startValuesBuildingBlockId);
      }
   }

   public class AddParameterStartValueFromQuantityInSimulationCommand : AddStartValueFromQuantityInSimulationCommand<IParameter, IParameterStartValue>
   {
      public AddParameterStartValueFromQuantityInSimulationCommand(IParameter parameter, IParameterStartValuesBuildingBlock parameterStartValuesBuildingBlock)
         : base(parameter, parameterStartValuesBuildingBlock)
      {
      }

      protected override IParameterStartValue CreateNewStartValue(IMoBiContext context)
      {
         var parameterStartValueCreator = context.Resolve<IParameterStartValuesCreator>();
         return parameterStartValueCreator.CreateParameterStartValue(_objectPath, _quantity);
      }
   }

   public class AddMoleculeStartValueFromQuantityInSimulationCommand : AddStartValueFromQuantityInSimulationCommand<IMoleculeAmount, IMoleculeStartValue>
   {
      public AddMoleculeStartValueFromQuantityInSimulationCommand(IMoleculeAmount moleculeAmount, IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
         : base(moleculeAmount, moleculeStartValuesBuildingBlock)
      {
      }

      protected override IMoleculeStartValue CreateNewStartValue(IMoBiContext context)
      {
         var moleculeStartValueCreator = context.Resolve<IMoleculeStartValuesCreator>();
         var containerPath = _objectPath.Clone<IObjectPath>();
         var lastIndex = containerPath.Count - 1;
         var name = containerPath[lastIndex];
         containerPath.RemoveAt(lastIndex);

         return moleculeStartValueCreator.CreateMoleculeStartValue(containerPath, name, _quantity.Dimension, _quantity.DisplayUnit, _quantity.ValueOrigin);
      }
   }
}