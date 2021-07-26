using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Animation
{
    public enum AnimationTrigger
    {
        OnLoad,
        OnClick,
        After
    }

    public interface IAnimationItem
    {
        void Preview();
        void StopPreview();
    }

    [Serializable]
    public class AnimationUnit : IAnimationItem
    {
        public AnimationTrigger Trigger;
        public int ObjectHash;
        public int DelayInMs;
        public AnimationClip Animation;
        public string Parameter = "None";

        public AnimationUnit Previous = null;
        public AnimationUnit Next = null;
        public string Name = string.Empty;

        private Guid m_guid;

        public void Refresh()
        {
            if (string.IsNullOrEmpty(Name))
            {
                m_guid = Guid.NewGuid();
                Name = m_guid.ToString();
            }
        }

        public void Preview()
        {
            throw new System.NotImplementedException();
        }

        public void StopPreview()
        {
            throw new System.NotImplementedException();
        }
    }

    [Serializable]
    public class AnimationGroup : IAnimationItem
    {
        public List<IAnimationItem> m_animationItems = new List<IAnimationItem>();

        public void Preview()
        {
            throw new System.NotImplementedException();
        }

        public void StopPreview()
        {
            throw new System.NotImplementedException();
        }
    }

}