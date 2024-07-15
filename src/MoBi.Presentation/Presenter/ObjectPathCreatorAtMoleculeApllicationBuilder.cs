﻿using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{

   public interface IObjectPathCreatorAtMoleculeApllicationBuilder:IObjectPathCreator { }
   class ObjectPathCreatorAtMoleculeApllicationBuilder : ObjectPathCreatorBase, IObjectPathCreatorAtMoleculeApllicationBuilder
   {
      public ObjectPathCreatorAtMoleculeApllicationBuilder(IObjectPathFactory objectPathFactory, IAliasCreator aliasCreator, IMoBiContext context) : base(objectPathFactory, aliasCreator, context)
      {
      }

      protected override T AdjustReferences<T>(IEntity entity, T path)
      {
         path.AddAtFront(ObjectPath.PARENT_CONTAINER);
         return path;
      }
   }
}