using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SynchronizeParameterStartValueCommand : MoBiReversibleCommand
   {
      private IParameter _parameter;
      private readonly ParameterStartValue _parameterStartValue;
      private readonly string _parameterId;

      public SynchronizeParameterStartValueCommand(IParameter parameter, ParameterStartValue parameterStartValue)
      {
         _parameter = parameter;
         _parameterId = parameter.Id;
         _parameterStartValue = parameterStartValue;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _parameterStartValue.Value = _parameter.Value;
         _parameterStartValue.Dimension = _parameter.Dimension;
         _parameterStartValue.DisplayUnit = _parameter.DisplayUnit;
         _parameterStartValue.UpdateValueOriginFrom(_parameter.ValueOrigin);
      }

      protected override void ClearReferences()
      {
         _parameter = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SynchronizeParameterStartValueCommand(_parameter, _parameterStartValue)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _parameter = context.Get<IParameter>(_parameterId);
      }
   }
}