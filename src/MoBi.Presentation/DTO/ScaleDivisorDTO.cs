using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ScaleDivisorDTO : BreadCrumbsDTO<IMoleculeAmount>
   {
      public IMoleculeAmount MoleculeAmount { get; private set; }


      public ScaleDivisorDTO(IMoleculeAmount moleculeAmount) : base(moleculeAmount)
      {
         MoleculeAmount = moleculeAmount;
      }

      public virtual double ScaleDivisor
      {
         get { return MoleculeAmount.ScaleDivisor; }
         set { /*nothing to do here. Dealt with in command*/}
      }

      public string PathAsString
      {
         get { return  ContainerPath.Clone<IObjectPath>().AndAdd(Name).ToString(); }
      }

      public string Name
      {
         get { return MoleculeAmount.Name; }
      }
   }
}