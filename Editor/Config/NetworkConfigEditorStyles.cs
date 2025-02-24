using UnityEditor;
using UnityEngine;

namespace Network.Editor
{
    internal static class NetworkConfigEditorStyles
    {
        public static Color HeaderBackgroundColor => EditorGUIUtility.isProSkin 
            ? new Color(0.1f, 0.1f, 0.1f) 
            : new Color(0.2f, 0.2f, 0.2f);
            
        public static Color SectionHeaderBackgroundColor => EditorGUIUtility.isProSkin 
            ? new Color(0.1f, 0.1f, 0.1f, 0.95f) 
            : new Color(0.8f, 0.8f, 0.8f, 0.95f);
        
        public static Color SectionDarkHighlightColor => EditorGUIUtility.isProSkin 
            ? new Color(0.1f, 0.1f, 0.1f, 0.6f) 
            : new Color(0.1f, 0.1f, 0.1f, 0.3f);

        public static GUIStyle HeaderStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 16,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(10, 10, 8, 8),
                    margin = new RectOffset(0, 0, 0, 0),
                    richText = true
                };

                style.normal.textColor = EditorGUIUtility.isProSkin 
                    ? new Color(0.9f, 0.9f, 0.9f) 
                    : Color.white;

                return style;
            }
        }

        public static GUIStyle SectionHeaderStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 12,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(10, 10, 6, 6),
                    margin = new RectOffset(0, 0, 0, 0)
                };

                style.normal.textColor = EditorGUIUtility.isProSkin 
                    ? new Color(0.9f, 0.9f, 0.9f) 
                    : Color.white;

                return style;
            }
        }

        public static GUIStyle SectionFoldoutStyle
        {
            get
            {
                var style = new GUIStyle(EditorStyles.foldout)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(15, 10, 4, 4),
                    margin = new RectOffset(0, 0, 0, 0)
                };

                style.normal.textColor = EditorGUIUtility.isProSkin 
                    ? new Color(0.9f, 0.9f, 0.9f) 
                    : Color.white;

                return style;
            }
        }

        public static void DrawUILine(Color color, int thickness = 1, int padding = 10)
        {
            var rect = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            rect.height = thickness;
            rect.y += padding / 2;
            EditorGUI.DrawRect(rect, color);
        }
    }
}