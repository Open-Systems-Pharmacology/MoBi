using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class TransportBuilderDTO : ObjectBaseDTO
   {
      public TransportBuilder TransportBuilder { get; }

      public TransportBuilderDTO(TransportBuilder transportBuilder):base(transportBuilder)  
      {
         TransportBuilder = transportBuilder;
      }

      public FormulaBuilderDTO Formula { get; set; }
      public IEnumerable<ParameterDTO> Parameters { get; set; }
      public TransportType TransportType { get; set; }

      public bool CreateProcessRateParameter => TransportBuilder.CreateProcessRateParameter;

      public bool ProcessRateParameterPersistable
      {
         get => TransportBuilder.ProcessRateParameterPersistable;
         set => TransportBuilder.ProcessRateParameterPersistable = value;
      }
   }
}