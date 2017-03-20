using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class TransporterMoleculeContainerDTO : ObjectBaseDTO
   {
      public IEnumerable<TransportBuilderDTO> Realizations { get; set; }
      public IEnumerable<ParameterDTO> Parameters { get; set; }
      public string TransportName { get; set; }

      public TransporterMoleculeContainerDTO()
      {
         Rules.Add(notEmptyTransportNameRule());
         Rules.Add(uniqueTransportNameRule());
      }

      private static IBusinessRule notEmptyTransportNameRule()
      {
         return CreateRule.For<TransporterMoleculeContainerDTO>()
            .Property(x => x.TransportName)
            .WithRule((dto, name) => !name.Trim().IsNullOrEmpty())
            .WithError(AppConstants.Validation.EmptyTransportName);
      }

      private static IBusinessRule uniqueTransportNameRule()
      {
         return CreateRule.For<TransporterMoleculeContainerDTO>()
            .Property(x => x.TransportName)
            .WithRule((dto, name) => dto.IsNameUnique(name))
            .WithError(AppConstants.Validation.TransportNameAllreadyUsed);
      }
   }
}