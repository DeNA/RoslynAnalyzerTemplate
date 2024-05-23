using System.Linq;
using UnityEditor;

namespace Editor
{
    public class PackageExporter
    {
        [MenuItem("Tools/ExportPackage")]
        private static void Export()
        {
            var analyzerPluginPath = $"Assets/Plugins/{Constants.AnalyzerName}/";
            var exportPath = $"./{Constants.AnalyzerName}.unitypackage";

            var assetPathNames = AssetDatabase.FindAssets("", new[] { analyzerPluginPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .ToArray();

            AssetDatabase.ExportPackage(
                assetPathNames,
                exportPath,
                ExportPackageOptions.IncludeDependencies);
        }
    }
}
