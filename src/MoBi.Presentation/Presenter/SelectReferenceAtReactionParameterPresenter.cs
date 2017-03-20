using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
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
         IObjectPathCreatorAtReactionParameter objectPathCreator)
         : base(view, objectBaseDTOMapper, context, userSettings, objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator)
      {
      }
   }
}