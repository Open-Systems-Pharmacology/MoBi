using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;

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
      {
         view.EnableMultiSelect = true;
      }

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();
         _view.ChangeLocalisationAllowed = false;
      }

      public override void SelectionChanged()
      {
         base.SelectionChanged();
         OnStatusChanged(null, null);
      }

      public override bool CanClose => isSelectionParameterType();

      private bool isSelectionParameterType()
      {
         var allSelected = GetAllSelected<IObjectBase>().ToList();
         var allSelectedAreParameters = allSelected.All(x => x is IParameter);
         return allSelectedAreParameters && allSelected.Any();
      }
   }
}