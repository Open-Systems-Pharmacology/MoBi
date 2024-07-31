using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using NHibernate.Util;
using NPOI.SS.Formula.Functions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using Remotion.Linq.Parsing.ExpressionVisitors.Transformation.PredefinedTransformations;

namespace MoBi.Presentation.Presenter
{
    public interface ISelectReferenceAtParameterValuePresenter : ISelectReferencePresenter
   {
   }

   public class SelectReferenceAtParameterValuePresenter : SelectReferencePresenterBase, ISelectReferenceAtParameterValuePresenter
    {
       protected ITreeNode _selectedNode;
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
      {}

      protected override void AddSpecificInitialObjects()
      {
         AddSpatialStructures();

         _view.ChangeLocalisationAllowed = false;
      }

      public override void SelectionChanged(ITreeNode treeNode)
      {
         OnStatusChanged(null, null);
      }

      public override bool CanClose => isSelectionParameterType();

      private bool isSelectionParameterType() => getSelected<IParameter>() != null;
      
      public override ObjectPath GetSelection()
      {
         var selection = getSelected<IParameter>();

         var path  = shouldCreateAbsolutePaths ?
            _objectPathFactory.CreateAbsoluteObjectPath(selection) :
            _objectPathFactory.CreateRelativeObjectPath(_refObject, selection);

         return shouldCreateAbsolutePaths ?
            _objectPathFactory.CreateAbsoluteObjectPath(selection) :
            _objectPathFactory.CreateRelativeObjectPath(_refObject, selection);
      }

      protected override T getSelected<T>() 
      {
         var dto = _view.SelectedDTO;
         return dto == null ? null : _context.Get<T>(dto.ObjectBase.Id);
      }
   }
}