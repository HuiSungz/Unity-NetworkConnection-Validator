
using UnityEngine;

namespace NetworkConnector
{
    // [CreateAssetMenu(fileName = "NetworkConfig", menuName = "Network/Network Config")]
    public class NetworkConfigSO : ScriptableObject
    { 
        [SerializeField] private string[] httpUrls = { "https://www.google.com" };
        [SerializeField, Range(1f, 10f)] private float httpTimeout = 3f;
        [SerializeField] private string[] pingHosts = { "8.8.8.8" };
        [SerializeField, Range(500, 5000)] private int pingTimeout = 3000;
        [SerializeField, Range(50, 1000)] private int baseRetryInterval = 100;
        [SerializeField, Range(1000, 10000)] private int maxRetryInterval = 5000;
        [SerializeField, Range(1.1f, 3f)] private float retryBackoffMultiplier = 1.5f;
        [SerializeField] private bool isVerboseLog;
        
        public string[] HttpUrls => httpUrls;
        public float HttpTimeout => httpTimeout;
        public string[] PingHosts => pingHosts;
        public int PingTimeout => pingTimeout;
        public int BaseRetryInterval => baseRetryInterval;
        public int MaxRetryInterval => maxRetryInterval;
        public float RetryBackoffMultiplier => retryBackoffMultiplier;
        public bool IsVerboseLog => isVerboseLog;
        
        private static NetworkConfigSO _instance;
        public static NetworkConfigSO Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }
                
                _instance = Resources.Load<NetworkConfigSO>("NetworkConfig");
                if (_instance)
                {
                    return _instance;
                }
                
                Debug.LogError("NetworkConfig를 찾을 수 없습니다. Resources 폴더에 NetworkConfig가 있는지 확인하세요.");
                _instance = CreateInstance<NetworkConfigSO>();
                
                return _instance;
            }
        }

#if UNITY_EDITOR
        public void Reset()
        {
            httpUrls = new[] {"https://www.google.com"};
            httpTimeout = 3f;
            pingHosts = new[] {"8.8.8.8"};
            pingTimeout = 3000;
            baseRetryInterval = 100;
            maxRetryInterval = 5000;
            retryBackoffMultiplier = 1.5f;
            isVerboseLog = false;
        }
        
        [UnityEditor.InitializeOnLoadMethod]
        private static void EnsureNetworkConfigExists()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                const string configPath = "Assets/Network/Resources";
                const string configName = "NetworkConfig.asset";
                string fullPath = $"{configPath}/{configName}";
                
                if (!System.IO.Directory.Exists(configPath))
                {
                    System.IO.Directory.CreateDirectory(configPath);
                    UnityEditor.AssetDatabase.Refresh();
                }

                var config = UnityEditor.AssetDatabase.LoadAssetAtPath<NetworkConfigSO>(fullPath);
                if (config != null)
                {
                    return;
                }

                try
                {
                    config = CreateInstance<NetworkConfigSO>();
                    UnityEditor.AssetDatabase.CreateAsset(config, fullPath);
                    UnityEditor.AssetDatabase.SaveAssets();
                    UnityEditor.AssetDatabase.Refresh();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to create NetworkConfig asset at {fullPath}: {ex.Message}");
                }
            };
        }
#endif
    }
}