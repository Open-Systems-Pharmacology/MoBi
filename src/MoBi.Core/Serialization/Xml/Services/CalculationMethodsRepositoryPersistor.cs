using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Serialization.Xml.Services
{
   public interface ICalculationMethodsRepositoryPersistor
   {
      void Load();
      void Save();
   }

   public class CalculationMethodsRepositoryPersistor : ICalculationMethodsRepositoryPersistor
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiConfiguration _configuration;
      private readonly IXmlSerializationService _xmlSerializationService;
      private readonly ICoreCalculationMethodRepository _calculationMethodRepository;

      public CalculationMethodsRepositoryPersistor(IMoBiContext context, IMoBiConfiguration configuration, 
         IXmlSerializationService xmlSerializationService, ICoreCalculationMethodRepository calculationMethodRepository)
      {
         _context = context;
         _configuration = configuration;
         _xmlSerializationService = xmlSerializationService;
         _calculationMethodRepository = calculationMethodRepository;
      }

      public void Load()
      {
         var xml = XElement.Load(_configuration.CalculationMethodRepositoryFile);
         int version = _xmlSerializationService.VersionFrom(xml);
         _xmlSerializationService.Deserialize<ICoreCalculationMethodRepository>(xml, _context.CurrentProject, version).All().Each(_calculationMethodRepository.AddCalculationMethod);
      }

      public void Save()
      {
         var xml = _xmlSerializationService.SerializeModelPart(_calculationMethodRepository);
         xml.Save(_configuration.CalculationMethodRepositoryFile);
      }
   }
}