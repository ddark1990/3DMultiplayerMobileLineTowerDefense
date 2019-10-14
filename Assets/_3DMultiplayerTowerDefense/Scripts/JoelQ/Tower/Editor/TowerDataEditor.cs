using UnityEngine;
using UnityEditor;
namespace JoelQ.GameSystem.Tower {

    [CustomEditor(typeof(TowerData))]
    public class TowerDataEditor : Editor {

        public override void OnInspectorGUI() {

            TowerData target = serializedObject.targetObject as TowerData;
            DrawDefaultInspector();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}