using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ApplicationBuilderDTO : EventGroupBuilderDTO
   {
      private Func<IEnumerable<string>> _getMoleculeNames;
      public IEnumerable<TransportBuilderDTO> Transports { get; set; }
      public IEnumerable<ApplicationMoleculeBuilderDTO> Molecules { get; set; }
      public string MoleculeName { get; set; }

      public ApplicationBuilderDTO(IApplicationBuilder applicationBuilder) : base(applicationBuilder)
      {
         Rules.Add(moleculeNameShouldBePresentInProjectRule);
      }

      private static IBusinessRule moleculeNameShouldBePresentInProjectRule { get; } = CreateRule.For<ApplicationBuilderDTO>()
         .Property(x => x.MoleculeName)
         .WithRule((dto, moleculeName) => dto._getMoleculeNames().Contains(moleculeName))
         .WithError(AppConstants.Exceptions.AppliedMoleculeNotInProject);

      public void GetMoleculeNames(Func<IEnumerable<string>> getMoleculeNames)
      {
         _getMoleculeNames = getMoleculeNames;
      }
   }
}