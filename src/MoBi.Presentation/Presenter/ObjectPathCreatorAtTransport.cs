using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   internal interface IObjectPathCreatorAtTransport : IObjectPathCreator
   {
      ITransportBuilder Transport { set; }
   }

   internal class ObjectPathCreatorAtTransport : ObjectPathCreatorBase, IObjectPathCreatorAtTransport
   {
      public ITransportBuilder Transport { private get; set; }

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
      public override ReferenceDTO CreatePathFromParameterDummy(IObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var dtoDummyParameter = (DummyParameterDTO) objectBaseDTO;
         var firstPathElemnt = getPathKeywordForContainer(dtoDummyParameter.Parent.ParentContainer);
         if (firstPathElemnt.IsNullOrEmpty())
            return base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);

         string moleculeElement = shouldCreateAbsolutePaths ? dtoDummyParameter.ModelParentName : ObjectPathKeywords.MOLECULE;
         var parameter = _context.Get<IParameter>(dtoDummyParameter.ParameterToUse.Id);
         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new[] {firstPathElemnt, moleculeElement, dtoDummyParameter.Name}, parameter)
         };
      }

      public override ReferenceDTO CreateMoleculePath(DummyMoleculeContainerDTO dtoObjectBase, bool shouldCreateAbsolutePaths, IEntity refObject)
      {
         var moleculeProperties = _context.Get<IContainer>(dtoObjectBase.MoleculePropertiesContainer.Id);
         var parentContainer = moleculeProperties.ParentContainer;
         var firstPathElemnt = getPathKeywordForContainer(parentContainer);
         if (firstPathElemnt.IsNullOrEmpty())
            return base.CreateMoleculePath(dtoObjectBase, shouldCreateAbsolutePaths, refObject);

         string moleculeElement = shouldCreateAbsolutePaths ? dtoObjectBase.Name : ObjectPathKeywords.MOLECULE;

         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new[] {firstPathElemnt, moleculeElement}, AppConstants.AmountAlias, Constants.Dimension.AMOUNT)
         };
      }

      public override ReferenceDTO CreatePathsFromEntity(IObjectBase objectBase, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var parameter = objectBase as IParameter;
         //Isolated parameter without parent container
         if (parameter != null && parameter.ParentContainer == null)
            return isolatedParameterReferenceFor(parameter);

         return base.CreatePathsFromEntity(objectBase, shouldCreateAbsolutePaths, refObject, editedObject);
      }

      private ReferenceDTO isolatedParameterReferenceFor(IParameter parameter)
      {
         return new ReferenceDTO
         {
            Path = CreateFormulaUsablePathFrom(new[] {parameter.Name}, parameter)
         };
      }

      protected override IFormulaUsablePath CreateRelativePath(IFormulaUsable formulaUsable, IEntity refObject, IUsingFormula editedObject)
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

         if (Transport.TargetCriteria.IsSatisfiedBy(container))
            return ObjectPathKeywords.TARGET;

         if (Transport.SourceCriteria.IsSatisfiedBy(container))
            return ObjectPathKeywords.SOURCE;

         return string.Empty;
      }
   }
}