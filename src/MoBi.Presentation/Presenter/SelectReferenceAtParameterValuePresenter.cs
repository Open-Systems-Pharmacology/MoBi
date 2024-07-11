using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
    public interface ISelectReferenceAtParameterValuePresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtParameterValuePresenter : SelectReferencePresenterBase, ISelectReferenceAtParameterValuePresenter
    {
      public SelectReferenceAtParameterValuePresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, 
         IObjectBaseDTOToReferenceNodeMapper referenceMapper, 
         IObjectPathCreatorAtParameter objectPathCreator, 
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly, buildingBlockRepository)
      {}

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();

         _view.ChangeLocalisationAllowed = false;
      }
   }
}