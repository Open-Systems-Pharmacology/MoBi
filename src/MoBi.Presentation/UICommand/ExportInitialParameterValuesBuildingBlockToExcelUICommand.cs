using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Infrastructure.Export;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System;
using System.Linq;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.Utility;

namespace MoBi.Presentation.UICommand
{
   public class ExportParameterValuesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ParameterValuesBuildingBlock>
   {
      public ExportParameterValuesBuildingBlockToExcelUICommand(
         IMoBiProjectRetriever projectRetriever,
         IDialogCreator dialogCreator,
         IParameterValueBuildingBlockToParameterValuesDataTableMapper mapper)
         : base(projectRetriever, dialogCreator, mapper)
      {
      }
   }
}