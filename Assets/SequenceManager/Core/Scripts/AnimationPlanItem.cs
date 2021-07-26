using Fordi.Animations;
using Fordi.UI.MenuControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fordi.UI
{
    public class AnimationPlanItem : MonoBehaviour, IMenuItem
    {
        [SerializeField]
        private TMP_Dropdown m_trigger;

        [SerializeField]
        private TextMeshProUGUI m_assetName;

        [SerializeField]
        private TMP_InputField m_delay;

        [SerializeField]
        private TMP_Dropdown m_animation;

        [SerializeField]
        private TMP_InputField m_parameter;

        private MenuItemInfo m_menuItem;
        public MenuItemInfo Item
        {
            get
            {
                return m_menuItem;
            }
        }
        private Selectable m_selectable;
        public Selectable Selectable
        {
            get
            {
                if (m_selectable == null)
                    m_selectable =  GetComponent<Selectable>();
                return m_selectable;
            }
        }

        public GameObject Gameobject { get { return gameObject; } }

        private AnimationUnit m_animationUnit;

        private AnimationEngine m_animationEngine;

        private List<string> m_clipNames = new List<string>();

        public void DataBind(MenuItemInfo item)
        {
            m_menuItem = item;
            m_animationUnit = (AnimationUnit)item.Data;
            m_trigger.options.Clear();
            m_trigger.AddOptions(Enum.GetNames(typeof(AnimationTrigger)).ToList());
            m_trigger.value = (int)m_animationUnit.Trigger;

            m_assetName.text = m_animationUnit.AssetName;
            m_delay.text = m_animationUnit.DelayInMs.ToString();

            m_animation.options.Clear();

            //Temporary. Must use proper dependency injection
            m_animationEngine = FindObjectOfType<AnimationEngine>();
            
            foreach (var clip in m_animationEngine.AnimationPlan.EnteryClips)
            {
                m_clipNames.Add(clip.name);
            }

            foreach (var clip in m_animationEngine.AnimationPlan.GeneralClips)
            {
                m_clipNames.Add(clip.name);
            }

            foreach (var clip in m_animationEngine.AnimationPlan.ExitClips)
            {
                m_clipNames.Add(clip.name);
            }
            m_animation.AddOptions(m_clipNames);

            m_animation.value = m_clipNames.FindIndex(val => val == m_animationUnit.Animation.name);

            m_parameter.text = "None";
        }
    }
}