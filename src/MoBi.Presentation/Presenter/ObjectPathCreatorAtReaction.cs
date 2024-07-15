using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtReaction : IObjectPathCreator
   {
      ReactionBuilder Reaction { set; }
   }

   class ObjectPathCreatorAtReaction : ObjectPathCreatorBase, IObjectPathCreatorAtReaction
   {
      public ReactionBuilder Reaction { private get; set; }

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
         var parameter = objectBase as IParameter;

         if (parameter != null && parameter.IsAtReaction())
         {
            // we need a specials treatment for parameters defined at a reaction
            return new ReferenceDTO {Path = cratePathToReacionProperty(parameter)};
         }

         return base.CreatePathsFromEntity(objectBase, shouldCreateAbsolutePaths, refObject, editedObject);
      }

      private FormulaUsablePath cratePathToReacionProperty(IParameter parameter)
      {
         var parentReaction = GetReactionBuilderFor(parameter);

         // Always Absolute paths
         if (parameter.BuildMode != ParameterBuildMode.Local)
            return CreateFormulaUsablePathFrom(new[] {parentReaction.Name, parameter.Name}, parameter);

         // Parameter is child of reaction
         if (parentReaction == Reaction)
            return CreateFormulaUsablePathFrom(new[] {parameter.Name}, parameter);

         // go from reaction to other reaction and then to child parameter
         return CreateFormulaUsablePathFrom(new[] {ObjectPath.PARENT_CONTAINER, parentReaction.Name, parameter.Name}, parameter);
      }

      protected override T AdjustReferences<T>(IEntity parameterToUse, T path)
      {
         return path.AndAddAtFront(ObjectPath.PARENT_CONTAINER);
      }
   }
}