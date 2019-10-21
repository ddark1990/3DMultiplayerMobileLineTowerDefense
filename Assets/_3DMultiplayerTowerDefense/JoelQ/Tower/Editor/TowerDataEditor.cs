using UnityEditor;
namespace JoelQ.GameSystem.Tower {
    [CustomEditor(typeof(TowerData))]
    public class TowerDataEditor : Editor {

        //private TowerManagerData tmd;
        //private string[] names;
        //private int[] indices;

        public override void OnInspectorGUI() {

            //tmd = AssetDatabase.LoadAssetAtPath<TowerManagerData>("Assets");
            //names = new string[tmd.Projectiles.Length];
            //indices = new int[tmd.Projectiles.Length];
            //for(int i = 0; i < tmd.Projectiles.Length; i++) {
            //    names[i] = tmd.Projectiles[i].TowerProjectile.name;
            //    indices[i] = i;
            //}
            DrawDefaultInspector();
            //SerializedProperty projectile = serializedObject.FindProperty("projectile");
            //projectile.intValue = EditorGUILayout.IntPopup(projectile.intValue, names, indices);
            //serializedObject.ApplyModifiedProperties();
            //serializedObject.Update();
        }
    }
}