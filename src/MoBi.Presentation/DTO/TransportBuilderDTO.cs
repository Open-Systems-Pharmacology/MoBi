using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class TransportBuilderDTO : ObjectBaseDTO
   {
      public ITransportBuilder TransportBuilder { get; }

      public TransportBuilderDTO(ITransportBuilder transportBuilder):base(transportBuilder)  
      {
         TransportBuilder = transportBuilder;
      }

      public FormulaBuilderDTO Formula { get; set; }
      public IEnumerable<ParameterDTO> Parameters { get; set; }
      public TransportType TransportType { get; set; }

      public bool CreateProcessRateParameter
      {
         get => TransportBuilder.CreateProcessRateParameter;
         set => TransportBuilder.CreateProcessRateParameter = value;
      }

      public bool ProcessRateParameterPersistable
      {
         get => TransportBuilder.ProcessRateParameterPersistable;
         set => TransportBuilder.ProcessRateParameterPersistable = value;
      }
   }
}