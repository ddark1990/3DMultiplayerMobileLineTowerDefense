#pragma warning disable CS0649
using UnityEngine;
using UnityEditor;
namespace JoelQ.GameSystem.Tower {
    public class TowerManagerEditor : EditorWindow {

        public static IntPopPair towers;
        public static IntPopPair projectiles;
        [SerializeField] private TowerPoolList towerList;
        [SerializeField] private TowerProjectilePoolList projectileList;

        [MenuItem("JoelQ/Tower Data Customization")]
        private static void Init() {
            GetWindow<TowerManagerEditor>("Tower Data Customization", true);
        }

        private void OnGUI() {
            SerializedObject so = new SerializedObject(this);
            EditorGUILayout.LabelField("Place AI Datas into this array to make it available for creation.");
            EditorGUILayout.PropertyField(so.FindProperty("towerList"), true);
            EditorGUILayout.PropertyField(so.FindProperty("projectileList"), true);
            so.ApplyModifiedProperties();
            so.Update();
            if (towerList != null) {
                towers = new IntPopPair(towerList.Towers.Length);
                for (int i = 0; i < towerList.Towers.Length; i++) {
                    towers.names[i] = towerList.Towers[i].Data.Name;
                    towers.indices[i] = i;
                }
            }
            if (projectileList != null) {
                projectiles = new IntPopPair(projectileList.Projectiles.Length);
                for (int i = 0; i < projectileList.Projectiles.Length; i++) {
                    projectiles.names[i] = projectileList.Projectiles[i].TowerProjectile.name;
                    projectiles.indices[i] = i;
                }
            }
        }

        public struct IntPopPair {
            public string[] names;
            public int[] indices;

            public IntPopPair(int length) {
                names = new string[length];
                indices = new int[length];
            }
        }
    }
}