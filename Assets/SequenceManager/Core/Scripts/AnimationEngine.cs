using Fordi.UI.MenuControl;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fordi.Animations
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
        [SerializeField]
        private AnimationPlan m_animationPlan;

        [SerializeField]
        private AnimationPlanView m_viewPrefab;

        private AnimationPlanView m_view;

        private List<AnimationObject> m_animationObjects = new List<AnimationObject>();

        private Queue<AnimationObject> m_planQueue = new Queue<AnimationObject>();

        private IEnumerator m_animationRunner = null;

        private void Awake()
        {
            EngineObject.OnSelect += EngineItem_OnSelect;
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
            Run();
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
        }

        private void OpenView()
        {
            m_view = Instantiate(m_viewPrefab, FindObjectOfType<Canvas>().transform);
        }

        private void CloseView()
        {
            if (m_view != null)
                Destroy(m_view.gameObject);
            
            m_view = null;
        }

        private void EngineItem_OnSelect(object sender, System.EventArgs e)
        {
            Debug.LogError(((EngineObject)sender).name);
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

        public void PPreview()
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
            throw new System.NotImplementedException();
        }

        public void StopPreview()
        {
            throw new System.NotImplementedException();
        }
    }
}