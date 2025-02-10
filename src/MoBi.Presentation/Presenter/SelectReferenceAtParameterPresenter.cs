using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtParameterPresenter : ISelectReferencePresenter
   {
      bool ChangeLocalisationAllowed { get; set; }
      void DisableTimeSelection();
   }

   public class SelectReferenceAtParameterPresenter : SelectReferencePresenterBase, ISelectReferenceAtParameterPresenter
   {
      private bool _addTime = true;

      public SelectReferenceAtParameterPresenter(ISelectReferenceView view,
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
      {
         ChangeLocalisationAllowed = true;
      }

      protected override void AddSpecificInitialObjects()
      {
         if(_addTime)
            AddTimeReference();
         AddSpatialStructures();
      }

      public bool ChangeLocalisationAllowed
      {
         get => _view.ChangeLocalisationAllowed;
         set => _view.ChangeLocalisationAllowed = value;
      }

      public void DisableTimeSelection()
      {
         _addTime = false;
      }
   }
}