using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Repositories;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeMoleculeTypeCommandSpecs : ContextSpecification<ChangeMoleculeTypeCommand>
   {
      protected MoleculeBuilder _moleculeBuilder;
      protected QuantityType _newType = QuantityType.Enzyme;
      private readonly QuantityType _oldType = QuantityType.Drug;
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _moleculeBuilder = new MoleculeBuilder().WithName("Drug");
         _moleculeBuilder.QuantityType = _oldType;
         _moleculeBuilder.Icon = ApplicationIcons.Drug.IconName;
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut = new ChangeMoleculeTypeCommand(_moleculeBuilder, _newType, _oldType, _buildingBlock);
      }
   }

   internal class When_excecuting_an_Change_molecule_type_command : concern_for_ChangeMoleculeTypeCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Resolve<IIconRepository>()).Returns(new IconRepository());
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_change_Type_property()
      {
         _moleculeBuilder.QuantityType.ShouldBeEqualTo(_newType);
      }

      [Observation]
      public void should_change_Icon()
      {
         _moleculeBuilder.Icon.ShouldBeEqualTo(ApplicationIcons.Enzyme.IconName);
      }
   }
}