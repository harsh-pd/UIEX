using Fordi.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fordi.UI.MenuControl
{
    public class OrderChangeArgs
    {
        public Guid Unit1;
        public Guid Unit2;
    }
    public class AnimationPlanView : MenuScreen
    {
        public static event EventHandler<OrderChangeArgs> OnOrderChange;
    }
}