using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Core.Domain.Repository;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtEventPresenter : ISelectReferencePresenter
   {
      void Init(IEntity refObject, IEnumerable<IObjectBase> entities, EventBuilder assingment);
   }

   internal class SelectReferenceAtEventPresenter : SelectReferencePresenterBase, ISelectReferenceAtEventPresenter
   {
      public SelectReferenceAtEventPresenter(ISelectReferenceView view,
         IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,
         IUserSettings userSettings,
         IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyDTOMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper,
         IObjectBaseDTOToReferenceNodeMapper referenceMapper,
         IObjectPathCreatorAtEvent objectPathCreator, 
         IBuildingBlockRepository buildingBlockRepository)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyDTOMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly, buildingBlockRepository)
      {
      }

      protected override void AddSpecificInitialObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
      }

      private void addEventGroupParameter(IContainer rootContainer)
      {
         _view.AddNode(_referenceMapper.MapFrom(rootContainer));
      }

      public void Init(IEntity refObject, IEnumerable<IObjectBase> entities, EventBuilder eventBuilder)
      {
         addEventGroupParameter(eventBuilder.RootContainer);
      }
   }
}