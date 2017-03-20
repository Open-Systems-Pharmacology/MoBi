using System;
using System.IO;

namespace MoBi.Core.SBML
{
    public class Helper
    {
        public static string TestFileFullPath(string fileName)
        {
            var dataFolder = Path.Combine(Path.Combine(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\.."),"Core","SBML", "Testfiles")));
            return Path.Combine(dataFolder, fileName);
        }
    }
}