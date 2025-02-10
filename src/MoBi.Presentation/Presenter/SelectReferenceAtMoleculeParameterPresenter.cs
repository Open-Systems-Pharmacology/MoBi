using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using MoBi.Core.Domain.Repository;

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
         IObjectPathCreatorAtMoleculeParameter objectBaseCreator, 
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings, objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectBaseCreator, buildingBlockRepository)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
         base.AddSpecificInitialObjects();
         AddReactions();
      }
   }
}