using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   internal interface IObjectPathCreatorAtMoleculeStartValue:IObjectPathCreator
   {
   }

   class ObjectPathCreatorAtMoleculeStartValue : ObjectPathCreatorBase, IObjectPathCreatorAtMoleculeStartValue
   {
      public ObjectPathCreatorAtMoleculeStartValue(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      public override ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var parameter = objectBase as IParameter;
         // we need a special treatment for parameters defined at the molecule
         if (parameter != null && parameter.IsAtMolecule())
         {
            var dto = new ReferenceDTO();
            dto.Path = cratePathToMoleculeProperty(parameter, shouldCreateAbsolutePaths, refObject);
            dto.BuildMode = parameter.BuildMode;
            return dto;
         }
         return base.CreatePathsFromEntity(objectBase, shouldCreateAbsolutePaths, refObject, editedObject);
      }

      

      private IFormulaUsablePath cratePathToMoleculeProperty(IParameter parameter, bool shouldCreateAbsolutePaths, IEntity refObject)
      {
         // global molecule properties are alwas refercend absolute
         if (parameter.BuildMode != ParameterBuildMode.Local)
            return _objectPathFactory.CreateFormulaUsablePathFrom(ObjectPathKeywords.MOLECULE, parameter.Name)
               .WithAlias(_aliasCreator.CreateAliasFrom(parameter.Name))
               .WithDimension(parameter.Dimension);

         if (shouldCreateAbsolutePaths)
         {
            if (refObject == null)
               return null;
            // Absolute path to local property so we take refObjects absolute path and add molecule and parameter name.
            return _objectPathFactory.CreateFormulaUsablePathFrom(_objectPathFactory.CreateAbsoluteObjectPath(refObject))
               .WithAlias(_aliasCreator.CreateAliasFrom(parameter.Name))
               .WithDimension(parameter.Dimension)
               .AndAdd(parameter.ParentContainer.Name)
               .AndAdd(parameter.Name);
         }
         else
         {
            return _objectPathFactory.CreateRelativeFormulaUsablePath(parameter.RootContainer, parameter);
         }
      }

      protected override T AdjustReferences<T>(IEntity entity, T path) 
      {
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         return path;
      }
   }
}