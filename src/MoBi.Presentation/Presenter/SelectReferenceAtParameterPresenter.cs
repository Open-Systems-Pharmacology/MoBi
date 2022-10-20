using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtParameterPresenter : ISelectReferencePresenter
   {
      bool ChangeLocalisationAllowed { get; set; }
   }

   public class SelectReferenceAtParameterPresenter : SelectReferencePresenterBase, ISelectReferenceAtParameterPresenter
   {
      public SelectReferenceAtParameterPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, IObjectPathCreatorAtParameter objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly)
      {
         ChangeLocalisationAllowed = true;
      }

      protected override void AddSpecificInitialObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
      }

      public bool ChangeLocalisationAllowed
      {
         get { return _view.ChangeLocalisationAllowed; }
         set { _view.ChangeLocalisationAllowed = value; }
      }
   }
}