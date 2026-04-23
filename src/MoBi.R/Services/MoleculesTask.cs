using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Services;

public interface IMoleculesTask
{
   string[] AllMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllMoleculeNamesOfType(MoleculeBuildingBlock buildingBlock, QuantityType quantityType);
   string[] AllXenobioticFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllEndogenousStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string MoleculeTypeFor(MoleculeBuildingBlock buildingBlock, string moleculeName);
}

public class MoleculesTask : IMoleculesTask
{
   public string[] AllMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.Select(x => x.Name).ToArray();

   public string[] AllFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.Where(x => x.IsFloating).Select(x => x.Name).ToArray();

   public string[] AllStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.Where(x => !x.IsFloating).Select(x => x.Name).ToArray();

   public string[] AllMoleculeNamesOfType(MoleculeBuildingBlock buildingBlock, QuantityType quantityType) =>
      buildingBlock.Where(x => x.QuantityType.Is(quantityType)).Select(x => x.Name).ToArray();

   public string[] AllXenobioticFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.Where(x => x.IsFloating && x.IsXenobiotic).Select(x => x.Name).ToArray();

   public string[] AllEndogenousStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.Where(x => !x.IsFloating && !x.IsXenobiotic).Select(x => x.Name).ToArray();

   public string MoleculeTypeFor(MoleculeBuildingBlock buildingBlock, string moleculeName)
   {
      var molecule = buildingBlock[moleculeName];
      return molecule == null ? throw new ArgumentException(AppConstants.Exceptions.MoleculeNotFoundInBuildingBlock(moleculeName)) : molecule.QuantityType.ToString();
   }
}
