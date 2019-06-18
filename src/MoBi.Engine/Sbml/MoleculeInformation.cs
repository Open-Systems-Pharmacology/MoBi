using System;
using System.Collections.Generic;
using System.Linq;
using libsbmlcs;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Engine.Sbml
{
    public class MoleculeInformation
    {
        private List<Species> _allSpecies;
        internal List<string> SpeciesIds ;
        private IMoleculeBuilder _moleculeBuilder;
        private List<IContainer> _container; 
        private IDimension _dimension;

        /// <summary>
        ///     Checks if there are multiple occurences of one compartment in the container List.
        /// </summary>
        public bool IsMultipleTimesInOneCompartment()
        {
            return _container.Select(container => container.Name).Any(name => _container.Any(con => name == con.Name));
        }

        public MoleculeInformation(Species species)
        {
            _allSpecies = new List<Species> {species};
            _container = new List<IContainer>();
            SpeciesIds = new List<string> {species.getId()};
        }

        public MoleculeInformation(IMoleculeBuilder moleculeBuilder)
        {
            _moleculeBuilder = moleculeBuilder;
            _container = new List<IContainer>();
            SpeciesIds = new List<string>();
        }

        public MoleculeInformation(Species species, IMoleculeBuilder moleculeBuilder)
        {
            _allSpecies = new List<Species> { species };
            SpeciesIds = new List<string> { species.getId() };
            _moleculeBuilder = moleculeBuilder;
        }

        public List<IContainer> GetContainer()
        {
            return _container;
        }

        public void SetContainer(List<IContainer> container)
        {
            _container = container;
        }

        public void AddContainer(IContainer container)
        {
            if (_container == null) _container = new List<IContainer>();
            _container.Add(container);
        }

        public string GetCompartment(Species species)
        {
            if (_allSpecies == null||species == null) return String.Empty;
            var sp = _allSpecies.FirstOrDefault(s => s.getId() == species.getId());
            return sp == null ? String.Empty : sp.getCompartment();
        }

        public Species GetSpeciesIfOne()
        {
            return _allSpecies.Count == 1 ? _allSpecies.First() : null;
        }

        public List<Species> GetAllSpecies()
        {
            return _allSpecies;
        }

        public void SetSpecies(Species species)
        {
            if (_allSpecies == null) _allSpecies = new List<Species>();
            _allSpecies.Add(species);
            SpeciesIds.Add(species.getId());
        }

        public List<string> GetAllSpeciesIds()
        {
            return _allSpecies.Select(s => s.getId()).ToList();
        }

        public string GetSpeciesNameIfOne()
        {
            return _allSpecies.Count == 1 ? _allSpecies.First().getName() : String.Empty;
        }

        public IMoleculeBuilder GetMoleculeBuilder()
        {
            return _moleculeBuilder;
        }

        public void SetMoleculeBuilder(IMoleculeBuilder mol)
        {
            _moleculeBuilder = mol;
        }

        public string GetMoleculeBuilderName()
        {
            return _moleculeBuilder == null ? String.Empty : _moleculeBuilder.Name;
        }

        public void SetDimension(IDimension dimension)
        {
            _dimension = dimension;
        }

        public IDimension GetDimension()
        {
            return _dimension;
        }

    }
}
