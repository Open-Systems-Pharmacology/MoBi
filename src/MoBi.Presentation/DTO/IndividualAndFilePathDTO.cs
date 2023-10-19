﻿using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.Presentation.DTO
{
   public class IndividualAndFilePathDTO : ValidatableDTO, IWithName
   {
      public string FilePath { get; set; }
      public IndividualBuildingBlock IndividualBuildingBlock { get; set; }
      public string Name { get; set; }
      public ObjectPath ContainerPath { get; set; }

      public string Description => ExportContainerDescription(ContainerPath?.PathAsString);

      public IndividualAndFilePathDTO()
      {
         Rules.AddRange(AllRules.All);
      }

      private static class AllRules
      {
         public static IReadOnlyList<IBusinessRule> All { get; } = new IBusinessRule[]
         {
            CreateRule.For<IndividualAndFilePathDTO>()
               .Property(x => x.FilePath)
               .WithRule((dto, name) => name.StringIsNotEmpty())
               .WithError(Validation.ValueIsRequired),

            CreateRule.For<IndividualAndFilePathDTO>()
               .Property(x => x.IndividualBuildingBlock)
               .WithRule((dto, buildingBlock) => buildingBlock != null)
               .WithError(Validation.ValueIsRequired)
         };
      }
   }
}