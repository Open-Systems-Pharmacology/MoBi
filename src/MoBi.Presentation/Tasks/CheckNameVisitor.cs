using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Visitor;

namespace MoBi.Presentation.Tasks
{
   public interface ICheckNameVisitor
   {
      /// <summary>
      ///    Returns the changes required to be performed in the <paramref name="buildingBlock" /> when renaming the
      ///    <paramref name="objectToRename" />
      /// </summary>
      /// <param name="objectToRename">Object to rename</param>
      /// <param name="newName">New name for the object</param>
      /// <param name="buildingBlock">Building block where the object to rename is defined</param>
      /// <param name="oldName"></param>
      IReadOnlyList<IStringChange> GetPossibleChangesFrom(IObjectBase objectToRename, string newName, IBuildingBlock buildingBlock, string oldName);
   }

   public class CheckNameVisitor : ICheckNameVisitor,
      IVisitor<IObjectBase>,
      IVisitor<IFormula>,
      IVisitor<ParameterValue>,
      IVisitor<InitialCondition>,
      IVisitor<ObserverBuilder>,
      IVisitor<ReactionBuilder>,
      IVisitor<ApplicationBuilder>,
      IVisitor<MoleculeBuilder>,
      IVisitor<EventAssignmentBuilder>,
      IVisitor<IBuildingBlock>,
      IVisitor<TransporterMoleculeContainer>,
      IVisitor<TransportBuilder>,
      IVisitor<ApplicationMoleculeBuilder>,
      IVisitor<SimulationSettings>,
      IVisitor<EventGroupBuilder>,
      IVisitor<NeighborhoodBuilder>
   {
      private string _newName;
      private IObjectBase _objectToRename;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IAliasCreator _aliasCreator;
      private readonly string _namePropertyName;
      private readonly string _eventObjectPathPropertyName;
      private readonly string _appBuilderMoleculeNamePropertyName;
      private IBuildingBlock _buildingBlock;
      private readonly string _transportNamePropertyName;
      private string _oldName;
      private readonly IParameterValuePathTask _parameterValuePathTask;
      private readonly IInitialConditionPathTask _msvPathTask;
      private readonly ICloneManager _cloneManager;
      private StringChanges _changes;

      public CheckNameVisitor(IObjectTypeResolver objectTypeResolver, IAliasCreator aliasCreator, IParameterValuePathTask parameterValuePathTask, IInitialConditionPathTask msvPathTask, ICloneManager cloneManager)
      {
         _objectTypeResolver = objectTypeResolver;
         _aliasCreator = aliasCreator;
         _parameterValuePathTask = parameterValuePathTask;
         _msvPathTask = msvPathTask;
         _cloneManager = cloneManager;

         Expression<Func<IObjectBase, string>> nameString = x => x.Name;
         _namePropertyName = nameString.Name();

         Expression<Func<ApplicationBuilder, string>> appBuilderMoleculeName = x => x.MoleculeName;
         _appBuilderMoleculeNamePropertyName = appBuilderMoleculeName.Name();

         Expression<Func<EventAssignmentBuilder, ObjectPath>> eventObjectPath = x => x.ObjectPath;
         _eventObjectPathPropertyName = eventObjectPath.Name();

         Expression<Func<TransporterMoleculeContainer, string>> transportName = x => x.TransportName;
         _transportNamePropertyName = transportName.Name();
      }

      public IReadOnlyList<IStringChange> GetPossibleChangesFrom(IObjectBase objectToRename, string newName, IBuildingBlock buildingBlock, string oldName)
      {
         _changes = new StringChanges(buildingBlock);
         _objectToRename = objectToRename;
         _newName = newName;
         _oldName = oldName;

         try
         {
            buildingBlock.AcceptVisitor(this);
            return _changes.ToList();
         }
         finally
         {
            _buildingBlock = null;
            _objectToRename = null;
            _changes.Clear();
         }
      }

      public void Visit(IFormula formula)
      {
         checkObjectBase(formula);
         // create aliases from name to change them accordingly
         var oldAlias = _aliasCreator.CreateAliasFrom(_oldName);
         var newAlias = _aliasCreator.CreateAliasFrom(_newName);
         foreach (var path in formula.ObjectPaths)
         {
            // possible path adjustments
            if (path.Contains(_oldName))
            {
               //var newPath = generateNewPath(path);
               _changes.Add(formula, new ChangePathElementAtFormulaUseablePathCommand(_newName, formula, _oldName, path, _buildingBlock));
            }

            // possible alias Adjustments
            if (path.Alias.Equals(oldAlias))
            {
               var i = 0;
               // change new alias and remember this for this formula to change formula string right if necessary
               while (formula.ObjectPaths.Select(x => x.Alias).Contains(newAlias))
               {
                  newAlias = $"{newAlias}{i}";
                  i++;
               }

               _changes.Add(formula, new EditFormulaAliasCommand(formula, newAlias, oldAlias, _buildingBlock));
            }
         }

         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null)
            return;

         // possible Formula string adjustments
         if (!containsWord(explicitFormula.FormulaString, oldAlias))
            return;

         var newFormulaString = wordReplace(explicitFormula.FormulaString, oldAlias, newAlias);
         var editFormulaStringCommand = new EditFormulaStringCommand(newFormulaString, explicitFormula, _buildingBlock);
         _changes.Add(formula, editFormulaStringCommand, editFormulaStringCommand.Description);
      }

      public void Visit(IObjectBase objectBase)
      {
         checkObjectBase(objectBase);
      }

      public void Visit(NeighborhoodBuilder neighborhoodBuilder)
      {
         checkObjectBase(neighborhoodBuilder);
         //If not renaming a container or we are renaming a neighborhood builder, we exit
         if (!(_objectToRename is IContainer) || !(_buildingBlock is SpatialStructure spatialStructure) || ReferenceEquals(neighborhoodBuilder, _objectToRename))
            return;

         //now the name of the container is used in the neighborhood name. Propose update
         if (containsOldName(neighborhoodBuilder.Name))
         {
            var newNeighborhoodName = stringReplace(neighborhoodBuilder.Name);
            _changes.Add(neighborhoodBuilder, renameCommand(neighborhoodBuilder, newNeighborhoodName), renameDescription(neighborhoodBuilder, neighborhoodBuilder.Name, newNeighborhoodName, _namePropertyName));
         }

         //check first and second neighbors
         if (neighborhoodBuilder.FirstNeighborPath?.Contains(_oldName) ?? false)
         {
            var modifiedPath = renamedPathFrom(neighborhoodBuilder.FirstNeighborPath);
            _changes.Add(neighborhoodBuilder, new ChangeFirstNeighborPathCommand(modifiedPath, neighborhoodBuilder, spatialStructure));
         }

         if (neighborhoodBuilder.SecondNeighborPath?.Contains(_oldName) ?? false)
         {
            var modifiedPath = renamedPathFrom(neighborhoodBuilder.SecondNeighborPath);
            _changes.Add(neighborhoodBuilder, new ChangeSecondNeighborPathCommand(modifiedPath, neighborhoodBuilder, spatialStructure));
         }
      }

      private ObjectPath renamedPathFrom(ObjectPath path)
      {
         var modifiedPath = path.Clone<ObjectPath>();
         modifiedPath.Replace(_oldName, _newName);
         return modifiedPath;
      }

      private string stringReplace(string replaceIn) => replaceIn.Replace(_oldName, _newName);

      private bool containsOldName(string stringToCheck) => stringToCheck.Contains(_oldName);

      private string wordReplace(string replaceIn, string oldValue, string newValue)
      {
         var pattern = $"\\b{oldValue}\\b";
         return Regex.Replace(replaceIn, pattern, newValue);
      }

      private bool containsWord(string stringToCheck, string value)
      {
         var pattern = $"\\b{value}\\b";
         return Regex.IsMatch(stringToCheck, pattern);
      }

      private void checkObjectBase<T>(T objectBase) where T : IObjectBase
      {
         if (objectBase.IsAnImplementationOf<IContainer>())
            checkTagsInContainer((IContainer) objectBase);

         if (_objectToRename.Equals(objectBase))
            return;

         if (string.Equals(_oldName, objectBase.Name))
            _changes.Add(objectBase, renameCommand(objectBase, _newName), renameDescription(objectBase, _oldName, _newName, _namePropertyName));
      }

      private IMoBiCommand renameCommand(IObjectBase objectBase, string newName) => new RenameObjectBaseCommand(objectBase, newName, _buildingBlock);

      private string renameDescription<T>(T objectBase, string oldName, string newName, string propertyName) where T : IObjectBase
      {
         return AppConstants.Commands.EditDescription(_objectTypeResolver.TypeFor<T>(), propertyName, oldName, newName, objectBase.Name);
      }

      private void checkTagsInContainer(IContainer container)
      {
         //Check Name Tags!
         if (!container.Tags.Any(x => x.Value.Equals(_oldName)))
            return;

         _changes.Add(container, new ChangeContainerTagCommand(_newName, _oldName, container, _buildingBlock), renameDescription(container, _oldName, _newName, "Tag"));
      }

      public void Visit(IUsingFormula usingFormula)
      {
         checkObjectBase(usingFormula);
         // Formula is not checked here, only checked in cache cause double will cause errors in undo
      }

      public void Visit(TransportBuilder transportBuilder)
      {
         checkMoleculeDependentBuilder(transportBuilder, _objectTypeResolver.TypeFor(transportBuilder));
         checkDescriptorCriteria(transportBuilder, x => x.SourceCriteria);
         checkDescriptorCriteria(transportBuilder, x => x.TargetCriteria);
      }

      private void checkMoleculeDependentBuilder(IMoleculeDependentBuilder transportBuilder, string objectType)
      {
         checkObjectBase(transportBuilder);
         if (transportBuilder.MoleculeNames().Contains(_oldName))
         {
            _changes.Add(transportBuilder, new ChangeMoleculeNameAtMoleculeDependentBuilderCommand(_newName, _oldName, transportBuilder, _buildingBlock) {ObjectType = objectType});
         }

         if (transportBuilder.MoleculeNamesToExclude().Contains(_oldName))
         {
            _changes.Add(transportBuilder, new ChangeExcludeMoleculeNameAtMoleculeDependentBuilderCommand(_newName, _oldName, transportBuilder, _buildingBlock) {ObjectType = objectType});
         }
      }

      public void Visit(ObserverBuilder observerBuilder)
      {
         checkMoleculeDependentBuilder(observerBuilder, ObjectTypes.ObserverBuilder);
         checkDescriptorCriteria(observerBuilder, x => x.ContainerCriteria);
      }

      private void checkDescriptorCriteria<T>(T taggedObject, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         foreach (var tagCondition in descriptorCriteriaRetriever(taggedObject).OfType<ITagCondition>())
         {
            if (!string.Equals(tagCondition.Tag, _oldName))
               continue;

            var commandParameters = new TagConditionCommandParameters<T> {TaggedObject = taggedObject, BuildingBlock = _buildingBlock, DescriptorCriteriaRetriever = descriptorCriteriaRetriever};
            _changes.Add(taggedObject, new EditTagCommand<T>(_newName, _oldName, commandParameters));
         }
      }

      public void Visit(EventGroupBuilder eventGroupBuilder)
      {
         checkObjectBase(eventGroupBuilder);
         checkDescriptorCriteria(eventGroupBuilder, x => x.SourceCriteria);
      }

      public void Visit(ReactionBuilder reaction)
      {
         checkObjectBase(reaction);
         checkReactionPartnerIn(reaction.Educts, reaction, educt: true);
         checkReactionPartnerIn(reaction.Products, reaction, educt: false);
         checkModifier(reaction);
         checkDescriptorCriteria(reaction, x => x.ContainerCriteria);
      }

      private void checkModifier(ReactionBuilder reaction)
      {
         if (!reaction.ModifierNames.Contains(_oldName))
            return;

         var reactionBuildingBlock = _buildingBlock.DowncastTo<MoBiReactionBuildingBlock>();
         _changes.Add(reaction, new ChangeModifierCommand(_newName, _oldName, reaction, reactionBuildingBlock));
      }

      private void checkReactionPartnerIn(IEnumerable<ReactionPartnerBuilder> reactionPartners, ReactionBuilder reaction, bool educt)
      {
         var reactionBuildingBlock = _buildingBlock.DowncastTo<MoBiReactionBuildingBlock>();
         foreach (var reactionPartner in reactionPartners.Where(reactionPartner => reactionPartner.MoleculeName.Equals(_oldName)))
         {
            _changes.Add(reactionPartner, new EditReactionPartnerMoleculeNameCommand(_newName, reaction, reactionPartner, reactionBuildingBlock));
         }
      }

      public void Visit(ApplicationBuilder applicationBuilder)
      {
         Visit(applicationBuilder.DowncastTo<EventGroupBuilder>());

         if (!string.Equals(applicationBuilder.MoleculeName, _oldName))
            return;

         _changes.Add(applicationBuilder, new EditObjectBasePropertyInBuildingBlockCommand(_appBuilderMoleculeNamePropertyName, _newName, _oldName, applicationBuilder, _buildingBlock),
            renameDescription(applicationBuilder, _oldName, _newName, _appBuilderMoleculeNamePropertyName));
      }

      public void Visit(MoleculeBuilder moleculeBuilder)
      {
         checkObjectBase(moleculeBuilder);
         // Formula is not checked here, only checked in cache cause double will cause errors in undo
      }

      public void Visit(EventAssignmentBuilder eventAssignmentBuilder)
      {
         Visit(eventAssignmentBuilder as IUsingFormula);
         if (!eventAssignmentBuilder.ObjectPath.Contains(_oldName)) return;

         var oldPath = eventAssignmentBuilder.ObjectPath;
         var newPath = renamedPathFrom(eventAssignmentBuilder.ObjectPath);
         _changes.Add(eventAssignmentBuilder, new EditObjectBasePropertyInBuildingBlockCommand(_eventObjectPathPropertyName, newPath, oldPath, eventAssignmentBuilder, _buildingBlock));
      }

      public void Visit(IBuildingBlock buildingBlock)
      {
         checkBuildingBlock(buildingBlock);
      }

      public void Visit(SimulationSettings simulationSettings)
      {
         checkBuildingBlock(simulationSettings);
         checkOutputSelectionIn(simulationSettings);
         checkChartTemplatesIn(simulationSettings);
      }

      private void checkChartTemplatesIn(SimulationSettings simulationSettings)
      {
         var chartTemplates = simulationSettings.ChartTemplates.ToList();
         if (!referencesOldName(chartTemplates))
            return;

         var newChartTemplates = new List<CurveChartTemplate>();
         foreach (var chartTemplate in simulationSettings.ChartTemplates)
         {
            var newChartTemplate = chartTemplate;
            if (referencesOldName(chartTemplate))
            {
               newChartTemplate = _cloneManager.Clone(chartTemplate);
               foreach (var curveTemplate in newChartTemplate.Curves)
               {
                  curveTemplate.xData.Path = stringReplace(curveTemplate.xData.Path);
                  curveTemplate.yData.Path = stringReplace(curveTemplate.yData.Path);
               }
            }

            newChartTemplates.Add(newChartTemplate);
         }

         _changes.Add(simulationSettings, new ReplaceBuildingBlockTemplatesCommand(simulationSettings, newChartTemplates));
      }

      private void checkOutputSelectionIn(SimulationSettings simulationSettings)
      {
         var outputSelections = simulationSettings.OutputSelections;
         if (!referencesOldName(outputSelections))
            return;

         var newOutputSelection = outputSelections.Clone();
         foreach (var outputUsingOldName in outputsReferencingOldName(newOutputSelection))
         {
            newOutputSelection.RemoveOutput(outputUsingOldName);
            newOutputSelection.AddOutput(new QuantitySelection(stringReplace(outputUsingOldName.Path), outputUsingOldName.QuantityType));
         }

         _changes.Add(outputSelections, new UpdateOutputSelectionInBuildingBlockCommand(newOutputSelection, simulationSettings));
      }

      private IReadOnlyList<QuantitySelection> outputsReferencingOldName(OutputSelections outputSelections)
      {
         return outputSelections.AllOutputs.Where(x => containsOldName(x.Path)).ToList();
      }

      private bool referencesOldName(CurveChartTemplate chartTemplate)
      {
         return chartTemplate.Curves.Any(c => containsOldName(c.xData.Path) || containsOldName(c.yData.Path));
      }

      private bool referencesOldName(IEnumerable<CurveChartTemplate> chartTemplates)
      {
         return chartTemplates.Any(referencesOldName);
      }

      private bool referencesOldName(OutputSelections outputSelections)
      {
         return outputsReferencingOldName(outputSelections).Any();
      }

      private void checkBuildingBlock(IBuildingBlock buildingBlock)
      {
         // We keep actual BB to provide Information to user
         _buildingBlock = buildingBlock;
         checkObjectBase(buildingBlock);
      }

      public void Visit(TransporterMoleculeContainer transporterMoleculeContainer)
      {
         checkObjectBase(transporterMoleculeContainer);
         if (!string.Equals(_oldName, transporterMoleculeContainer.TransportName)) 
            return;

         _changes.Add(transporterMoleculeContainer,
            new EditObjectBasePropertyInBuildingBlockCommand(_transportNamePropertyName, _newName, _oldName, transporterMoleculeContainer, _buildingBlock),
            renameDescription(transporterMoleculeContainer, _oldName, _newName, _transportNamePropertyName));
      }

      public void Visit(InitialCondition initialCondition)
      {
         checkPathAndValueEntity(initialCondition, _msvPathTask);
      }

      public void Visit(ParameterValue parameterValue)
      {
         checkPathAndValueEntity(parameterValue, _parameterValuePathTask);
      }

      private void checkPathAndValueEntity<TPathAndValueEntity, TBuildingBlock>(TPathAndValueEntity pathAndValueEntity, IStartValuePathTask<TBuildingBlock, TPathAndValueEntity> startValueTask)
         where TPathAndValueEntity : PathAndValueEntity
         where TBuildingBlock : ILookupBuildingBlock<TPathAndValueEntity>
      {
         if (Equals(_objectToRename, pathAndValueEntity)) return;

         var entities = _buildingBlock as ILookupBuildingBlock<TPathAndValueEntity>;
         if (string.Equals(pathAndValueEntity.Name, _oldName))
            _changes.Add(pathAndValueEntity, startValueTask.UpdateNameCommand(entities, pathAndValueEntity, _newName));

         for (int i = 0; i < pathAndValueEntity.ContainerPath.Count; i++)
         {
            if (string.Equals(pathAndValueEntity.ContainerPath[i], _oldName))
               _changes.Add(pathAndValueEntity, startValueTask.UpdateContainerPathCommand(entities, pathAndValueEntity, i, _newName));
         }
      }

      public void Visit(ApplicationMoleculeBuilder applicationMoleculeBuilder)
      {
         checkObjectBase(applicationMoleculeBuilder);
         // possible path adjustments
         if (applicationMoleculeBuilder.RelativeContainerPath.Contains(_oldName))
         {
            _changes.Add(applicationMoleculeBuilder, new ChangePathElementAtContainerPathCommand(_newName, applicationMoleculeBuilder, _oldName, _buildingBlock));
         }
      }
   }
}