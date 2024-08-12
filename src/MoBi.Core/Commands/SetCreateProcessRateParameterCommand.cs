using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;
using System.Runtime.InteropServices;

namespace MoBi.Core.Commands
{
   public class SetCreateProcessRateParameterCommand<T> : MoBiReversibleCommand where T : class, IBuildingBlock
   {
      private IProcessBuilder _processBuilder;
      private readonly string _processBuilderId;
      private readonly bool _oldCreateProcessRate;
      private readonly bool _createProcessRate;

      public SetCreateProcessRateParameterCommand(bool createProcessRate, IProcessBuilder processBuilder)
      {
         _processBuilder = processBuilder;
         _processBuilderId = processBuilder.Id;
         _createProcessRate = createProcessRate;
         _oldCreateProcessRate = _processBuilder.CreateProcessRateParameter;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = processBuilder.IsAnImplementationOf<ReactionBuilder>() ? ObjectTypes.Reaction : ObjectTypes.ApplicationTransport;
         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.CreateProcessRateParameter, _oldCreateProcessRate.ToString(), _createProcessRate.ToString(), _processBuilder.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _processBuilder.CreateProcessRateParameter = _createProcessRate;
      }

      protected override void ClearReferences()
      {
         _processBuilder = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _processBuilder = context.Get<IProcessBuilder>(_processBuilderId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetCreateProcessRateParameterCommand<T>(_oldCreateProcessRate, _processBuilder).AsInverseFor(this);
      }
   }
}