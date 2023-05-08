using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   internal interface ISelectReferenceAtMoleculePresenter : ISelectReferencePresenter
   {
   }

   internal class SelectReferenceAtMoleculePresenter : SelectReferencePresenterBase, ISelectReferenceAtMoleculePresenter
   {
      public SelectReferenceAtMoleculePresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, IObjectPathCreatorAtInitialCondition objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.PhysicalContainerOnly)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();
         AddReactions();
      }
   }
}