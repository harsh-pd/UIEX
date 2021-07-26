using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fordi
{
    public interface IEngineObject
    {
    }

    public class EngineObject : MonoBehaviour, IEngineObject
    {
        public static event EventHandler OnSelect;


        public void OnMouseDown()
        {
            OnSelect?.Invoke(this, EventArgs.Empty);
        }
    }
}