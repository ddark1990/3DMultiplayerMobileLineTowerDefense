using UnityEditor;
namespace JoelQ.GameSystem.Tower {
    [CustomEditor(typeof(TowerData))]
    public class TowerDataEditor : Editor {

        public override void OnInspectorGUI() {
            
            DrawDefaultInspector();
            SerializedProperty projectile = serializedObject.FindProperty("projectile");
            projectile.intValue = EditorGUILayout.IntPopup(projectile.intValue, TowerManagerEditor.projectiles.names, TowerManagerEditor.projectiles.indices);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}