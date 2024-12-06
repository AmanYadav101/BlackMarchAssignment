using Game.Scripts.Obstacle;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor
{
    [CustomEditor(typeof(ObstacleData))]
    public class ObstacleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ObstacleData data = (ObstacleData)target;

            EditorGUILayout.LabelField("Obstacle Grid Editor", EditorStyles.boldLabel);

            for (int x = 0; x < 10; x++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int y = 0; y < 10; y++)
                {
                    bool currentValue = data.GetObstacleAt(x, y);
                    bool newValue = GUILayout.Toggle(currentValue, "", GUILayout.Width(20), GUILayout.Height(20));

                    if (newValue != currentValue)
                    {
                        data.SetObstacleAt(x, y, newValue);

                        EditorUtility.SetDirty(data);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                AssetDatabase.SaveAssets();
            }
        }
    }
}