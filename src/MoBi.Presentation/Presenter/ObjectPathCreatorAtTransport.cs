using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtTransport : IObjectPathCreator
   {
      TransportBuilder ProcessBuilder { set; }
   }

   internal class ObjectPathCreatorAtTransport : ObjectPathCreatorAtProcessBuilder<TransportBuilder>, IObjectPathCreatorAtTransport
   {
      public ObjectPathCreatorAtTransport(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      /// <summary>
      ///    Creates the path from parameter dummy.
      /// </summary>
      /// <param name="objectBaseDTO">The dummy parameter dto.</param>
      /// <param name="shouldCreateAbsolutePaths">if set to <c>true</c>  creates absolute paths else creates relative paths.</param>
      /// <param name="refObject">
      ///    The reference object the user chosen (may the concrete object that uses the reference or a
      ///    existing parent of it).
      /// </param>
      /// <param name="editedObject"></param>
      /// <returns>
      ///    The path that could be uses in the model to reference the object
      /// </returns>
      public override ReferenceDTO CreatePathFromParameterDummy(ObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var dtoDummyParameter = (DummyParameterDTO)objectBaseDTO;
         var firstPathElement = getPathKeywordForContainer(dtoDummyParameter.Parent.ParentContainer);
         if (firstPathElement.IsNullOrEmpty() || shouldCreateAbsolutePaths)
         {
            var dto = base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);
            if (!shouldCreateAbsolutePaths)
               dto.Path.Replace(dtoDummyParameter.ModelParentName, ObjectPathKeywords.MOLECULE);

            return dto;
         }

         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new[] { firstPathElement, ObjectPathKeywords.MOLECULE, dtoDummyParameter.Name }, dtoDummyParameter.Parameter)
         };
      }

      public override ReferenceDTO CreateMoleculePath(DummyMoleculeContainerDTO dtoObjectBase, bool shouldCreateAbsolutePaths, IEntity refObject)
      {
         var moleculeProperties = _context.Get<IContainer>(dtoObjectBase.MoleculePropertiesContainer.Id);
         var parentContainer = moleculeProperties.ParentContainer;
         var firstPathElement = getPathKeywordForContainer(parentContainer);
         if (firstPathElement.IsNullOrEmpty() || shouldCreateAbsolutePaths)
         {
            var dto = base.CreateMoleculePath(dtoObjectBase, shouldCreateAbsolutePaths, refObject);
            if (!shouldCreateAbsolutePaths)
               dto?.Path.Replace(dtoObjectBase.Name, ObjectPathKeywords.MOLECULE);
            return dto;
         }

         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new[] { firstPathElement, ObjectPathKeywords.MOLECULE }, AppConstants.AmountAlias, Constants.Dimension.MOLAR_AMOUNT)
         };
      }

      public override ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         if (objectBase is IParameter parameter)
         {
            //Isolated parameter without parent container
            if (parameter.ParentContainer == null)
               return isolatedParameterReferenceFor(parameter);

            if (parameter.IsAtTransport())
               return new ReferenceDTO { Path = CreatePathToProcessProperty(parameter) };
         }

         return base.CreatePathsFromEntity(objectBase, shouldCreateAbsolutePaths, refObject, editedObject);
      }

      private ReferenceDTO isolatedParameterReferenceFor(IParameter parameter)
      {
         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new[] { parameter.Name }, parameter)
         };
      }

      protected override FormulaUsablePath CreateRelativePath(IFormulaUsable formulaUsable, IEntity refObject, IUsingFormula editedObject)
      {
         if (refObject == null) return null;
         var container = formulaUsable.ParentContainer;

         var keywordForContainer = getPathKeywordForContainer(container);

         if (keywordForContainer.IsNullOrEmpty())
            //we have a parameter that is not located at Target or Source so we create a normal relative path
            return base.CreateRelativePath(formulaUsable, refObject, editedObject);

         // We have a target or source parameter in use
         return _objectPathFactory.CreateRelativeFormulaUsablePath(container, formulaUsable)
            .AndAddAtFront(keywordForContainer);
      }

      protected override T AdjustReferences<T>(IEntity parameterToUse, T path)
      {
         // add two parent references cause tranports are created under MOLECLE Containier in neighborhood, neighborhood is ref object
         return path
            .AndAddAtFront(ObjectPath.PARENT_CONTAINER)
            .AndAddAtFront(ObjectPath.PARENT_CONTAINER);
      }

      /// <summary>
      ///    Gets the path keyword for source or target container if the container matches conditions.
      /// </summary>
      /// <param name="container">The container.</param>
      /// <returns>the correct keyword, String.Empty if no condition matched </returns>
      private string getPathKeywordForContainer(IContainer container)
      {
         if (container == null)
            return string.Empty;

         if (ProcessBuilder.TargetCriteria.IsSatisfiedBy(container))
            return ObjectPathKeywords.TARGET;

         if (ProcessBuilder.SourceCriteria.IsSatisfiedBy(container))
            return ObjectPathKeywords.SOURCE;

         return string.Empty;
      }

      protected override TransportBuilder GetProcessBuilderFor(IParameter parameter)
      {
         if (parameter.IsAtTransport())
            return parameter.RootContainer as TransportBuilder;

         throw new MoBiException(AppConstants.Exceptions.CannotFindTransportForParameter(parameter.Name));
      }
   }
}