using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.Animation
{
    public interface IAnimationEngine
    {
        void PPreview();
        void StopPreview();
        void Run();
        void Stop();
    }

    public class AnimationEngine : MonoBehaviour, IAnimationEngine
    {
        private void Start()
        {
        }

        public void PPreview()
        {
            throw new System.NotImplementedException();
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void StopPreview()
        {
            throw new System.NotImplementedException();
        }
    }
}