using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectReferenceAtTransportPresenter : ISelectReferencePresenter
   {
      void Init(IEntity refObjectBase, IEnumerable<IObjectBase> entities, ITransportBuilder transportBuilder);
   }

   internal class SelectReferenceAtTransportPresenter : SelectReferencePresenterBase, ISelectReferenceAtTransportPresenter
   {
      private readonly IObjectPathCreatorAtTransport _objectPathCreatorAtTransport;
      private readonly ITransportMoleculeContainerToObjectBaseDTOMapper _transporterMoleculeContainerMapper;

      public SelectReferenceAtTransportPresenter(ISelectReferenceView view, IObjectBaseToObjectBaseDTOMapper objectBaseDTOMapper,
         IMoBiContext context,IUserSettings userSettings, IObjectBaseToDummyMoleculeDTOMapper objectBaseToMoleculeDummyMapper,
         IParameterToDummyParameterDTOMapper dummyParameterDTOMapper, IObjectBaseDTOToReferenceNodeMapper referenceMapper, 
         IObjectPathCreatorAtTransport objectPathCreatorAtTransport, ITransportMoleculeContainerToObjectBaseDTOMapper transporterMoleculeContainerMapper)
         : base(view, objectBaseDTOMapper, context, userSettings,
            objectBaseToMoleculeDummyMapper, dummyParameterDTOMapper, referenceMapper, objectPathCreatorAtTransport, Localisations.NeighborhoodsOnly)
      {
         _objectPathCreatorAtTransport = objectPathCreatorAtTransport;
         _transporterMoleculeContainerMapper = transporterMoleculeContainerMapper;
      }


      public void Init(IEntity refObjectBase, IEnumerable<IObjectBase> entities, ITransportBuilder transportBuilder)
      {
         //Nessecary to create correct paths
         _objectPathCreatorAtTransport.Transport = transportBuilder; 
         base.Init(refObjectBase, entities, transportBuilder);
      }

      protected override void AddSpecificInitalObjects()
      {
         AddTimeReference();
         AddSpatialStructures();
      }

      protected override void AddChildrenFromDummyMolecule(List<IObjectBaseDTO> children, DummyMoleculeContainerDTO dummyMolecule)
      {
         IEnumerable<TransporterMoleculeContainer> allTransporterMoleculeContainers = new List<TransporterMoleculeContainer>();
         IMoleculeBuildingBlock editedMoleculeBuildingBlock = null;
         var allMoleculeBuildingBlocks = editedMoleculeBuildingBlock!=null ? new[] {editedMoleculeBuildingBlock} : _context.CurrentProject.MoleculeBlockCollection;

         foreach (var moleculeBuildingBlock in allMoleculeBuildingBlocks)
         {
            if (moleculeBuildingBlock.Any(builder => builder.Name.Equals(dummyMolecule.Name)))
            {
               allTransporterMoleculeContainers = moleculeBuildingBlock[dummyMolecule.Name].TransporterMoleculeContainerCollection.Union(allTransporterMoleculeContainers);
               allTransporterMoleculeContainers = allTransporterMoleculeContainers.Distinct(new NameComparer<TransporterMoleculeContainer>());
            }
         }

         children.AddRange(allTransporterMoleculeContainers.MapAllUsing(_transporterMoleculeContainerMapper));
         base.AddChildrenFromDummyMolecule(children, dummyMolecule);
      }
   }
}