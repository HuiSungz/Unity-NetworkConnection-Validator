
using System;
using UnityEditor;
using UnityEngine;

namespace NetworkConnector.Editor
{
    internal class NetworkConfigSettingsGroup
    {
        private readonly string _title;
        private readonly Action _drawContent;
        private bool _isExpanded = true;  // 토글 상태 추가

        public NetworkConfigSettingsGroup(string title, Action drawContent)
        {
            _title = title;
            _drawContent = drawContent;
        }

        public void Draw()
        {
            EditorGUILayout.Space(5);

            // 섹션 헤더 배경 그리기
            var headerRect = GUILayoutUtility.GetRect(GUIContent.none, NetworkConfigEditorStyles.SectionHeaderStyle, GUILayout.Height(22));
        
            // 기본 배경색 먼저 그리기
            EditorGUI.DrawRect(headerRect, NetworkConfigEditorStyles.SectionHeaderBackgroundColor);
        
            // 항상 어두운 하이라이트 추가
            EditorGUI.DrawRect(headerRect, NetworkConfigEditorStyles.SectionDarkHighlightColor);

            // 토글 영역
            var toggleRect = new Rect(headerRect);
            toggleRect.xMin += 15; // 들여쓰기

            // 토글과 레이블
            _isExpanded = EditorGUI.Foldout(toggleRect, _isExpanded, _title, true, NetworkConfigEditorStyles.SectionFoldoutStyle);

            if (_isExpanded)
            {
                EditorGUILayout.Space(2);
                
                // 컨텐츠 영역
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.Space(2);
                _drawContent?.Invoke();
                EditorGUILayout.Space(2);
                EditorGUILayout.EndVertical();
            }
        }
    }
}