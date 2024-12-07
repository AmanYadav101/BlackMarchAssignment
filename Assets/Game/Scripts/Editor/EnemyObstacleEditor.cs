using Game.Scripts.Enemy;
using Game.Scripts.Obstacle;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor
{
    /// <summary>
    /// Custom editor for EnemyObstacleData, allowing a grid-based toggle system.
    /// Only one grid cell can be set to true at a time.
    /// </summary>
    [CustomEditor(typeof(EnemyObstacleData))]
    public class EnemyObstacleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EnemyObstacleData data = (EnemyObstacleData)target;

            EditorGUILayout.LabelField("Enemy Grid Editor", EditorStyles.boldLabel);

            // Track the currently selected position
            Vector2Int selectedPosition = data.GetSelectedPosition();

            for (int x = 0; x < 10; x++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int y = 0; y < 10; y++)
                {
                    bool currentValue = (selectedPosition.x == x && selectedPosition.y == y);
                    bool newValue = GUILayout.Toggle(currentValue, "", GUILayout.Width(20), GUILayout.Height(20));

                    if (newValue != currentValue)
                    {
                        if (newValue)
                        {
                            // Set this position as the only true position
                            data.SetSelectedPosition(x, y);
                        }
                        else
                        {
                            // If toggling off, ensure no position is active
                            data.ClearSelection();
                        }

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