using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddExpressionAsParameterStartValuesCommand : ObjectUICommand<IMoBiProject>
   {
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTaskForExpressionProfileBuildingBlock;
      private readonly IInteractionTasksForBuildingBlock<IParameterStartValuesBuildingBlock> _interactionTasksForPSVBuildingBlock;
      private readonly IExpressionProfileToParameterStartValuesMapper _mapper;

      public AddExpressionAsParameterStartValuesCommand(IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfileBuildingBlock, IInteractionTasksForBuildingBlock<IParameterStartValuesBuildingBlock> interactionTasksForPSVBuildingBlock, IExpressionProfileToParameterStartValuesMapper mapper)
      {
         _context = context;
         _interactionTaskForExpressionProfileBuildingBlock = interactionTaskForExpressionProfileBuildingBlock;
         _interactionTasksForPSVBuildingBlock = interactionTasksForPSVBuildingBlock;
         _mapper = mapper;
      }

      protected override void PerformExecute()
      {
         var expressionProfiles = _interactionTaskForExpressionProfileBuildingBlock.CreateFromPKML();

         foreach (var expressionProfile in expressionProfiles)
         {
            var psvBuildingBlock = mapExpressionProfileToParameterStartValues(expressionProfile);
            _context.AddToHistory(_interactionTasksForPSVBuildingBlock.AddToProject(psvBuildingBlock));
         }
      }

      private IParameterStartValuesBuildingBlock mapExpressionProfileToParameterStartValues(ExpressionProfileBuildingBlock expressionProfile)
      {
         return _mapper.MapFrom(expressionProfile);
      }
   }
}