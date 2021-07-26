using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Animations
{

    public interface IAnimationObject
    {
        
    }

    [RequireComponent(typeof(Animation))]
    public class AnimationObject : MonoBehaviour, IAnimationObject
    {
        [SerializeField]
        private AnimationUnit m_animationUnit;
        public AnimationUnit AnimationUnit { get { return m_animationUnit; } }

        public bool IsDone { get { return m_animation.IsPlaying(m_animationUnit.Animation.name); } }

        private Animation m_animation = null;
        private Renderer m_renderer = null;

        private void Awake()
        {
            m_animationUnit.ObjectHash = gameObject.GetHashCode();
            m_animationUnit.AssetName = name;
            m_animationUnit.Refresh();
            m_renderer = GetComponent<Renderer>();
            m_renderer.enabled = !m_animationUnit.DisabledByDefault;
            m_animation = GetComponent<Animation>();
        }

        public void Run()
        {
            m_renderer.enabled = true;
            m_animation.AddClip(m_animationUnit.Animation, m_animationUnit.Animation.name);
            m_animation.clip = m_animationUnit.Animation;
            m_animation.Play();
            Debug.LogError("Playing: " + m_animation.clip.name + " on: " + m_animation.name);
        }

        public void Stop()
        {
            m_animation.Stop();
        }
    }
}