using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtEventPresenter : ISelectReferencePresenter
   {
      void Init(IEntity refObject, IEnumerable<IObjectBase> entities, IEventBuilder assingment);
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
         IObjectPathCreatorAtEvent objectPathCreator)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyDTOMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly)
      {
      }

      protected override void AddSpecificInitalObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
      }

      private void addEventGroupParameter(IContainer rootContainer)
      {
         _view.AddNode(_referenceMapper.MapFrom(rootContainer));
      }

      public void Init(IEntity refObject, IEnumerable<IObjectBase> entities, IEventBuilder eventBuilder)
      {
         addEventGroupParameter(eventBuilder.RootContainer);
      }
   }
}