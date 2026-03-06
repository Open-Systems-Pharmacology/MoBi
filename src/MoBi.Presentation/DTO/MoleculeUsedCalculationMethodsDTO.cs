using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO;

public class MoleculeUsedCalculationMethodsDTO : DxValidatableDTO, IWithName, IViewItem
{
   private readonly List<UsedCalculationMethodDTO> _usedCalculationMethods = new();

   public MoleculeUsedCalculationMethodsDTO()
   {

   }

   public void AddUsedCalculationMethod(UsedCalculationMethodDTO usedCalculationMethodDTO) => _usedCalculationMethods.Add(usedCalculationMethodDTO);

   public IReadOnlyList<UsedCalculationMethodDTO> UsedCalculationMethods => _usedCalculationMethods;

   public string MoleculeName { get; set; }

   public string Icon { get; init; }

   public string Name
   {
      get => MoleculeName;
      set => MoleculeName = value;
   }
}