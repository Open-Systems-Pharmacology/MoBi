using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Core.Services
{
   /// <summary>
   ///    Checks the name defined in the current project and ensure, that a molecule cannot have the same name as a parameter
   ///    etc..
   ///    It goes way beyond just checking the value in parent containers
   ///    Here are the rules:
   ///    - A molecule cannot have the same name as any parameters in the spatial structure or in molecules
   ///    - A parameter cannot have the same name as any molecules, observers or reaction
   ///    - An observer cannot have the same name as any local molecules parameters
   ///    - A transport cannot have the same name as any local molecules parameters
   ///    - A container cannot have the same name as a molecule or a Reaction
   /// </summary>
   public interface IForbiddenNamesRetriever
   {
      IEnumerable<string> For(MoleculeBuilder moleculeBuilder);
      IEnumerable<string> For(ReactionBuilder moleculeBuilder);
      IEnumerable<string> For(IParameter parameter);
      IEnumerable<string> For(ObserverBuilder parameter);
      IEnumerable<string> For(TransportBuilder transportBuilder);
      IEnumerable<string> For(IContainer container);
      IEnumerable<string> For(TransporterMoleculeContainer transporterMoleculeContainer);
      IEnumerable<string> For(IModelCoreSimulation simulation);
      IEnumerable<string> For<T>(T objectBase) where T : IObjectBase;
   }

   public class ForbiddenNamesRetriever : IForbiddenNamesRetriever,
      IVisitor<MoleculeBuilder>,
      IVisitor<IParameter>,
      IVisitor<ObserverBuilder>,
      IVisitor<TransportBuilder>,
      IVisitor<IContainer>,
      IVisitor<TransporterMoleculeContainer>,
      IVisitor<ReactionBuilder>,
      IVisitor<IDistributedParameter>,
      IVisitor<IModelCoreSimulation>

   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly ISimulationRepository _simulationRepository;
      private HashSet<string> _nameHashForVisitor;

      public ForbiddenNamesRetriever(IBuildingBlockRepository buildingBlockRepository, ISimulationRepository simulationRepository)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _simulationRepository = simulationRepository;
      }

      public IEnumerable<string> For(MoleculeBuilder moleculeBuilder)
      {
         var nameHash = new HashSet<string>();
         addNamesToHash(nameHash, allParameterNamesFromSpatialStructureInProject());
         addNamesToHash(nameHash, allParametersFromMoleculesInProject());
         addNamesToHash(nameHash, allMoleculeNamesFromInitialConditions(moleculeBuilder));
         addNamesToHash(nameHash, allReactionNamesFromProject());
         return nameHash;
      }

      public IEnumerable<string> For(ReactionBuilder moleculeBuilder)
      {
         var nameHash = new HashSet<string>();
         addNamesToHash(nameHash, allParameterNamesFromSpatialStructureInProject());
         addNamesToHash(nameHash, allMoleculeNamesFromProject());
         return nameHash;
      }

      public IEnumerable<string> For(IParameter parameter)
      {
         var nameHash = new HashSet<string>();
         addNamesToHash(nameHash, allMoleculeNamesFromProject());
         addNamesToHash(nameHash, allObserverNamesFromProject());
         addNamesToHash(nameHash, allReactionNamesFromProject());
         return nameHash;
      }

      public IEnumerable<string> For(ObserverBuilder observerBuilder) => allLocalParametersFromMoleculesInProject();

      public IEnumerable<string> For(TransportBuilder transportBuilder)
      {
         var nameHash = new HashSet<string>();
         addNamesToHash(nameHash, allMoleculeNamesFromProject());
         addNamesToHash(nameHash, allReactionNamesFromProject());
         return nameHash;
      }

      public IEnumerable<string> For(IContainer container) => allMoleculeNamesFromProject();

      public IEnumerable<string> For(TransporterMoleculeContainer transporterMoleculeContainer) => allLocalParametersFromMoleculesInProject();

      public IEnumerable<string> For(IModelCoreSimulation simulation)
      {
         var nameHash = new HashSet<string>();
         addNamesToHash(nameHash, _buildingBlockRepository.MoleculeBlockCollection.SelectMany(mbb => mbb.Select(x => x.Name)));
         addNamesToHash(nameHash, _buildingBlockRepository.SpatialStructureCollection.SelectMany(mbb => mbb.Select(x => x.Name)));
         addNamesToHash(nameHash, _buildingBlockRepository.ReactionBlockCollection.SelectMany(rbb => rbb.Select(x => x.Name)));
         addNamesToHash(nameHash, _simulationRepository.All().Select(bb => bb.Name));
         return nameHash;
      }

      public IEnumerable<string> For<T>(T parameter) where T : IObjectBase
      {
         _nameHashForVisitor = new HashSet<string>();
         this.Visit(parameter);
         return _nameHashForVisitor;
      }

      private IEnumerable<string> allObserverNamesFromProject() => allNamesFrom<ObserverBuildingBlock, ObserverBuilder>(_buildingBlockRepository.ObserverBlockCollection);

      private IEnumerable<string> allMoleculeNamesFromProject() => allNamesFrom<MoleculeBuildingBlock, MoleculeBuilder>(_buildingBlockRepository.MoleculeBlockCollection);

      private IEnumerable<string> allReactionNamesFromProject() => allNamesFrom<MoBiReactionBuildingBlock, ReactionBuilder>(_buildingBlockRepository.ReactionBlockCollection);

      private IEnumerable<string> allNamesFrom<TBuildingBlock, TBuilder>(IEnumerable<TBuildingBlock> buildingBlock) where TBuildingBlock : IBuildingBlock<TBuilder> where TBuilder : class, IBuilder => 
         buildingBlock.SelectMany(x => x.All()).Select(x => x.Name);

      private IEnumerable<string> allLocalParametersFromMoleculesInProject() => allParametersFromMoleculesInProject(ParameterBuildMode.Local);

      private IEnumerable<string> allParametersFromMoleculesInProject()
      {
         var nameHash = new HashSet<string>();
         addNamesToHash(nameHash, allParametersFromMoleculesInProject(ParameterBuildMode.Global));
         addNamesToHash(nameHash, allParametersFromMoleculesInProject(ParameterBuildMode.Local));
         return nameHash;
      }

      private IEnumerable<string> allParametersFromMoleculesInProject(ParameterBuildMode buildMode)
      {
         var nameHash = new HashSet<string>();
         _buildingBlockRepository.MoleculeBlockCollection
            .SelectMany(x => x.All()).Each(m => addNamesToHash(nameHash, allParameterNamesFrom(m, x => x.BuildMode == buildMode)));

         return nameHash;
      }

      /// <summary>
      ///    Retrieves all the molecule names from start values where a molecule is named like the molecule builder.
      ///    The MoleculeBuilders Name is not returned
      /// </summary>
      /// <param name="moleculeBuilder">The molecule builder for which forbidden names are retrieved.</param>
      /// <returns>molecule names that are forbidden for the moleculebuilder</returns>
      /// <remarks>
      ///    This is necessary when a molecule was removed and another molecule should be renamed. It
      ///    should not be possible to rename to the removed named to prevent double definitions of InitialConditions
      /// </remarks>
      private IEnumerable<string> allMoleculeNamesFromInitialConditions(MoleculeBuilder moleculeBuilder)
      {
         var nameHash = new HashSet<string>();
         // We need to retrieve Names from here if a removed Molecule is still in MSV
         // to prevent double definitions in InitialConditions
         getAllInitialConditionsFromBuildingBlocksFor(moleculeBuilder)
            .Select(x => x.MoleculeName).Distinct()
            .Where(x => !x.Equals(moleculeBuilder.Name))
            .Each(x => nameHash.Add(x));

         return nameHash;
      }

      private IEnumerable<InitialCondition> getAllInitialConditionsFromBuildingBlocksFor(MoleculeBuilder builder)
      {
         var builderName = builder.Name;
         return _buildingBlockRepository.InitialConditionBlockCollection
            .Where(x => x.Any(msv => msv.MoleculeName.Equals(builderName)))
            .SelectMany(x => x.All());
      }

      private IEnumerable<string> allParameterNamesFromSpatialStructureInProject()
      {
         var nameHash = new HashSet<string>();

         _buildingBlockRepository.SpatialStructureCollection
            .SelectMany(x => x.TopContainers).Each(c => addNamesToHash(nameHash, allParameterNamesFrom(c)));

         return nameHash;
      }

      private void addNamesToHash(HashSet<string> nameHash, IEnumerable<string> namesToAdd) => namesToAdd.Each(x => nameHash.Add(x));

      private IEnumerable<string> allParameterNamesFrom(IContainer container) => allParameterNamesFrom(container, x => true);

      private IEnumerable<string> allParameterNamesFrom(IContainer container, Func<IParameter, bool> selector) => container.GetAllChildren(selector).Select(x => x.Name);

      public void Visit(MoleculeBuilder moleculeBuilder) => addNamesToHash(_nameHashForVisitor, For(moleculeBuilder));

      public void Visit(IParameter parameter) => addNamesToHash(_nameHashForVisitor, For(parameter));

      public void Visit(IDistributedParameter parameter) => addNamesToHash(_nameHashForVisitor, For(parameter.DowncastTo<IParameter>()));

      public void Visit(ObserverBuilder observerBuilder) => addNamesToHash(_nameHashForVisitor, For(observerBuilder));

      public void Visit(TransportBuilder transportBuilder) => addNamesToHash(_nameHashForVisitor, For(transportBuilder));

      public void Visit(IContainer container) => addNamesToHash(_nameHashForVisitor, For(container));

      public void Visit(TransporterMoleculeContainer transporterMoleculeContainer) => addNamesToHash(_nameHashForVisitor, For(transporterMoleculeContainer));

      public void Visit(ReactionBuilder objToVisit) => addNamesToHash(_nameHashForVisitor, For(objToVisit));

      public void Visit(IModelCoreSimulation objToVisit) => addNamesToHash(_nameHashForVisitor, For(objToVisit));
   }
}