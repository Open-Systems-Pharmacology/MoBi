using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtParameter : IObjectPathCreator
   {
   }

   internal class ObjectPathCreatorAtParameter : ObjectPathCreatorBase, IObjectPathCreatorAtParameter
   {
      public ObjectPathCreatorAtParameter(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override T AdjustReferences<T>(IEntity entity, T path)
      {
         return path;
      }

      protected IFormulaUsablePath GenerateLocalReference(IFormulaUsable formulaUsable,  IUsingFormula editedObject)
      {
         if (editedObject == null) return null;
         IFormulaUsablePath path;
         if (formulaUsable.ParentContainer.Equals(editedObject.ParentContainer))
         {
            path = _objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, formulaUsable.Name);
         }
         else
         {
            path = _objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER,
               formulaUsable.ParentContainer.Name, formulaUsable.Name);
         }
         path.Dimension = formulaUsable.Dimension;
         path.Alias = formulaUsable.Name;
         return path;
      }
   }

   internal interface IObjectPathCreatorAtReactionParameter : IObjectPathCreatorAtParameter
   {
   }

   internal class ObjectPathCreatorAtReactionParameter : ObjectPathCreatorAtParameter, IObjectPathCreatorAtReactionParameter
   {
      public ObjectPathCreatorAtReactionParameter(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      public override ReferenceDTO CreatePathFromParameterDummy(IObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var dto = base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);
         var dtoDummy = objectBaseDTO as DummyParameterDTO;
         if (dtoDummy != null && !shouldCreateAbsolutePaths)
         {
            // We need here the Molecule name even in relative paths
            dto.Path.Replace(ObjectPathKeywords.MOLECULE, dtoDummy.ModelParentName);
         }
         return dto;
      }

      protected override IFormulaUsablePath CreateRelativePath(IFormulaUsable formulaUsable, IEntity refObject,IUsingFormula editedObject)
      {
         if (formulaUsable.IsAtReaction())
         {
            return GenerateLocalReference(formulaUsable,editedObject);
         }
         return base.CreateRelativePath(formulaUsable, refObject,editedObject);
      }

      protected override T AdjustReferences<T>(IEntity parameterToUse, T path)
      {
         var references = base.AdjustReferences(parameterToUse, path);
         if (references == null) return references;
         references.AddAtFront(ObjectPath.PARENT_CONTAINER);
         references.AddAtFront(ObjectPath.PARENT_CONTAINER);
         return references;
      }
   }

   internal interface IObjectPathCreatorAtMoleculeParameter : IObjectPathCreatorAtParameter
   {
   }

   internal class ObjectPathCreatorAtMoleculeParameter : ObjectPathCreatorAtParameter, IObjectPathCreatorAtMoleculeParameter
   {
      public ObjectPathCreatorAtMoleculeParameter(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override IFormulaUsablePath CreateRelativePath(IFormulaUsable formulaUseable, IEntity refObject, IUsingFormula editedObject)
      {
         IFormulaUsablePath path;

         if (!formulaUseable.IsAtMolecule())
         {
            path = base.CreateRelativePath(formulaUseable, refObject,editedObject);
         }
         else
         {
            path = GenerateLocalReference(formulaUseable, editedObject);
            if (Equals(formulaUseable.ParentContainer, editedObject.ParentContainer))
               path.Replace(editedObject.ParentContainer.Name, ObjectPathKeywords.MOLECULE);
         }


         return path;
      }

      public override ReferenceDTO CreatePathFromParameterDummy(IObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths,
         IEntity refObject, IUsingFormula editedObject)
      {
         var referenceDTO = base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);
         var dtoDummyParameter = (DummyParameterDTO) objectBaseDTO;
         
         if (Equals(dtoDummyParameter.ModelParentName, editedObject.ParentContainer.Name))
            referenceDTO.Path.Replace(editedObject.ParentContainer.Name, ObjectPathKeywords.MOLECULE);
         return referenceDTO;
      }

      protected override T AdjustReferences<T>(IEntity parameterToUse, T path)
      {
         //Add 2 parent references to path, because local molecule parameters are located 2 levels lower then the selected reference point
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         return path;
      }
   }
}