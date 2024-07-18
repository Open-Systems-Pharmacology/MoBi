using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Presenter
{
   internal interface IObjectPathCreatorAtEvent : IObjectPathCreator
   {
   }

   internal abstract class ObjectPathCreatorAtEventBase : ObjectPathCreatorBase, IObjectPathCreatorAtEvent
   {
      protected ObjectPathCreatorAtEventBase(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      public override ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var entity = objectBase as IFormulaUsable;
         var dtoReference = new ReferenceDTO();
         if (entity == null) return dtoReference;
         // relative paths are only used for parameter in the same event group
         if (refObject != null && refObject != null && entity.RootContainer.Equals(refObject.RootContainer))
         {
            dtoReference.Path = CreateRelativePath(entity, refObject, editedObject);
         }
         else
         {
            dtoReference.Path = CreateAbsolutePath(entity);
         }
         return dtoReference;
      }

      public override ReferenceDTO CreatePathFromParameterDummy(ObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);
         var dtoMoleculeParameter = objectBaseDTO as DummyParameterDTO;
         var dto = base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);
         if (dtoMoleculeParameter != null && !shouldCreateAbsolutePaths)
         {
            // We need here the Molecule name even in relative paths
            dto.Path.Replace(ObjectPathKeywords.MOLECULE, dtoMoleculeParameter.ModelParentName);
         }
         return dto;
      }
   }

   internal class ObjectPathCreatorAtEvent : ObjectPathCreatorAtEventBase
   {
      private readonly IReactionDimensionRetriever _dimensionRetriever;

      public ObjectPathCreatorAtEvent(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context, IReactionDimensionRetriever dimensionRetriever)
         : base(objectPathFactory, aliasCreator, context)
      {
         _dimensionRetriever = dimensionRetriever;
      }

      protected override FormulaUsablePath CreateRelativePath(IFormulaUsable formulaUsable, IEntity refObject, IUsingFormula editedObject)
      {
         // relative paths are only used for parameter in the same event group
         if (refObject != null && formulaUsable.RootContainer.Equals(refObject.RootContainer))
         {
            FormulaUsablePath path;
            if (formulaUsable.IsAtMolecule())
            {
               if (formulaUsable.IsAnImplementationOf<IParameter>())
               {
                  path = _objectPathFactory.CreateAbsoluteFormulaUsablePath(formulaUsable)
                     .AndAddAtFront((ObjectPath.PARENT_CONTAINER));
               }
               else
               {
                  //Molecule replace With Amount
                  path = CreateFormulaUsablePathFrom(new[] {ObjectPath.PARENT_CONTAINER, formulaUsable.Name}, formulaUsable.Name, Constants.Dimension.MOLAR_AMOUNT);
               }
               correctPath(path, refObject as IContainer);
            }
            else
            {
               if (formulaUsable.IsAtReaction())
               {
                  path = CreateFormulaUsablePathFrom(new[] {formulaUsable.Name}, formulaUsable);
                  correctPath(path, refObject as IContainer);
               }
               else
               {
                  path = base.CreateRelativePath(formulaUsable, refObject, editedObject);
               }
            }
            return path;
         }

         return CreateAbsolutePath(formulaUsable);
      }

      protected override T AdjustReferences<T>(IEntity entity, T path)
      {
         return path;
      }

      private void correctPath(FormulaUsablePath path, IContainer container)
      {
         while (container != null && container.Mode.Equals(ContainerMode.Physical))
         {
            container = container.ParentContainer;
            path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         }
      }
   }
}