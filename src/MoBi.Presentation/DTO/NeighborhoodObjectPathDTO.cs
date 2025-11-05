using System;
using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodObjectPathDTO : DxValidatableDTO, IViewItem
   {
      private readonly Func<NeighborhoodObjectPathDTO> _myNeighbor;
      private ContainerMode? _myMode;
      private string _path;

      public virtual ContainerMode? Mode
      {
         get => _myMode;
         set => SetProperty(ref _myMode, value);
      }

      public virtual string Path
      {
         get => _path;
         set => SetProperty(ref _path, value);
      }

      public NeighborhoodBuilderDTO Neighborhood { get; }

      public NeighborhoodObjectPathDTO(NeighborhoodBuilderDTO neighborhood, Func<NeighborhoodObjectPathDTO> myNeighbor)
      {
         _myNeighbor = myNeighbor;
         Neighborhood = neighborhood;
         Rules.AddRange(AllRules.All);
      }

      public override string ToString()
      {
         return Path;
      }

      private static class AllRules
      {
         private static IBusinessRule notEmptyPathRule { get; } = GenericRules.NonEmptyRule<NeighborhoodObjectPathDTO>(x => x.Path, AppConstants.Validation.EmptyPath);

         private static IBusinessRule noEquivalentNeighborhood { get; } = CreateRule.For<NeighborhoodObjectPathDTO>()
            .Property(x => x.Path)
            .WithRule((dto, path) => !dto.Neighborhood.HasConnectionBetween(path, dto._myNeighbor().Path))
            .WithError((dto, path) => AppConstants.Validation.HasEquivalentNeighborhood(path, dto._myNeighbor().Path));

         private static IBusinessRule neighborsAreNotEqual { get; } = CreateRule.For<NeighborhoodObjectPathDTO>()
            .Property(x => x.Path)
            .WithRule((dto, path) => !Equals(path, dto._myNeighbor().Path))
            .WithError((dto, path) => AppConstants.Validation.CannotCreateANeighborhoodThatConnectsAContainerToItself);

         private static IBusinessRule neighborsArePhysical { get; } = CreateRule.For<NeighborhoodObjectPathDTO>()
               .Property(x => x.Path) 
               .WithRule((dto, path) => dto.Mode != ContainerMode.Logical)
               .WithError((dto, _) => AppConstants.Validation.CannotCreateANeighborhoodFromLogicalContainers);

         public static IReadOnlyList<IBusinessRule> All { get; } = new[]
         {
            notEmptyPathRule,
            noEquivalentNeighborhood,
            neighborsAreNotEqual,
            neighborsArePhysical
         };
      }
   }
}