using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers;

public abstract class MoleculeUsedCalculationMethodsDTOMapper<T> : IMapper<T, IReadOnlyList<MoleculeUsedCalculationMethodsDTO>>
{
   public IReadOnlyList<MoleculeUsedCalculationMethodsDTO> MapFrom(T input)
   {
      var molecules = AllMolecules(input)?.Where(x => x.IsFloating);
      
      if(molecules == null)
         return new List<MoleculeUsedCalculationMethodsDTO>();

      var cache = new Cache<string, MoleculeUsedCalculationMethodsDTO>();
      molecules.Each(m =>
      {
         var overridingCalculationMethods = MoleculeUsedCalculationMethodsFor(input, m);
         if (!overridingCalculationMethods.Any())
            return;

         var dto = new MoleculeUsedCalculationMethodsDTO
         {
            MoleculeName = m.Name,
            Icon = m.Icon
         };

         overridingCalculationMethods.Each(x => dto.AddUsedCalculationMethod(usedCalculationMethodDTOFor(x)));

         // The intent is to have only one entry per molecule, so if there are multiple, we just overwrite the previous one.
         cache[m.Name] = dto;
      });

      return cache.ToList();
   }

   private static UsedCalculationMethodDTO usedCalculationMethodDTOFor(UsedCalculationMethod usedCalculationMethod)
   {
      return new UsedCalculationMethodDTO { Category = usedCalculationMethod.Category, CalculationMethodName = usedCalculationMethod.CalculationMethod };
   }

   protected abstract IReadOnlyList<MoleculeBuilder> AllMolecules(T input);
   protected abstract IReadOnlyCollection<UsedCalculationMethod> MoleculeUsedCalculationMethodsFor(T input, MoleculeBuilder m);
}