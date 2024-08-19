using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.UICommand
{
   public class ExportInitialConditionsBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<InitialConditionsBuildingBlock>
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IDialogCreator _dialogCreator;
      private readonly IInitialConditionsBuildingBlockToDataTableMapper _mapper;

      public ExportInitialConditionsBuildingBlockToExcelUICommand(
         IMoBiProjectRetriever projectRetriever,
         IDialogCreator dialogCreator,
         IInitialConditionsBuildingBlockToDataTableMapper mapper)
         : base(projectRetriever, dialogCreator, mapper)
      {
      }
   }
}