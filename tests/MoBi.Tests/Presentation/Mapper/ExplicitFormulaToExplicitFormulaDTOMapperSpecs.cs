using FakeItEasy;
using MoBi.Assets;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_ExplicitFormulaToExplicitFormulaDTOMapper : ContextSpecification<IExplicitFormulaToExplicitFormulaDTOMapper>
   {
      private IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathMapper;
      private IContainer _topContainer;
      private IContainer _parentContainer;
      protected IParameter _parameter;
      protected ExplicitFormula _explicitFormula;
      protected IParameter _parameter2;

      protected override void Context()
      {
         _formulaUsablePathMapper = A.Fake<IFormulaUsablePathToFormulaUsablePathDTOMapper>();
         sut = new ExplicitFormulaToExplicitFormulaDTOMapper(_formulaUsablePathMapper);

         _explicitFormula = new ExplicitFormula();
         _explicitFormula.AddObjectPath(
            new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, "P2")
               .WithDimension(DimensionFactoryForSpecs.MassDimension)
               .WithAlias(AppConstants.Param)
            );


         _explicitFormula.FormulaString = "Param*2";

         _topContainer = new Container().WithName("TOP");
         _parentContainer = new Container().WithName("Parent");
         _topContainer.Add(_parentContainer);
         _parameter = new Parameter().WithName("P").WithFormula(_explicitFormula);
         _parameter2 = new Parameter().WithName("P2").WithFormula(new ConstantFormula(2));
         _parentContainer.Add(_parameter);
         _parentContainer.Add(_parameter2);
      }
   }

  
}