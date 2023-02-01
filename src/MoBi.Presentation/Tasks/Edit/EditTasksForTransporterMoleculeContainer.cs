using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForTransporterMoleculeContainer : IEditTaskFor<TransporterMoleculeContainer>
   {
      void ChangeTransportName(TransporterMoleculeContainer transporterMoleculeContainer, IBuildingBlock buildingBlock);
   }

   public class EditTasksForTransporterMoleculeContainer : EditTaskFor<TransporterMoleculeContainer>, IEditTasksForTransporterMoleculeContainer
   {
      private readonly ICoreCalculationMethodRepository _calculationMethodRepository;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private readonly IDialogCreator _dialogCreator;

      public EditTasksForTransporterMoleculeContainer(IInteractionTaskContext interactionTaskContext, ICoreCalculationMethodRepository calculationMethodRepository,
         IReactionDimensionRetriever dimensionRetriever, IDialogCreator dialogCreator) : base(interactionTaskContext)
      {
         _calculationMethodRepository = calculationMethodRepository;
         _dimensionRetriever = dimensionRetriever;
         _dialogCreator = dialogCreator;
      }

      public override bool EditEntityModal(TransporterMoleculeContainer entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         var name = _interactionTaskContext.DialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(ObjectName), AppConstants.Captions.NewWindow(ObjectName),
            string.Empty, GetForbiddenNamesWithoutSelf(entity, existingObjectsInParent));
         if (name.IsNullOrEmpty())
            return false;

         entity.Name = name;
         entity.TransportName = name;
         var moleculesBuildingBlock = buildingBlock.DowncastTo<IMoleculeBuildingBlock>();
         var molecule = moleculesBuildingBlock.FindByName(entity.Name);
         if (molecule != null)
            return true;

         var moleculeBuilder = _context.Create<IMoleculeBuilder>().WithName(entity.Name).WithIcon(ApplicationIcons.Transporter.IconName);
         moleculeBuilder.QuantityType = QuantityType.Transporter;
         moleculeBuilder.DefaultStartFormula = _context.Create<ConstantFormula>().WithValue(0).WithDimension(_dimensionRetriever.MoleculeDimension);

         _calculationMethodRepository.GetAllCategoriesDefault().Each(cm => moleculeBuilder.AddUsedCalculationMethod(new UsedCalculationMethod(cm.Category, AppConstants.DefaultNames.EmptyCalculationMethod)));
         commandCollector.AddCommand(new AddMoleculeBuilderCommand(moleculesBuildingBlock, moleculeBuilder).Run(_context));
         return true;
      }

      public void ChangeTransportName(TransporterMoleculeContainer transporterMoleculeContainer, IBuildingBlock buildingBlock)
      {
         var unallowedNames = new List<string>(AppConstants.UnallowedNames);
         unallowedNames.AddRange(GetForbiddenNamesWithoutSelf(transporterMoleculeContainer, transporterMoleculeContainer.ParentContainer));
         var oldTransportName = transporterMoleculeContainer.TransportName;
         var newName = _dialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(oldTransportName), AppConstants.Captions.NewName, oldTransportName, unallowedNames);

         if (string.IsNullOrEmpty(newName))
            return;

         var commandCollector = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.RenameCommand,
            ObjectType = ObjectName,
            Description = AppConstants.Commands.RenameDescription(transporterMoleculeContainer, newName)
         };

         if (CheckUsagesFor(newName, transporterMoleculeContainer.TransportName, transporterMoleculeContainer, commandCollector))
         {
            commandCollector.AddCommand(new EditObjectBasePropertyInBuildingBlockCommand(transporterMoleculeContainer.PropertyName(x => x.TransportName), newName, oldTransportName, transporterMoleculeContainer, buildingBlock) { ObjectType = ObjectName });
         }
         commandCollector.Run(_context);
         _context.AddToHistory(commandCollector);
      }
   }
}