using Fordi.Animations;
using UnityEditor;
using UnityEngine;

namespace Fordi.Editors
{

    [CustomEditor(typeof(AnimationPlan))]
    public class GameDataEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            AnimationPlan plan = (AnimationPlan)target;

            if (GUILayout.Button("Refresh"))
            {
                plan.Refresh();
            }
        }
    }
}