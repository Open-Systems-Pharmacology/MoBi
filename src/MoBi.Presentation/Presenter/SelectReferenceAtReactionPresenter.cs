using System.Collections.Generic;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtReactionPresenter : ISelectReferencePresenter
   {
      void Init(IEntity refObjectBase, IEnumerable<IObjectBase> entities, IReactionBuilder reactionBuilder);
   }
   internal class SelectReferenceAtReactionPresenter : SelectReferencePresenterBase, ISelectReferenceAtReactionPresenter
   {
      private readonly IObjectPathCreatorAtReaction _objectPathCreatorAtReaction;

      public SelectReferenceAtReactionPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, IObjectPathCreatorAtReaction objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.PhysicalContainerOnly)
      {
         _objectPathCreatorAtReaction = objectPathCreator;
      }

      public void Init(IEntity refObjectBase, IEnumerable<IObjectBase> entities, IReactionBuilder reactionBuilder)
      {
         _objectPathCreatorAtReaction.Reaction = reactionBuilder;
         base.Init(refObjectBase, entities, reactionBuilder);
      }

      protected override void AddSpecificInitalObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
         AddReactions();
      }
   }
}