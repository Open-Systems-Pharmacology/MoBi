using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using MoBi.Core.Domain.Repository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtMoleculePresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtMoleculePresenter : SelectReferencePresenterBase, ISelectReferenceAtMoleculePresenter
   {
      public SelectReferenceAtMoleculePresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, 
         IObjectBaseDTOToReferenceNodeMapper referenceMapper, 
         IObjectPathCreatorAtInitialCondition objectPathCreator, 
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings, objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.PhysicalContainerOnly, buildingBlockRepository)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();
         AddReactions();
      }
   }
}