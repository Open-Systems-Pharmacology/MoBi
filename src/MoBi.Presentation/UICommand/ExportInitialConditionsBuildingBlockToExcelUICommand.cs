﻿using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ExportInitialConditionsBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<InitialConditionsBuildingBlock>
   {
      public ExportInitialConditionsBuildingBlockToExcelUICommand(
         IMoBiProjectRetriever projectRetriever,
         IDialogCreator dialogCreator,
         IInitialConditionsBuildingBlockToDataTableMapper mapper)
         : base(projectRetriever, dialogCreator, mapper)
      {
      }
   }
}