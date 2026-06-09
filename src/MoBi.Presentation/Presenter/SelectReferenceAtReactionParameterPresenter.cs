using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   internal interface ISelectReferenceAtReactionParameterPresenter : ISelectReferenceAtParameterPresenter
   {
   }

   internal class SelectReferenceAtReactionParameterPresenter : SelectReferenceAtParameterPresenter, ISelectReferenceAtReactionParameterPresenter
   {
      public SelectReferenceAtReactionParameterPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context, IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreatorAtReactionParameter objectPathCreator, 
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings, objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, buildingBlockRepository)
      {
      }
   }
}