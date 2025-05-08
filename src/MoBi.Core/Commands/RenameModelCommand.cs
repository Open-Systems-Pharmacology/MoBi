using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class RenameModelCommand : MoBiReversibleCommand
   {
      private IModel _model;

      private readonly string _modelId;
      private readonly string _newName;
      private readonly string _oldName;

      public RenameModelCommand(IModel model, string newName)
      {
         _model = model;
         _newName = newName;
         _oldName = model.Name;
         _modelId = model.Id;
         ObjectType = ObjectTypes.Model;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.SetDescription(ObjectType, AppConstants.Captions.Name, newName, _oldName);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RenameModelCommand(_model, _oldName).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _model = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _model.Name = _newName;
         var root = _model.Root;
         root.Name = _newName;

         updateFormulasInContainer(root);
         updateFormulasInContainer(_model.Neighborhoods);

         var allEventAssignments = root.GetAllChildren<EventAssignment>();
         allEventAssignments.Each(ea => updateObjectPath(ea.ObjectPath));
      }

      private void updateFormulasInContainer(IContainer root)
      {
         var allFormulas = root.GetAllChildren<IUsingFormula>(uf => !uf.Formula.IsConstant()).Select(uf => uf.Formula);
         allFormulas.Each(updateObjectPaths);
         allFormulas = root.GetAllChildren<IParameter>(p => p.RHSFormula != null).Select(p => p.RHSFormula);
         allFormulas.Each(updateObjectPaths);
      }

      private void updateObjectPaths(IFormula formula)
      {
         formula.ObjectPaths.Each(updateObjectPath);
      }

      private void updateObjectPath(ObjectPath path)
      {
         if (!path.First().Equals(_oldName)) 
            return;

         path.Remove(_oldName);
         path.AddAtFront(_newName);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _model = context.Get<IModel>(_modelId);
      }
   }
}