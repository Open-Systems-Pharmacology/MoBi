using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class TransportBuilderDTO : ObjectBaseDTO
   {
      public ITransportBuilder TransporBuilder { get; private set; }

      public TransportBuilderDTO(ITransportBuilder transportBuilder)
      {
         TransporBuilder = transportBuilder;
      }

      public FormulaBuilderDTO Formula { get; set; }
      public IEnumerable<ParameterDTO> Parameters { get; set; }
      public TransportType TransportType { get; set; }

      public bool CreateProcessRateParameter
      {
         get { return TransporBuilder.CreateProcessRateParameter; }
         set { TransporBuilder.CreateProcessRateParameter = value; }
      }

      public bool ProcessRateParameterPersistable
      {
         get { return TransporBuilder.ProcessRateParameterPersistable; }
         set { TransporBuilder.ProcessRateParameterPersistable = value; }
      }
   }
}