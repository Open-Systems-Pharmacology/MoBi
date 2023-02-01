using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtObserverPresenter : ISelectReferencePresenter
   {
   }

   public abstract class SelectReferenceAtObserverPresenter : SelectReferencePresenterBase, ISelectReferenceAtObserverPresenter
   {
      protected SelectReferenceAtObserverPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, 
         IObjectPathCreatorAtObserver objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly)
      {
      }
   }

   public interface ISelectReferenceAtContainerObserverPresenter : ISelectReferenceAtObserverPresenter
   {
   }

   internal class SelectReferenceAtContainerObserverPresenter : SelectReferenceAtObserverPresenter,ISelectReferenceAtContainerObserverPresenter
   {
      public SelectReferenceAtContainerObserverPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper, IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, 
          IObjectPathCreatorAtObserver objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();
      }
   }

   public interface ISelectReferenceAtAmountObserverPresenter : ISelectReferenceAtObserverPresenter
   {
   }

   internal class SelectReferenceAtAmountObserverPresenter : SelectReferenceAtObserverPresenter,ISelectReferenceAtAmountObserverPresenter
   {
      public SelectReferenceAtAmountObserverPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper, IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, IObjectPathCreatorAtAmountObserver objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper,referenceMapper, objectPathCreator)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
         AddMolecule();
         AddSpatialStructures();
      }
   }
}