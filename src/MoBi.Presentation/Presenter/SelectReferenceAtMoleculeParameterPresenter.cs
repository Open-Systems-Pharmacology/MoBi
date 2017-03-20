using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   internal interface ISelectReferenceAtMoleculeParameterPresenter : ISelectReferenceAtParameterPresenter
   {
   }

   internal class SelectReferenceAtMoleculeParameterPresenter : SelectReferenceAtParameterPresenter, ISelectReferenceAtMoleculeParameterPresenter
   {
      public SelectReferenceAtMoleculeParameterPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreatorAtMoleculeParameter objectBaseCreator)
         : base(view, objectBaseDTOMapper, context, userSettings, objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectBaseCreator)
      {
      }

      protected override void AddSpecificInitalObjects()
      {
         base.AddSpecificInitalObjects();
         AddReactions();
      }
   }
}