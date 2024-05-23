using UnityEditor;

namespace Editor
{
    public class AnalyzerDllModifier : AssetPostprocessor
    {
        private readonly string _analyzerDLLPath =
            $"Assets/Plugins/{Constants.AnalyzerName}/{Constants.AnalyzerName}.dll";

        private const string RoslynAnalyzerLabel = "RoslynAnalyzer";

        private void OnPreprocessAsset()
        {
            if (assetPath != _analyzerDLLPath)
            {
                return;
            }

            var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
            if (importer == null)
            {
                return;
            }

            if (!importer.importSettingsMissing)
            {
                return;
            }

            importer.SetCompatibleWithAnyPlatform(false);
            AssetDatabase.SetLabels(importer, new[] { RoslynAnalyzerLabel });
        }
    }
}
