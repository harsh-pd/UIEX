using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fordi.UI.MenuControl
{
    public interface IScreen
    {
        void Reopen();
        void Deactivate();
        void Close();
        bool Blocked { get; }
        bool Persist { get; }
        void ShowPreview(Sprite sprite);
        void ShowTooltip(string tooltip);
        void Hide();
        void UnHide();
        GameObject Gameobject { get; }
        IScreen Pair { get; }
        void DisplayResult(Error error);
        void DisplayProgress(string text);
    }

    public class MenuArgs
    {
        public MenuItemInfo[] Items = new MenuItemInfo[] { };
        public AudioClip AudioClip = null;
        public Action<string[]> OnAction = null;
        public string Title = "";
        public bool Block = false;
        public bool BackEnabled = true;
        public bool Persist = true;
        public Vector2? Position = Vector2.zero;
        public object Data = null;
    }

    [DisallowMultipleComponent]
    public class MenuScreen : MonoBehaviour, IScreen
    {
        [SerializeField]
        protected Transform m_contentRoot;

        [SerializeField]
        protected GameObject m_menuItem;

        [SerializeField]
        protected TextMeshProUGUI m_title;

        [SerializeField]
        protected Button m_closeButton, m_okButton;

        [SerializeField]
        private GameObject m_backButton, m_refreshButton;

        [SerializeField]
        protected Image m_preview;

        [SerializeField]
        protected TextMeshProUGUI m_description;

        [SerializeField]
        protected GameObject m_loader = null;

        [SerializeField]
        protected GameObject m_standaloneMenu = null;

        [SerializeField]
        protected Blocker m_blocker;
 
        public bool Blocked { get; protected set; }

        public bool Persist { get; protected set; }

        public GameObject Gameobject { get { return gameObject; } }

        private IScreen m_pair = null;
        public IScreen Pair { get { return m_pair; } set { m_pair = value; } }

        private string m_refreshCategory = null;

        private Vector3 m_localScale = Vector3.zero;

        protected List<IMenuItem> m_menuItems = new List<IMenuItem>();

        public static EventHandler ExternalChangesDone = null;
        protected Action<string[]> m_onAction = null;

        void Awake()
        {

            if (m_localScale == Vector3.zero)
                m_localScale = transform.localScale;

            AwakeOverride();
        }

        private void OnDestroy()
        {
            OnDestroyOverride();
        }

        protected virtual void AwakeOverride()
        {

        }

        protected virtual void OnDestroyOverride()
        {

        }

        public virtual void Init(bool block, bool persist)
        {
            Blocked = block;
            Persist = persist;
        }

        public virtual void Deactivate()
        {
            if (m_loader != null)
                m_loader.SetActive(false);
            if (m_blocker != null)
                m_blocker.gameObject.SetActive(false);
            if (m_description != null)
                m_description.text = "";
            gameObject.SetActive(false);
            if (Pair != null)
                Pair.Deactivate();
        }

        public virtual void Reopen()
        {
            gameObject.SetActive(true);
            if (m_preview != null)
                m_preview.gameObject.SetActive(m_preview.sprite != null);

            if (m_refreshCategory != null)
                Refresh();

            if (Pair != null)
                Pair.Reopen();
        }

        private void Refresh() { }

        protected MenuItemInfo[] ResourceToMenuItems(ExperienceResource[] resources)
        {
            MenuItemInfo[] menuItems = new MenuItemInfo[resources.Length];
            for (int i = 0; i < resources.Length; i++)
                menuItems[i] = resources[i];
            return menuItems;
        }


        public virtual IMenuItem SpawnMenuItem(MenuItemInfo menuItemInfo, GameObject prefab, Transform parent)
        {
            IMenuItem menuItem = Instantiate(prefab, parent, false).GetComponentInChildren<IMenuItem>();
            //menuItem.name = "MenuItem";
            menuItem.DataBind(menuItemInfo);
            m_menuItems.Add(menuItem);
            return menuItem;
        }

        public virtual void Clear()
        {
            if (m_contentRoot == null)
                return;

            foreach (Transform child in m_contentRoot)
            {
                Destroy(child.gameObject);
            }
            m_contentRoot.DetachChildren();
        }

        public virtual void OpenMenu(MenuArgs args)
        {
            m_menuItems.Clear();
            Blocked = args.Block;
            Persist = args.Persist;
            m_onAction = args.OnAction;
            gameObject.SetActive(true);

            Populate(args.Items);
        }

        protected virtual void Populate(MenuItemInfo[] items)
        {
            Clear();
            foreach (var item in items)
                SpawnMenuItem(item, m_menuItem, m_contentRoot);
        }

        public virtual void OpenMenu(string text, bool blocked, bool persist)
        {
            Clear();
            Blocked = blocked;
            Persist = persist;
            gameObject.SetActive(true);
            m_description.text = text;
        }

        public virtual void Close()
        {
            if (Pair != null)
                Pair.Close();
            if (gameObject != null)
                Destroy(gameObject);
        }

        public void CloseAllScreen()
        {
            Destroy(gameObject);
        }

        public virtual void BackClick()
        {
            
        }

        public void ShowPreview(Sprite sprite)
        {
            if (m_preview == null)
                return;
            m_preview.gameObject.SetActive(sprite != null);
            m_preview.sprite = sprite;

            if (Pair != null)
                Pair.ShowPreview(sprite);
        }

        public void ShowTooltip(string tooltip)
        {
            if (m_description)
                m_description.text = tooltip;
            if (Pair != null)
                Pair.ShowTooltip(tooltip);
        }

        public void Hide()
        {
            if (m_localScale == Vector3.zero)
                m_localScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void UnHide()
        {
            transform.localScale = m_localScale;
        }

        public virtual void DisplayResult(Error error)
        {
            if (m_preview != null && m_preview.sprite != null)
                m_preview.gameObject.SetActive(true);

            if (m_loader)
                m_loader.SetActive(false);
            if (m_blocker)
                m_blocker.gameObject.SetActive(false);

            m_description.text = error.ErrorText;
        }

        public virtual void DisplayProgress(string text)
        {
            if (m_preview)
                m_preview.gameObject.SetActive(false);
            if (m_loader == null)
            {
                Debug.LogError(this.name);
                return;
            }
            //Debug.LogError("Loadr activating: " + name);
            m_loader.SetActive(true);
            m_blocker.gameObject.SetActive(true);
            m_description.text = text;
        }

        public void Quit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");
            #else
                Application.Quit();
            #endif
        }

    }
}