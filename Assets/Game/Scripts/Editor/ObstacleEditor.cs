using Game.Scripts.Obstacle;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Editor
{
    /// <summary>
    /// ObstacleEditor class is used to create a custom editor for the "ObstaclesData" ScriptableObject.
    /// Visually the custom editor looks like the grid and can be used to edit if the tile corresponding to that
    /// grid location should have an obstacle or not. 
    /// </summary>
    [CustomEditor(typeof(ObstacleData))]
    public class ObstacleEditor : UnityEditor.Editor
    {
        // Used for overriding the default GUI, so that the new grid editor can be displayed.
        public override void OnInspectorGUI()
        {
            ObstacleData data = (ObstacleData)target;

            EditorGUILayout.LabelField("Obstacle Grid Editor", EditorStyles.boldLabel);

            for (int x = 0; x < 10; x++)
            {
                // Starts a horizontal row for the current row of grid toggles.
                EditorGUILayout.BeginHorizontal();

                for (int y = 0; y < 10; y++)
                {
                    bool currentValue = data.GetObstacleAt(x, y);

                    // Displays the toggle button for the cell, and gets the updated value if the user changes the value.
                    bool newValue = GUILayout.Toggle(currentValue, "", GUILayout.Width(20), GUILayout.Height(20));

                    if (newValue != currentValue)
                    {
                        data.SetObstacleAt(x, y, newValue);

                        // Marks the scriptable object as dirty so that changes can presist.
                        EditorUtility.SetDirty(data);
                    }
                }

                //  Ends the Horizontal row started above.
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                AssetDatabase.SaveAssets();
            }
        }
    }
}