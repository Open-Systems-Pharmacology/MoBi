using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public class OutputIntervalMapper : OSPSuite.Core.Snapshots.Mappers.OutputIntervalMapper
{
   private readonly IOutputIntervalFactory _intervalFactory;

   public OutputIntervalMapper(ParameterMapper parameterMapper, IOutputIntervalFactory intervalFactory) : base(parameterMapper)
   {
      _intervalFactory = intervalFactory;
   }

   protected override OutputInterval CreateDefault()
   {
      return _intervalFactory.CreateDefault();
   }
}