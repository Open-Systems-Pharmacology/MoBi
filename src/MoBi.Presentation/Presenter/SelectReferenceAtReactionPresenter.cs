using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Core.Domain.Repository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtReactionPresenter : ISelectReferencePresenter
   {
      void Init(IEntity refObjectBase, IReadOnlyList<IObjectBase> entities, ReactionBuilder reactionBuilder);
   }
   internal class SelectReferenceAtReactionPresenter : SelectReferencePresenterBase, ISelectReferenceAtReactionPresenter
   {
      private readonly IObjectPathCreatorAtReaction _objectPathCreatorAtReaction;

      public SelectReferenceAtReactionPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, 
         IObjectBaseDTOToReferenceNodeMapper referenceMapper, 
         IObjectPathCreatorAtReaction objectPathCreator, 
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.PhysicalContainerOnly, buildingBlockRepository)
      {
         _objectPathCreatorAtReaction = objectPathCreator;
      }

      public void Init(IEntity refObjectBase, IReadOnlyList<IObjectBase> entities, ReactionBuilder reactionBuilder)
      {
         _objectPathCreatorAtReaction.Reaction = reactionBuilder;
         base.Init(refObjectBase, entities, reactionBuilder);
      }

      protected override void AddSpecificInitialObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
         AddReactions();
      }
   }
}