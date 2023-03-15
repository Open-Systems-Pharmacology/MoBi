using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class TransporterMoleculeContainerDTO : ObjectBaseDTO
   {
      public IEnumerable<TransportBuilderDTO> Realizations { get; set; }
      public IEnumerable<ParameterDTO> Parameters { get; set; }
      public string TransportName { get; set; }

      public TransporterMoleculeContainerDTO(TransporterMoleculeContainer transporterMoleculeContainer) :base(transporterMoleculeContainer)
      {
         Rules.Add(notEmptyTransportNameRule);
         Rules.Add(uniqueTransportNameRule);
      }

      private static IBusinessRule notEmptyTransportNameRule { get; } = CreateRule.For<TransporterMoleculeContainerDTO>()
         .Property(x => x.TransportName)
         .WithRule((dto, name) => !name.Trim().IsNullOrEmpty())
         .WithError(AppConstants.Validation.EmptyTransportName);

      private static IBusinessRule uniqueTransportNameRule { get; } = CreateRule.For<TransporterMoleculeContainerDTO>()
         .Property(x => x.TransportName)
         .WithRule((dto, name) => dto.IsNameUnique(name))
         .WithError(AppConstants.Validation.TransportNameAlreadyUsed);
   }
}