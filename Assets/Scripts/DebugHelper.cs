#if UNITY_EDITOR
using MicroJam10.Player;
using UnityEditor;
using UnityEngine;

namespace MicroJam10.City
{
    public class DebugHelper : EditorWindow
    {
        [MenuItem("Window/Debug Helper")]
        public static void ShowWindow()
        {
            GetWindow(typeof(DebugHelper));
        }

        public void OnGUI()
        {
            var target = FindObjectOfType<PlayerController>().GetInteractionTarget(1 << LayerMask.NameToLayer("Map") | 1 << LayerMask.NameToLayer("Prop"));
            GUILayout.Label($"Placement Interaction Target: {(target == null ? "None" : target.Value.collider?.name)}");
            target = FindObjectOfType<PlayerController>().GetInteractionTarget(1 << LayerMask.NameToLayer("Map") | 1 << LayerMask.NameToLayer("Spot"));
            GUILayout.Label($"Placement Spot Target: {(target == null ? "None" : target.Value.collider?.name)}");
            GUILayout.Label("Hover window to refresh", new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Italic,
                fontSize = 11
            });
        }
    }
}
#endif