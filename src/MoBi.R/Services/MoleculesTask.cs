using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Core.Serialization.Services.ICoreSerializationTask;

namespace MoBi.R.Services;

public interface IMoleculesTask : IBuildingBlockTask<MoleculeBuildingBlock>
{
   string[] AllMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllMoleculeNamesOfType(MoleculeBuildingBlock buildingBlock, QuantityType quantityType);
   string[] AllXenobioticFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string[] AllEndogenousStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock);
   string MoleculeTypeFor(MoleculeBuildingBlock buildingBlock, string moleculeName);
}

public class MoleculesTask : BuildingBlockTask<MoleculeBuildingBlock>, IMoleculesTask
{
   public MoleculesTask(ISerializationTask serializationTask) : base(serializationTask)
   {
   }

   public string[] AllMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.AllNames().ToArray();

   public string[] AllFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.AllFloating().AllNames().ToArray();

   public string[] AllStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.AllStationary().AllNames().ToArray();

   public string[] AllMoleculeNamesOfType(MoleculeBuildingBlock buildingBlock, QuantityType quantityType) =>
      buildingBlock.AllOfType(quantityType).AllNames().ToArray();

   public string[] AllXenobioticFloatingMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.AllXenobioticFloating().AllNames().ToArray();

   public string[] AllEndogenousStationaryMoleculeNames(MoleculeBuildingBlock buildingBlock) =>
      buildingBlock.AllEndogenousStationary().AllNames().ToArray();

   public string MoleculeTypeFor(MoleculeBuildingBlock buildingBlock, string moleculeName)
   {
      var molecule = buildingBlock[moleculeName];
      return molecule == null ? throw new ArgumentException(AppConstants.Exceptions.MoleculeNotFoundInBuildingBlock(moleculeName)) : molecule.QuantityType.ToString();
   }
}