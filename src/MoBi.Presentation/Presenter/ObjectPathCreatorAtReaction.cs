using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtReaction : IObjectPathCreator
   {
      ReactionBuilder ProcessBuilder { set; }
   }

   public class ObjectPathCreatorAtReaction : ObjectPathCreatorAtProcessBuilder<ReactionBuilder>, IObjectPathCreatorAtReaction
   {
      public ObjectPathCreatorAtReaction(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      public override ReferenceDTO CreatePathFromParameterDummy(ObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var dummyParameterDTO = objectBaseDTO as DummyParameterDTO;
         var dto = base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);

         // We need here the Molecule name even in relative paths
         if (dummyParameterDTO != null && !shouldCreateAbsolutePaths)
         {
            dto.Path.Replace(ObjectPathKeywords.MOLECULE, dummyParameterDTO.ModelParentName);
         }

         return dto;
      }

      public override ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         if (objectBase is IParameter parameter && parameter.IsAtReaction())
         {
            // we need a special treatment for parameters defined at a reaction
            return new ReferenceDTO { Path = CreatePathToProcessProperty(parameter) };
         }

         return base.CreatePathsFromEntity(objectBase, shouldCreateAbsolutePaths, refObject, editedObject);
      }

      protected override T AdjustReferences<T>(IEntity parameterToUse, T path) => path.AndAddAtFront(ObjectPath.PARENT_CONTAINER);

      protected override ReactionBuilder GetProcessBuilderFor(IParameter parameter) => GetReactionBuilderFor(parameter);
   }
}