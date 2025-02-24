
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Network.Editor
{
    public static class NetworkConfigMenu
    {
        private const string ConfigPath = "Assets/Network/Resources";
        private const string ConfigName = "NetworkConfig.asset";

        [MenuItem("Tools/NConnector/Network Config")]
        public static void OpenOrCreateNetworkConfig()
        {
            string fullPath = Path.Combine(ConfigPath, ConfigName);
            
            NetworkConfigSO config = AssetDatabase.LoadAssetAtPath<NetworkConfigSO>(fullPath);
            if (config == null)
            {
                if (!Directory.Exists(ConfigPath))
                {
                    Directory.CreateDirectory(ConfigPath);
                    AssetDatabase.Refresh();
                }
                
                config = ScriptableObject.CreateInstance<NetworkConfigSO>();
                AssetDatabase.CreateAsset(config, fullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }
    }
}