using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class EntityExtensions
   {
      public static bool IsAtMolecule(this IEntity entity)
      {
         return entity.IsAtMoleculeBuilder() || (entity.ParentContainer != null && entity.ParentContainer.IsMoleculeProperties());
      }

      public static bool IsMoleculeProperties(this IContainer container)
      {
         return container.IsNamed(Constants.MOLECULE_PROPERTIES);
      }

      public static bool IsAtMoleculeBuilder(this IEntity entity)
      {
         return entity.RootContainer.IsAnImplementationOf<MoleculeBuilder>();
      }

      public static bool IsAtReaction(this IEntity entity)
      {
         var parent = entity.ParentContainer;
         if (parent == null)
            return false;

         return parent.IsAnImplementationOf<Reaction>() ||
                parent.IsAnImplementationOf<ReactionBuilder>() ||
                parent.ContainerType == ContainerType.Reaction;
      }
   }
}