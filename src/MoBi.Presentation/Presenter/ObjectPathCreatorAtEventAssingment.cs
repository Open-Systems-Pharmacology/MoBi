using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectPathCreatorAtEventAssingment : IObjectPathCreator
   {
   }

   internal class ObjectPathCreatorAtEventAssingment : ObjectPathCreatorAtEventBase, IObjectPathCreatorAtEventAssingment
   {

      public ObjectPathCreatorAtEventAssingment(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override T AdjustReferences<T>(IEntity entity, T path)
      {
         return path;
      }
   }
}