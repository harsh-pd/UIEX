using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Animations
{
    [CreateAssetMenu(fileName = "Animation Plan", menuName = "Configs/AnimationPlan", order = 1)]
    public class AnimationPlan : ScriptableObject
    {
        public List<AnimationUnit> AnimationUnits = new List<AnimationUnit>();

        [Header("AVAILABLE CLIPS")]
        public List<AnimationClip> EnteryClips = new List<AnimationClip>();
        public List<AnimationClip> GeneralClips = new List<AnimationClip>();
        public List<AnimationClip> ExitClips = new List<AnimationClip>();

        public void Refresh()
        {
            foreach (var item in AnimationUnits)
            {
                item.Refresh();
            }
        }
    }
}