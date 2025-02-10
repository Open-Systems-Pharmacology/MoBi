using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;


namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtEventAssignment : IObjectPathCreator
   {
   }

   internal class ObjectPathCreatorAtEventAssignment : ObjectPathCreatorAtEventBase, IObjectPathCreatorAtEventAssignment
   {

      public ObjectPathCreatorAtEventAssignment(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override T AdjustReferences<T>(IEntity entity, T path)
      {
         return path;
      }
   }
}