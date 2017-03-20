using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtEventAssignmentPresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtEventAssignmentPresenter : SelectReferencePresenterBase, ISelectReferenceAtEventAssignmentPresenter
   {
      public SelectReferenceAtEventAssignmentPresenter(ISelectReferenceView view, IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context, IUserSettings userSettings, IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, IObjectPathCreatorAtEventAssingment objectPathCreator)
         : base(
            view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreator, Localisations.ContainerOnly)
      {
      }

      protected override void AddSpecificInitalObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
      }

      public override void Init(IEntity assignment, IEnumerable<IObjectBase> contextSpecificEntitiesToAddToReferenceTree, IUsingFormula editedObject)
      {
         base.Init(assignment, contextSpecificEntitiesToAddToReferenceTree, editedObject);
         addEventGroupParameter(assignment.RootContainer);
      }

      private void addEventGroupParameter(IContainer rootContainer)
      {
         _view.AddNode(_referenceMapper.MapFrom(rootContainer));
      }
   }
}