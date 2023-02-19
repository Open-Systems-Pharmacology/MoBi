using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateMoleculeStartValuesInBuildingBlockCommand : ContextSpecification<UpdateMoleculeStartValueInBuildingBlockCommand>
   {
      protected IMoleculeStartValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected ObjectPath _path;

      private IDimension _fakeDimension;

      protected override void Context()
      {
         _fakeDimension = A.Fake<IDimension>();
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new MoleculeStartValuesBuildingBlock();

         var msv = new MoleculeStartValue { Path = new ObjectPath("path1"), Dimension = _fakeDimension, StartValue = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1),IsPresent = false};

         _buildingBlock.Add(msv);
         _path = msv.Path;

         sut = new UpdateMoleculeStartValueInBuildingBlockCommand(_buildingBlock, _path, 1.0, true, 22.0, true);
         A.CallTo(() => _context.Get<IMoleculeStartValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class When_updating_molecule_start_values : concern_for_UpdateMoleculeStartValuesInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void command_fieds_updated()
      {
         sut.CommandType.ShouldBeEqualTo(AppConstants.Commands.UpdateCommand);
         sut.Description.ShouldBeEqualTo(AppConstants.Commands.UpdateMoleculeStartValue(_path, 1.0, true, _buildingBlock[_path].DisplayUnit, 22.0, true));
         sut.ObjectType.ShouldBeEqualTo(ObjectTypes.MoleculeStartValue);
      }

      [Observation]
      public void start_values_must_be_updated()
      {
         var msv = _buildingBlock[_path];

         if (msv.Value == null) return;

         var value = (double)msv.Value;
         value.ShouldBeGreaterThanOrEqualTo(0);
         msv.IsPresent.ShouldBeTrue();
         msv.NegativeValuesAllowed.ShouldBeTrue();

      }

      [Observation]
      public void scale_factor_must_be_updated()
      {
         var msv = _buildingBlock[_path];

         msv.ScaleDivisor.ShouldBeEqualTo(22.0);
      }
   }


   public class When_retrieving_molecule_inverse : concern_for_UpdateMoleculeStartValuesInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_reverse_updates_to_result_in_original_value()
      {
         _buildingBlock.Count().ShouldBeEqualTo(1);
         var msv = _buildingBlock[_path];

         if (msv.Value == null) return;
         
         var startValue = (double)msv.Value;
         startValue.ShouldBeSmallerThan(0.0 + double.Epsilon);
      }
   }

}
