using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using MoBi.Core.Domain.Repository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferencePresenterAtApplicationBuilder : ISelectReferencePresenter
   {
   }

   internal class SelectReferencePresenterAtApplicationBuilder : SelectReferencePresenterBase, ISelectReferencePresenterAtApplicationBuilder
   {
      public SelectReferencePresenterAtApplicationBuilder(ISelectReferenceView view, IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper, IMoBiContext context, IUserSettings userSettings, IObjectBaseToDummyMoleculeDTOMapper dtoObjectBaseToMoleculeDTOMapper, IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper, IObjectPathCreatorAtMoleculeApplicationBuilder objectPathCreator, IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings, dtoObjectBaseToMoleculeDTOMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly, buildingBlockRepository)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
      }
   }
}