using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Animation
{
    [CreateAssetMenu(fileName = "Animation Plan", menuName = "Configs/AnimationPlan", order = 1)]
    public class AnimationPlan : ScriptableObject
    {
        public List<AnimationUnit> AnimationUnits = new List<AnimationUnit>();

        public void Refresh()
        {
            foreach (var item in AnimationUnits)
            {
                item.Refresh();
            }
        }
    }
}