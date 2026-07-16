using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Snapshots.Mappers;

public class OutputSchemaMapper : OSPSuite.Core.Snapshots.Mappers.OutputSchemaMapper
{
   private readonly IObjectBaseFactory _objectBaseFactory;

   public OutputSchemaMapper(OutputIntervalMapper outputIntervalMapper, IContainerTask containerTask, IObjectBaseFactory objectBaseFactory) : base(outputIntervalMapper, containerTask)
   {
      _objectBaseFactory = objectBaseFactory;
   }

   protected override OutputSchema CreateEmpty() => _objectBaseFactory.Create<OutputSchema>();
}