using System.Collections.Generic;
using System.Linq;
using libsbmlcs;
using MoBi.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using Model = libsbmlcs.Model;

namespace MoBi.Engine.Sbml
{
    public class SBMLInformation
    {
        public int SboTerm = -1;
        public int Level { get; set; }
        public int Version { get; set; }
        public string ConversionFactor = "";
        public IAliasCreator AliasCreator { get; private set; }
        public List<NotificationMessage> NotificationMessages { get; set; }
        public List<SpeciesReference> SpeciesReferences { get; set; }
        public List<MoleculeInformation> MoleculeInformation { get; set; }

        //maps a sbml unit to it's equivalent mobi dimension 
        public Dictionary<string, IDimension> MobiDimension { get; set; }

        //maps the name of the dummy species to it's container
        public Dictionary<string, string> DummyNameContainerDictionary { get; set; }

        
        public SBMLInformation()
        {
            AliasCreator = new AliasCreator();
            MobiDimension = new Dictionary<string, IDimension>();
            NotificationMessages = new List<NotificationMessage>();
            DummyNameContainerDictionary = new Dictionary<string, string>();
            MoleculeInformation = new List<MoleculeInformation>();
        }


        private void saveSpeciesReferences(Model sbmlModel)
        {
            SpeciesReferences = new List<SpeciesReference>();
            for (long i = 0; i < sbmlModel.getNumReactions(); i++)
            {
                for (long a = 0; a < sbmlModel.getReaction(i).getNumReactants(); a++)
                {
                    var tmp = sbmlModel.getReaction(i).getReactant(a);
                    if (!tmp.isSetStoichiometry() || tmp.isSetId())
                    {
                        SpeciesReferences.Add(tmp);
                    }
                }
                for (long a = 0; a < sbmlModel.getReaction(i).getNumProducts(); a++)
                {
                    var tmp = sbmlModel.getReaction(i).getProduct(a);
                    if (!tmp.isSetStoichiometry() || tmp.isSetId())
                    {
                        SpeciesReferences.Add(tmp);
                    }
                }
            }
        }

        public void Initialize(Model sbmlModel, SBMLDocument sbmlDoc)
        {
            Level = (int)sbmlDoc.getLevel();
            Version = (int)sbmlDoc.getVersion();
            saveSpeciesReferences(sbmlModel);

            if (sbmlModel.isSetConversionFactor())
                ConversionFactor = sbmlModel.getConversionFactor();
            if (sbmlModel.isSetSBOTerm())
                SboTerm = sbmlModel.getSBOTerm();
        }

        public MoleculeBuilder GetMoleculeBySBMLId(string sbmlSpeciesId)
        {
           var moleculeInformation =MoleculeInformation.FirstOrDefault(info => info.SpeciesIds.Exists(s => s == sbmlSpeciesId));
           if (moleculeInformation == null) return null;
           var molecule = moleculeInformation.GetMoleculeBuilder();
           return molecule;
        }
    }
}
