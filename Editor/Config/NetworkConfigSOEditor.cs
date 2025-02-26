
using UnityEditor;
using UnityEngine;

namespace NetworkConnector.Editor
{
    [CustomEditor(typeof(NetworkConfigSO))]
    public class NetworkConfigSOEditor : UnityEditor.Editor
    {
        private NetworkConfigSettingsGroup[] _settingsGroups;
        private SerializedProperty _httpUrls;
        private SerializedProperty _httpTimeout;
        private SerializedProperty _pingHosts;
        private SerializedProperty _pingTimeout;
        private SerializedProperty _baseRetryInterval;
        private SerializedProperty _maxRetryInterval;
        private SerializedProperty _retryBackoffMultiplier;
        private SerializedProperty _isVerboseLog;

        private void OnEnable()
        {
            InitializeProperties();
            InitializeSettingsGroups();
        }

        private void InitializeProperties()
        {
            _httpUrls = serializedObject.FindProperty("httpUrls");
            _httpTimeout = serializedObject.FindProperty("httpTimeout");
            _pingHosts = serializedObject.FindProperty("pingHosts");
            _pingTimeout = serializedObject.FindProperty("pingTimeout");
            _baseRetryInterval = serializedObject.FindProperty("baseRetryInterval");
            _maxRetryInterval = serializedObject.FindProperty("maxRetryInterval");
            _retryBackoffMultiplier = serializedObject.FindProperty("retryBackoffMultiplier");
            _isVerboseLog = serializedObject.FindProperty("isVerboseLog");
        }

        private void InitializeSettingsGroups()
        {
            _settingsGroups = new[]
            {
                new NetworkConfigSettingsGroup(NetworkConfigContent.HTTP.TITLE, DrawHttpSettings),
                new NetworkConfigSettingsGroup(NetworkConfigContent.Ping.TITLE, DrawPingSettings),
                new NetworkConfigSettingsGroup(NetworkConfigContent.Retry.TITLE, DrawRetrySettings),
                new NetworkConfigSettingsGroup(NetworkConfigContent.Debug.TITLE, DrawDebugSettings)
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(10);
            DrawHeaderSection();
            EditorGUILayout.Space(5);

            foreach (var group in _settingsGroups)
            {
                group.Draw();
                EditorGUILayout.Space(10);
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(20);
            DrawBottomButtons();
        }
        
        private void DrawHeaderSection()
        {
            // 메인 헤더 배경
            var headerRect = GUILayoutUtility.GetRect(GUIContent.none, NetworkConfigEditorStyles.HeaderStyle, GUILayout.Height(50));
            EditorGUI.DrawRect(headerRect, NetworkConfigEditorStyles.HeaderBackgroundColor);

            // 아이콘 영역 (선택적)
            var iconRect = new Rect(headerRect.x + 10, headerRect.y + 10, 30, 30);
            var labelRect = new Rect(iconRect.xMax + 10, headerRect.y, headerRect.width - iconRect.width - 20, headerRect.height);

            // 아이콘 그리기 (Builtin 아이콘 사용)
            GUI.Label(iconRect, EditorGUIUtility.IconContent("Settings"));

            // 타이틀 그리기
            GUI.Label(labelRect, NetworkConfigContent.Common.HEADER, NetworkConfigEditorStyles.HeaderStyle);
    
            EditorGUILayout.Space(5);
            NetworkConfigEditorStyles.DrawUILine(Color.gray);
    
            // 설명 섹션
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.Space(2);
            
            EditorGUILayout.LabelField("네트워크 연결 검증 및 재시도 설정을 구성합니다.", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("각 설정 항목에 마우스를 올리면 자세한 설명을 확인할 수 있습니다.", EditorStyles.miniLabel);
    
            EditorGUILayout.Space(2);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        private void DrawHttpSettings()
        {
            // URL 리스트
            EditorGUILayout.LabelField(new GUIContent(
                NetworkConfigContent.HTTP.URL_LIST,
                NetworkConfigContent.HTTP.URL_TOOLTIP
            ), EditorStyles.boldLabel);
            
            EditorGUI.indentLevel++;
            
            for (int i = 0; i < _httpUrls.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_httpUrls.GetArrayElementAtIndex(i), 
                    new GUIContent($"URL [{i}]", NetworkConfigContent.HTTP.URL_TOOLTIP));
                
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    _httpUrls.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add URL", GUILayout.Width(100)))
            {
                _httpUrls.InsertArrayElementAtIndex(_httpUrls.arraySize);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_httpTimeout, new GUIContent(NetworkConfigContent.HTTP.TIMEOUT, NetworkConfigContent.HTTP.TIMEOUT_TOOLTIP));

            if (_httpUrls.arraySize == 0)
            {
                EditorGUILayout.HelpBox(NetworkConfigContent.HTTP.EMPTY_WARNING, MessageType.Warning);
            }
        }

        private void DrawPingSettings()
        {
            EditorGUILayout.LabelField(new GUIContent(
                NetworkConfigContent.Ping.HOST_LIST,
                NetworkConfigContent.Ping.HOST_TOOLTIP
            ), EditorStyles.boldLabel);
            
            EditorGUI.indentLevel++;
            
            for (int i = 0; i < _pingHosts.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_pingHosts.GetArrayElementAtIndex(i), 
                    new GUIContent($"HOST [{i}]", NetworkConfigContent.Ping.HOST_TOOLTIP));
                
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    _pingHosts.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add Host", GUILayout.Width(100)))
            {
                _pingHosts.InsertArrayElementAtIndex(_pingHosts.arraySize);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.indentLevel--;

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(_pingTimeout, new GUIContent(NetworkConfigContent.Ping.TIMEOUT, NetworkConfigContent.Ping.TIMEOUT_TOOLTIP));

            if (_pingHosts.arraySize == 0)
            {
                EditorGUILayout.HelpBox(NetworkConfigContent.Ping.EMPTY_WARNING, MessageType.Warning);
            }
        }

        private void DrawRetrySettings()
        {
            EditorGUILayout.PropertyField(_baseRetryInterval, new GUIContent(NetworkConfigContent.Retry.BASE_INTERVAL, NetworkConfigContent.Retry.BASE_INTERVAL_TOOLTIP));
            EditorGUILayout.PropertyField(_maxRetryInterval, new GUIContent(NetworkConfigContent.Retry.MAX_INTERVAL, NetworkConfigContent.Retry.MAX_INTERVAL_TOOLTIP));
            EditorGUILayout.PropertyField(_retryBackoffMultiplier, new GUIContent(NetworkConfigContent.Retry.MULTIPLIER, NetworkConfigContent.Retry.MULTIPLIER_TOOLTIP));

            if (_maxRetryInterval.intValue < _baseRetryInterval.intValue)
            {
                EditorGUILayout.HelpBox(NetworkConfigContent.Retry.INTERVAL_ERROR, MessageType.Error);
            }
        }

        private void DrawDebugSettings()
        {
            EditorGUILayout.PropertyField(_isVerboseLog, new GUIContent(NetworkConfigContent.Debug.VERBOSE_LOG, NetworkConfigContent.Debug.VERBOSE_LOG_TOOLTIP));
        }

        private void DrawBottomButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button(NetworkConfigContent.Common.RESET_BUTTON, GUILayout.Height(30)))
            {
                HandleResetButton();
            }

            if (GUILayout.Button(NetworkConfigContent.Common.SAVE_BUTTON, GUILayout.Height(30)))
            {
                HandleSaveButton();
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void HandleResetButton()
        {
            if (EditorUtility.DisplayDialog(
                NetworkConfigContent.Common.RESET_TITLE, 
                NetworkConfigContent.Common.RESET_MESSAGE, 
                NetworkConfigContent.Common.CONFIRM, 
                NetworkConfigContent.Common.CANCEL))
            {
                var config = (NetworkConfigSO)target;
                Undo.RecordObject(config, "Reset Network Config");
                config.Reset();
                EditorUtility.SetDirty(config);
            }
        }

        private void HandleSaveButton()
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog(
                NetworkConfigContent.Common.SAVE_TITLE, 
                NetworkConfigContent.Common.SAVE_MESSAGE, 
                NetworkConfigContent.Common.CONFIRM);
        }
    }
}