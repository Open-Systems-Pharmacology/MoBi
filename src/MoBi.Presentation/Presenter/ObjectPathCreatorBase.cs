using System;
using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreator
   {
      ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject);

      /// <summary>
      ///    Creates the path from parameter dummy.
      /// </summary>
      /// <param name="objectBaseDTO">The dummy parameter dto.</param>
      /// <param name="shouldCreateAbsolutePaths">if set to <c>true</c>  creates absolute paths else creates reltive paths.</param>
      /// <param name="refObject">
      ///    The reference object the user chosen (may the concrete object that uses the reference or a
      ///    existing parent of it).
      /// </param>
      /// <param name="editedObject"></param>
      /// <returns> The path that could be used in the model to reference the object</returns>
      ReferenceDTO CreatePathFromParameterDummy(ObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject);

      ReferenceDTO CreateMoleculePath(DummyMoleculeContainerDTO dtoObjectBase, bool shouldCreateAbsolutePaths, IEntity refObject);
   }

   public abstract class ObjectPathCreatorBase : IObjectPathCreator
   {
      protected readonly IAliasCreator _aliasCreator;
      protected readonly IMoBiContext _context;
      protected readonly IObjectPathFactory _objectPathFactory;

      protected ObjectPathCreatorBase(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context)
      {
         _objectPathFactory = objectPathFactory;
         _context = context;
         _aliasCreator = aliasCreator;
      }

      /// <summary>
      ///    Creates the path from entity.
      /// </summary>
      /// <param name="objectBase">The object base that should be referenced</param>
      /// <param name="shouldCreateAbsolutePaths">if set to <c>true</c> should create absolute paths otherwise relative paths are created.</param>
      /// <param name="refObject">The object from which the path is created.</param>
      /// <param name="editedObject"></param>
      /// <returns> the dto Object for the reference</returns>
      public virtual ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var dto = new ReferenceDTO();
         if (!objectBase.IsAnImplementationOf<IFormulaUsable>()) return null;
         var formulaUseable = objectBase.DowncastTo<IFormulaUsable>();
         if (isGlobal(objectBase))
         {
            // This a global parameters that we always use as absolute paths
            dto.Path = CreateAlwaysAbsolutePaths(objectBase, formulaUseable);
         }
         else
         {
            // local reaction and molecule properties are always referenced local. 
            if (formulaUseable.IsAtReaction() || formulaUseable.IsAtMolecule())
            {
               shouldCreateAbsolutePaths = false;
            }
            dto.Path = shouldCreateAbsolutePaths
               ? CreateAbsolutePath(formulaUseable)
               : CreateRelativePath(formulaUseable, refObject, editedObject);
         }
         var parameter = formulaUseable as IParameter;
         if (parameter == null)
            return dto;

         dto.BuildMode = parameter.BuildMode;
         updateReferenceForTransportMoleculeContainer(dto, parameter, shouldCreateAbsolutePaths);
         return dto;
      }

      private void updateReferenceForTransportMoleculeContainer(ReferenceDTO dto, IParameter parameter, bool shouldCreateAbsolutePaths)
      {
         var transporterMoleculeContainer = parameter.ParentContainer as TransporterMoleculeContainer;
         if (transporterMoleculeContainer == null) return;

         dto.Path.Replace(transporterMoleculeContainer.Name, shouldCreateAbsolutePaths ? transporterMoleculeContainer.TransportName : ObjectPathKeywords.TRANSPORTER);
      }

      /// <summary>
      ///    Creates the path from parameter dummy.
      /// </summary>
      /// <param name="objectBaseDTO">The dummy parameter dto.</param>
      /// <param name="shouldCreateAbsolutePaths">if set to <c>true</c>  creates absolute paths otherwise creates reltive paths.</param>
      /// <param name="refObject">
      ///    The reference object the user chosen (may the concrete object that uses the reference or a
      ///    existing parent of it).
      /// </param>
      /// <param name="editedObject"></param>
      /// <returns> The path that could be uses in the model to reference the object</returns>
      public virtual ReferenceDTO CreatePathFromParameterDummy(ObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         if (IsMoleculeReference(objectBaseDTO))
            return createMoleculeReference();

         var dtoDummyParameter = (DummyParameterDTO) objectBaseDTO;
         var parameterToUse = _context.Get<IParameter>(dtoDummyParameter.ParameterToUse.Id);
         ReferenceDTO dtoReference;
         if (parameterToUse.IsAtMolecule())
         {
            dtoReference = new ReferenceDTO();
            ObjectPath tmpPath;
            //global molecule parameters we always reference absolute
            if (shouldCreateAbsolutePaths || !parameterToUse.BuildMode.Equals(ParameterBuildMode.Local))
            {
               tmpPath = _objectPathFactory.CreateAbsoluteObjectPath(dtoDummyParameter.Parent);
            }
            else
            {
               if (refObject != dtoDummyParameter.Parent)
               {
                  tmpPath = _objectPathFactory.CreateRelativeObjectPath(refObject, dtoDummyParameter.Parent);
               }
               else
               {
                  tmpPath = new ObjectPath();
               }
               
               tmpPath = AdjustReferences(parameterToUse, tmpPath);
            }
            dtoReference.Path = _objectPathFactory.CreateFormulaUsablePathFrom(tmpPath)
               .WithAlias(_aliasCreator.CreateAliasFrom(parameterToUse.Name))
               .WithDimension(parameterToUse.Dimension);
            dtoReference.Path.Add(parameterToUse.Name);
         }
         else
         {
            dtoReference = CreatePathsFromEntity(parameterToUse, shouldCreateAbsolutePaths, refObject, editedObject);
         }
         dtoReference.Path.Replace(Constants.MOLECULE_PROPERTIES, dtoDummyParameter.ModelParentName);
         dtoReference.BuildMode = parameterToUse.BuildMode;
         return dtoReference;
      }

      public virtual ReferenceDTO CreateMoleculePath(DummyMoleculeContainerDTO dtoObjectBase, bool shouldCreateAbsolutePaths, IEntity refObject)
      {
         var moleculeProperties = _context.Get<IContainer>(dtoObjectBase.MoleculePropertiesContainer.Id);
         var parentContainer = moleculeProperties.ParentContainer;
         if (parentContainer == null) return null;
         ObjectPath elements;
         FormulaUsablePath path;
         if (shouldCreateAbsolutePaths)
         {
            elements = _objectPathFactory.CreateAbsoluteObjectPath(parentContainer);
            path = _objectPathFactory.CreateFormulaUsablePathFrom(elements);
         }
         else
         {
            if (refObject == parentContainer)
            {
               path = new FormulaUsablePath();
            }
            else
            {
               elements = _objectPathFactory.CreateRelativeObjectPath(refObject, parentContainer);
               path = _objectPathFactory.CreateFormulaUsablePathFrom(elements);
            }
            AdjustReferences(parentContainer, path);
         }

         path.Alias = _aliasCreator.CreateAliasFrom(dtoObjectBase.Name);
         path.Dimension = _context.DimensionFactory.Dimension(Constants.Dimension.MOLAR_AMOUNT);
         path.Add(dtoObjectBase.Name);

         return new ReferenceDTO {Path = path};
      }

      private ReferenceDTO createMoleculeReference()
      {
         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new [] { ObjectPath.PARENT_CONTAINER }, AppConstants.AmountAlias, Constants.Dimension.MOLAR_AMOUNT)
         };
      }

      protected FormulaUsablePath CreateFormulaUsablePathFrom(IReadOnlyCollection<string> paths, IFormulaUsable formulaUsable)
      {
         return CreateFormulaUsablePathFrom(paths, formulaUsable.Name, formulaUsable.Dimension);
      }

      protected FormulaUsablePath CreateFormulaUsablePathFrom(IReadOnlyCollection<string> paths , string alias, string dimensionName)
      {
         return CreateFormulaUsablePathFrom(paths, alias, _context.DimensionFactory.Dimension(dimensionName));
      }

      protected FormulaUsablePath CreateFormulaUsablePathFrom(IReadOnlyCollection<string> paths, string alias, IDimension dimension)
      {
         return _objectPathFactory.CreateFormulaUsablePathFrom(paths)
            .WithAlias(_aliasCreator.CreateAliasFrom(alias))
            .WithDimension(dimension);
      }

      protected virtual FormulaUsablePath CreateRelativePath(IFormulaUsable formulaUsable, IEntity refObject, IUsingFormula editedObject)
      {
         var path = createRelativePathBase(formulaUsable, refObject);
         if (path == null)
            return null;

         path.Replace(Constants.MOLECULE_PROPERTIES, ObjectPathKeywords.MOLECULE);
         return AdjustReferences(formulaUsable, path);
      }

      protected abstract T AdjustReferences<T>(IEntity entity, T path) where T : ObjectPath;

      private bool isGlobal(IObjectBase objectBase)
      {
         var parameter = objectBase as IParameter;
         return parameter != null && parameter.BuildMode != ParameterBuildMode.Local;
      }

      protected FormulaUsablePath CreateAlwaysAbsolutePaths(IObjectBase objectBase, IFormulaUsable formulaUsable)
      {
         if (formulaUsable.IsAtMolecule())
            return _objectPathFactory.CreateAbsoluteFormulaUsablePath(formulaUsable);

         return CreateFormulaUsablePathFrom(new[] {getReactionNameFor(formulaUsable), objectBase.Name}, formulaUsable);
      }

      private FormulaUsablePath createRelativePathBase(IFormulaUsable formulaUsable, IEntity refObject)
      {
         if (refObject == null)
            return null;

         if (formulaUsable.IsAtMolecule() && formulaUsable.RootContainer.Equals(formulaUsable.ParentContainer))
            return CreateFormulaUsablePathFrom(new[] {formulaUsable.ParentContainer.Name, formulaUsable.Name}, formulaUsable);

         return _objectPathFactory.CreateRelativeFormulaUsablePath(refObject, formulaUsable);
      }

      protected FormulaUsablePath CreateAbsolutePath(IFormulaUsable formulaUsable)
      {
         return _objectPathFactory.CreateAbsoluteFormulaUsablePath(formulaUsable);
      }

      private string getReactionNameFor(IFormulaUsable formulaUsable)
      {
         return GetReactionBuilderFor(formulaUsable).Name;
      }

      protected IReactionBuilder GetReactionBuilderFor(IFormulaUsable formulaUsable)
      {
         var parameter = formulaUsable as IParameter;
         if (parameter != null && parameter.IsAtReaction())
            return parameter.RootContainer as IReactionBuilder;
         
         throw new MoBiException($"cant find reaction for parameter{formulaUsable.Name}");
      }

      protected static bool IsMoleculeReference(ObjectBaseDTO dtoObjectBase)
      {
         return dtoObjectBase.Id.Equals(ObjectPathKeywords.MOLECULE);
      }
   }
}