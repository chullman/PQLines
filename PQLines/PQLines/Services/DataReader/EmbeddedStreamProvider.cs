using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using PQLines.Models;

namespace PQLines.Services.DataReader
{
    // Given the name of a root level model (e.g. class CondContent), find the appropriate JSON file in the project and returns it's System.IO.Stream
    //
    // As System.IO.FileStream is not supported with PCL projects, we instead need to find and read the JSON files based on their manifest resource names
    //      With reference to: http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/files/#Loading_Files_Embedded_as_Resources
    //
    // IMPORTANT: For this to work, the JSON files must be set to be built as "Embedded Resource"
    // IMPORTANT: For this to work, the name of the root level model (e.g. class CondContent) must match the file name (i.e. CondContent.json)

    public class EmbeddedStreamProvider : IStreamProvider
    {
        public Stream GetResourceStream<T>(string fileExtension) where T : IRootModel
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            string embeddedResourcePath;
            try
            {
                embeddedResourcePath = GetResourceLocation<T>(assembly, fileExtension);
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: Unable to retrieve the path of the - check the JSON file is set to build as Embedded Resource," +
                                " and also check that the name of the root level model (e.g. class CondContent) matches the file name (i.e. CondContent.json)");
                throw;
            }
            

            return assembly.GetManifestResourceStream(embeddedResourcePath);
        }

        private string GetResourceLocation<T>(Assembly assembly, string fileExtension) where T : IRootModel
        {
            // Quick check that the "." in, for example, ".json" has been passed into the fileExtension argument
            if (!(fileExtension[0].Equals('.')))
            {
                fileExtension = "." + fileExtension;
            }

            var type = typeof (T);

            var resourceFileName = type.Name + fileExtension;

            // Get the particular Embedded Resource path string based on the interpolated file name (e.g. "CondContent.json")
            // This is assuming that the file name of the JSON is unique in this project
            var embeddedResourcePath = assembly.GetManifestResourceNames().First(name => name.Contains(resourceFileName));

            return embeddedResourcePath;
        }
    }
}