﻿using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class SynchronizeParameterValueToParameterValueCommand : MoBiReversibleCommand
   {
      private IParameter _sourceParameter;
      private IParameter _targetParameter;
      private readonly string _sourceParameterId;
      private readonly string _targetParameterId;

      public SynchronizeParameterValueToParameterValueCommand(IParameter sourceParameter, IParameter targetParameter)
      {
         _sourceParameter = sourceParameter;
         _sourceParameterId = sourceParameter.Id;
         _targetParameter = targetParameter;
         _targetParameterId = targetParameter.Id;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _targetParameter.Dimension = _sourceParameter.Dimension;
         _targetParameter.DisplayUnit = _sourceParameter.DisplayUnit;
         _targetParameter.UpdateValueOriginFrom(_sourceParameter.ValueOrigin);
         _targetParameter.UpdateQuantityValue(_sourceParameter.Value);
      }

      protected override void ClearReferences()
      {
         _sourceParameter = null;
         _targetParameter = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SynchronizeParameterValueToParameterValueCommand(_sourceParameter, _targetParameter).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _sourceParameter = context.Get<IParameter>(_sourceParameterId);
         _targetParameter = context.Get<IParameter>(_targetParameterId);
      }
   }
}