using UnityEngine;
using UnityEditor;
namespace Assets.Scripts.Element
{
    [CustomEditor(typeof(MapManager))]
    public class MapManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MapManager mapnamager = (MapManager)target;
            if (GUILayout.Button("MapInit"))
            {
                mapnamager.Mapinit();
            }
        }
    }
}