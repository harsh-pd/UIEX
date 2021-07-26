using Fordi.UI;
using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fordi.Animations
{
    public interface IAnimationEngine
    {
        void Preview();
        void StopPreview();
        void Run();
        void Stop();
    }

    public class AnimationEngine : MonoBehaviour, IAnimationEngine
    {
        [SerializeField]
        private AnimationPlan m_animationPlan;

        public AnimationPlan AnimationPlan { get { return m_animationPlan; } }

        [SerializeField]
        private AnimationPlanView m_viewPrefab;

        private AnimationPlanView m_view;

        private List<AnimationObject> m_animationObjects = new List<AnimationObject>();

        private Queue<AnimationObject> m_planQueue = new Queue<AnimationObject>();

        private IEnumerator m_animationRunner = null;

        private List<AnimationClip> m_clips = new List<AnimationClip>();

        private void Awake()
        {
            EngineObject.OnSelect += EngineItem_OnSelect;
            AnimationPlanItem.OnValueChange += AnimationPlanItem_OnValueChange;
            AnimationPlanView.OnOrderChange += AnimationPlanView_OnOrderChange;

            foreach (var item in m_animationPlan.EnteryClips)
                m_clips.Add(item);
            foreach (var item in m_animationPlan.GeneralClips)
                m_clips.Add(item);
            foreach (var item in m_animationPlan.ExitClips)
                m_clips.Add(item);
        }

       

        private void Start()
        {
            m_animationObjects = FindObjectsOfType<AnimationObject>().ToList();
            m_animationObjects = m_animationObjects.OrderBy(o => o.AnimationUnit.Order).ToList();

            m_animationPlan.AnimationUnits.Clear();
            foreach (var item in m_animationObjects)
                m_animationPlan.AnimationUnits.Add(item.AnimationUnit);

            m_planQueue.Clear();
            foreach (var item in m_animationObjects)
            {
                m_planQueue.Enqueue(item);
            }

            //Debugging purpose
            //Run();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (m_view != null)
                    CloseView();
                else
                    OpenView();
            }
        }

        private void OnDestroy()
        {
            EngineObject.OnSelect -= EngineItem_OnSelect;
            AnimationPlanItem.OnValueChange -= AnimationPlanItem_OnValueChange;
            AnimationPlanView.OnOrderChange -= AnimationPlanView_OnOrderChange;
        }

        private void OpenView()
        {
            m_view = Instantiate(m_viewPrefab, FindObjectOfType<Canvas>().transform);
            m_view.OpenMenu(new MenuArgs()
            {
                Items = ResourceToMenuItems(m_animationPlan.AnimationUnits.ToArray())
            });
        }

        private void CloseView()
        {
            if (m_view != null)
                Destroy(m_view.gameObject);
            
            m_view = null;
        }

        private void EngineItem_OnSelect(object sender, System.EventArgs e)
        {
            Debug.Log("EngineItem_OnSelect" + ((EngineObject)sender).name);
        }

        private void AnimationPlanItem_OnValueChange(object sender, ValueChangeArgs e)
        {
            var animationItem = (AnimationPlanItem)sender;
            var unit = animationItem.AnimationUnit;
            var animObject = m_animationObjects.Find(item => item.AnimationUnit.UnitGuid == unit.UnitGuid);

            //Temporary work. Must use reflection to implement this

            switch (e.FieldName)
            {
                case "Trigger":
                    animObject.AnimationUnit.Trigger = (AnimationTrigger)e.Value;
                    break;
                case "DelayInMs":
                    animObject.AnimationUnit.DelayInMs = (int)e.Value;
                    break;
                case "Animation":
                    var clipName = (string)e.Value;
                    animObject.AnimationUnit.Animation = m_clips.Find(clip => clip.name == clipName);
                    break;
                case "Parameter":
                    break;
                default:
                    break;
            }

            unit.Refresh();
        }

        private void AnimationPlanView_OnOrderChange(object sender, OrderChangeArgs e)
        {
            var animationItem = (AnimationPlanItem)sender;
            var unit = animationItem.AnimationUnit;
            var animObject = m_animationObjects.Find(item => item.AnimationUnit.UnitGuid == unit.UnitGuid);

        }

        private IEnumerator AnimationRunner()
        {
            while (m_planQueue.Count > 0)
            {
                var current = m_planQueue.Dequeue();
                yield return new WaitForSeconds(current.AnimationUnit.DelayInMs / 1000f);
                current.Run();
                yield return new WaitUntil(() => current.IsDone);
            }
        }

        public void Preview()
        {
            throw new System.NotImplementedException();
        }

        public void Run()
        {
            if (m_animationRunner != null)
                StopCoroutine(m_animationRunner);

            foreach (var item in m_animationObjects)
                item.Stop();

            m_animationRunner = AnimationRunner();
            StartCoroutine(m_animationRunner);
        }

        public void Stop()
        {
            if (m_animationRunner != null)
                StopCoroutine(m_animationRunner);

            foreach (var item in m_animationObjects)
                item.Stop();
        }

        public void StopPreview()
        {
            throw new System.NotImplementedException();
        }

        public static MenuItemInfo[] ResourceToMenuItems(ExperienceResource[] resources)
        {
            MenuItemInfo[] menuItems = new MenuItemInfo[resources.Length];
            for (int i = 0; i < resources.Length; i++)
                menuItems[i] = resources[i];
            return menuItems;
        }
    }
}