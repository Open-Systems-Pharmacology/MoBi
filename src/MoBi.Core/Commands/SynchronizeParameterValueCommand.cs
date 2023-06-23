using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SynchronizeParameterValueCommand : MoBiReversibleCommand
   {
      private IParameter _parameter;
      private readonly ParameterValue _parameterValue;
      private readonly string _parameterId;

      public SynchronizeParameterValueCommand(IParameter parameter, ParameterValue parameterValue)
      {
         _parameter = parameter;
         _parameterId = parameter.Id;
         _parameterValue = parameterValue;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _parameterValue.Value = _parameter.Value;
         _parameterValue.Dimension = _parameter.Dimension;
         _parameterValue.DisplayUnit = _parameter.DisplayUnit;
         _parameterValue.UpdateValueOriginFrom(_parameter.ValueOrigin);
      }

      protected override void ClearReferences()
      {
         _parameter = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SynchronizeParameterValueCommand(_parameter, _parameterValue)
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