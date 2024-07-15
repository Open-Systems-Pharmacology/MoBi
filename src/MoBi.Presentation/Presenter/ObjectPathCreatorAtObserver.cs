using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtObserver : IObjectPathCreator
   {
   }

   internal class ObjectPathCreatorAtObserver : ObjectPathCreatorBase, IObjectPathCreatorAtObserver
   {
      public ObjectPathCreatorAtObserver(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override T AdjustReferences<T>(IEntity entity, T path) 
      {
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         return path;
      }
   }

   internal interface IObjectPathCreatorAtAmountObserver : IObjectPathCreatorAtObserver
   {
   }

   internal class ObjectPathCreatorAtAmountObserver : ObjectPathCreatorAtObserver, IObjectPathCreatorAtAmountObserver
   {
      public ObjectPathCreatorAtAmountObserver(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context)
         : base(objectPathFactory, aliasCreator, context)
      {
      }
      
      public override ReferenceDTO CreatePathFromParameterDummy(ObjectBaseDTO objectBaseDTO, bool shouldCreateAbsolutePaths, IEntity refObject, IUsingFormula editedObject)
      {
         var dtoReference = base.CreatePathFromParameterDummy(objectBaseDTO, shouldCreateAbsolutePaths, refObject, editedObject);
         if (!shouldCreateAbsolutePaths && !IsMoleculeReference(objectBaseDTO))
         {
            var dtoDummyParameter = (DummyParameterDTO) objectBaseDTO;
            correctMoleculeReferences(dtoDummyParameter.ModelParentName, dtoDummyParameter.Parameter, dtoReference.Path);
         }
         return dtoReference;
      }

      private void correctMoleculeReferences<T>(string moleculeName,IEntity entity,T path) where T : ObjectPath
      {
         if (!entity.IsAtMolecule()) 
            return;

         if (!path.Contains(moleculeName)) 
            return;

         path.Replace(moleculeName, ObjectPathKeywords.MOLECULE);
      }
   }
}