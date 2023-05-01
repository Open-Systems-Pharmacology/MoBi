using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
   public interface ISBMLImporter
   {
      MoBiSpatialStructure GetMainSpatialStructure(Model model);
      MoBiReactionBuildingBlock GetMainReactionBuildingBlock();
      IContainer GetMainTopContainer();
      IContainer GetEventsTopContainer();
      MoleculeBuildingBlock GetMainMoleculeBuildingBlock();
      ParameterStartValuesBuildingBlock GetMainParameterStartValuesBuildingBlock();
      MoleculeStartValuesBuildingBlock GetMainMSVBuildingBlock();
      IEntity GetContainerFromCompartment(string compartment);
      IContainer GetContainerFromCompartment_(string compartment);
      void DoImport(Model sbmlModel, Module sbmlModule, SBMLInformation sbmlInformation, ICommandCollector command);
      void AddToProject();
      IObjectPathFactory ObjectPathFactory { get; set; }
      IObjectBaseFactory ObjectBaseFactory { get; set; }
   }

   public abstract class SBMLImporter : ISBMLImporter
   {
      protected ASTHandler _astHandler;
      protected Module _sbmlModule;
      protected SBMLInformation _sbmlInformation;
      protected ICommandCollector _command;
      protected IMoBiContext _context;

      protected SBMLImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context)
      {
         ObjectPathFactory = objectPathFactory;
         ObjectBaseFactory = objectBaseFactory;
         _astHandler = astHandler;
         _context = context;
      }

      /// <summary>
      ///     Gets the MoBi Spatial Structure generated for the SBML Import.
      /// </summary>
      public MoBiSpatialStructure GetMainSpatialStructure(Model model)
      {
         return _sbmlModule.SpatialStructure as MoBiSpatialStructure;
      }

      /// <summary>
      ///     Gets the MoBi Reaction Building Block generated for the SBML Import.
      /// </summary>
      public MoBiReactionBuildingBlock GetMainReactionBuildingBlock()
      {
         return _sbmlModule.Reactions as MoBiReactionBuildingBlock;
      }

      /// <summary>
      ///     Gets the MoBi Top Container generated for the SBML Import.
      /// </summary>
      public IContainer GetMainTopContainer()
      {
         return
            _sbmlModule.SpatialStructure?.TopContainers.FindById(SBMLConstants.SBML_TOP_CONTAINER);
      }

      /// <summary>
      ///     Gets the MoBi Eventes Top Container generated for the SBML Import.
      /// </summary>
      public IContainer GetEventsTopContainer()
      {
         if (_sbmlModule == null) return null;
         if (_sbmlModule.SpatialStructure == null) return null;
         return
            _sbmlModule.SpatialStructure.TopContainers.FindByName(SBMLConstants.SBML_EVENTS_TOP_CONTAINER);
      }

      /// <summary>
      ///     Gets the MoBi Molecule Building Block generated for the SBML Import.
      /// </summary>
      public MoleculeBuildingBlock GetMainMoleculeBuildingBlock()
      {
         return _sbmlModule.Molecules;
      }

      /// <summary>
      ///     Gets the MoBi Parameter Start Values Building Block generated for the SBML Import.
      /// </summary>
      public ParameterStartValuesBuildingBlock GetMainParameterStartValuesBuildingBlock()
      {
         return
            _sbmlModule.ParameterStartValuesCollection.FirstOrDefault(
               mb => mb.Id == SBMLConstants.SBML_PARAMETERSTARTVALUES_BB);
      }

      /// <summary>
      ///     Gets the MoBi Molecule Start Values Building Block generated for the SBML Import.
      /// </summary>
      public MoleculeStartValuesBuildingBlock GetMainMSVBuildingBlock()
      {
         return
            _sbmlModule.MoleculeStartValuesCollection.FirstOrDefault(
               mb => mb.Id == SBMLConstants.SBML_MOLECULESTARTVALUES_BB);
      }

      /// <summary>
      ///     Gets the MoBi Container by a given SBML Compartment.
      /// </summary>
      public IEntity GetContainerFromCompartment(string compartment)
      {
         return GetMainTopContainer().GetSingleChildByName(compartment);
      }

      public IContainer GetContainerFromCompartment_(string compartment)
      {
         if (GetMainTopContainer() == null) return null;
         var children = GetMainTopContainer().GetAllChildren<IContainer>();
         return children.FirstOrDefault(child => child.Name == compartment);
      }

      /// <summary>
      ///     Gets the MoBi Molecule by it's name.
      /// </summary>
      protected MoleculeBuilder GetMoleculeByName(string moleculeName)
      {
         return _sbmlModule.Molecules.FindByName(moleculeName);
      }

      public void DoImport(Model sbmlModel, Module sbmlModule, SBMLInformation sbmlInformation, ICommandCollector command)
      {
         _sbmlModule = sbmlModule;
         _sbmlInformation = sbmlInformation;
         _command = command;
         try
         {
            Import(sbmlModel);
         }
         finally
         {
            _sbmlModule = null;
            _sbmlInformation = null;
            _command = null;
         }
      }


      protected abstract void Import(Model model);

      public abstract void AddToProject();
      public static IWithIdRepository IdRepository { get; private set; }
      public IObjectPathFactory ObjectPathFactory { get; set; }
      public IObjectBaseFactory ObjectBaseFactory { get; set; }
   }
}