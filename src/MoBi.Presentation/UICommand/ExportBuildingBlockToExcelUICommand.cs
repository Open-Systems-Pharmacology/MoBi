using System.Collections.Generic;
using System.Data;
using DevExpress.Entity.Model.Metadata;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.UICommand
{
   public abstract class ExportBuildingBlockToExcelUICommand<T, TBuilder> : ObjectUICommand<T> where T : class, IBuildingBlock
   {
      private readonly IMapper<T, List<DataTable>> _mapper;
      private readonly IInteractionTasksForPathAndValueEntity<T,TBuilder> _interactionTasks;

      protected ExportBuildingBlockToExcelUICommand(
         IInteractionTasksForPathAndValueEntity<T, TBuilder> interactionTasks,
         IMapper<T, List<DataTable>> mapper)
      {
         _interactionTasks = interactionTasks;
         _mapper = mapper;
      }

      protected override void PerformExecute() => _interactionTasks.ExportExcel(Subject, _mapper.MapFrom(Subject));
   }
}