using Fordi.Animations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Fordi.UI.MenuControl
{
    public class OrderChangeArgs
    {
        public Dictionary<string, AnimationUnit> AnimationUnits = new Dictionary<string, AnimationUnit>();
    }

    public class AnimationPlanView : MenuScreen
    {
        public static event EventHandler<OrderChangeArgs> OnOrderChange;

        private ReorderableList m_reorderableList;

        private void Awake()
        {
            m_reorderableList = GetComponentInChildren<ReorderableList>();
            m_reorderableList.OnElementDropped.AddListener(OnElementDropped);
        }

        private void OnElementDropped(ReorderableList.ReorderableListEventStruct arg0)
        {
            StartCoroutine(ItemDroopped());
        }

        private IEnumerator ItemDroopped()
        {
            yield return new WaitForSeconds(.1f);
            Dictionary<string, AnimationUnit> animationUnits = new Dictionary<string, AnimationUnit>();


            var planItems = GetComponentsInChildren<AnimationPlanItem>();

            foreach (var item in planItems)
            {
                item.AnimationUnit.Order = item.transform.GetSiblingIndex();
                item.RefreshTrigger();
                animationUnits[item.AnimationUnit.UnitGuid] = item.AnimationUnit;
            }

            //Temp. Must initialise the values of OrderChangeArgs
            OnOrderChange?.Invoke(this, new OrderChangeArgs()
            {
                AnimationUnits = animationUnits
            });
        }
    }
}