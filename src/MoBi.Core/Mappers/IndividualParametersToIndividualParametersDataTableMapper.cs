using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using System.Collections.Generic;
using System.Data;

namespace MoBi.Core.Mappers
{
   public interface IIndividualParametersToIndividualParametersDataTableMapper : IMapper<IEnumerable<IndividualParameter>, List<DataTable>>
   {

   }

   public class IndividualParametersToIndividualParametersDataTableMapper : ParametersToDataTableMapper<IndividualParameter>, IIndividualParametersToIndividualParametersDataTableMapper
   {
      protected override string TableName => AppConstants.Captions.IndividualParameters;
   }
}