using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{

   public interface IObjectPathCreatorAtMoleculeApplicationBuilder:IObjectPathCreator { }
   class ObjectPathCreatorAtMoleculeApplicationBuilder : ObjectPathCreatorBase, IObjectPathCreatorAtMoleculeApplicationBuilder
   {
      public ObjectPathCreatorAtMoleculeApplicationBuilder(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override T AdjustReferences<T>(IEntity entity, T path)
      {
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         return path;
      }
   }
}