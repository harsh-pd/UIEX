using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fordi.UI.MenuControl
{
    public delegate void MenuItemEventHandler(MenuItem menuItem);

    public interface IMenuItem
    {
        MenuItemInfo Item { get; }
        Selectable Selectable { get; }
        void DataBind(MenuItemInfo item);
        GameObject Gameobject { get; }
    }

    public class MenuItem : MonoBehaviour, IMenuItem
    {
        [SerializeField]
        protected Image m_icon = null;
        [SerializeField]
        protected bool m_allowTextScroll = false;
        [SerializeField]
        private float m_textScrollSpeed = 10.0f;
        [SerializeField]
        private TextMeshProUGUI m_text;


        protected TextMeshProUGUI m_clonedText;

        private Selectable m_selectable;

        public Selectable Selectable
        {
            get
            {
                if (m_selectable == null)
                {
                    m_selectable = GetComponent<Selectable>();
                }
                return m_selectable;
            }
        }

        private bool m_textScrollInitialized = false;

        protected IEnumerator m_textScrollEnumerator;

        protected Vector3 m_initialTextPosition = Vector3.zero;

        protected MenuItemInfo m_item;
        public MenuItemInfo Item
        {
            get { return m_item; }
            //set
            //{
            //    if(m_item != value)
            //    {
            //        m_item = value;
            //        DataBind();
            //    }
            //}
        }

        public GameObject Gameobject { get { return gameObject; } }

        public virtual void DataBind(MenuItemInfo item)
        {
            m_item = item;

            if(m_item != null)
            {
                m_icon.sprite = m_item.Icon;
                m_text.text = m_item.Text;
                m_icon.gameObject.SetActive(m_item.Icon != null);
            }
            else
            {
                m_icon.sprite = null;
                m_icon.gameObject.SetActive(false);
                m_text.text = string.Empty;
            }

            if (m_item.Validate == null)
                m_item.Validate = new MenuItemValidationEvent();


            m_item.Validate.AddListener(DefaultCanExecuteMenuCommand);
            m_item.Validate.AddListener((args) => args.IsValid = m_item.IsValid);

            var validationResult = IsValid();
            if(validationResult.IsVisible)
            {
                if (!m_item.IsValid)
                {
                    m_text.color = Color.white;
                }

                if (m_item.Action == null)
                    m_item.Action = new MenuItemEvent();
                m_item.Action.AddListener(item.OnExecute);

                if (Selectable != null)
                    ((Button)Selectable).onClick.AddListener(() => m_item.Action.Invoke(new MenuClickArgs(m_item.Path, m_item.Text, m_item.Command, m_item.CommandType, m_item.Data)));

            }

            gameObject.SetActive(validationResult.IsVisible);
            m_selectable.interactable = validationResult.IsValid;
        }

        public void DefaultCanExecuteMenuCommand(MenuItemValidationArgs args)
        {
            args.IsValid = true;
        }

        protected bool m_textOverflowing = false;

        public virtual void DataBind(MenuItemInfo item, object sender)
        {
            m_item = item;

            if (m_item != null)
            {
                m_icon.sprite = m_item.Icon;
                m_text.text = m_item.Text;
                m_icon.gameObject.SetActive(m_item.Icon != null);
            }
            else
            {
                m_icon.sprite = null;
                m_icon.gameObject.SetActive(false);
                m_text.text = string.Empty;
            }

            if (m_item.Validate == null)
                m_item.Validate = new MenuItemValidationEvent();


            m_item.Validate.AddListener(DefaultCanExecuteMenuCommand);
            m_item.Validate.AddListener((args) => args.IsValid = m_item.IsValid);

            var validationResult = IsValid();
            if (validationResult.IsVisible)
            {
                if (!m_item.IsValid)
                {
                    m_text.color = Color.white;
                }

                if (m_item.Action == null)
                    m_item.Action = new MenuItemEvent();
                m_item.Action.AddListener(item.OnExecute);

                if (Selectable != null)
                    ((Button)Selectable).onClick.AddListener(() => m_item.Action.Invoke(new MenuClickArgs(m_item.Path, m_item.Text, m_item.Command, m_item.CommandType, m_item.Data)));

            }

            gameObject.SetActive(validationResult.IsVisible);
            m_selectable.interactable = validationResult.IsValid;
        }

        protected MenuItemValidationArgs IsValid()
        {
            if(m_item == null)
            {
                return new MenuItemValidationArgs(m_item.Command) { IsValid = false, IsVisible = false };
            }

            if(m_item.Validate == null)
            {
                return new MenuItemValidationArgs(m_item.Command) { IsValid = true, IsVisible = true };
            }

            MenuItemValidationArgs args = new MenuItemValidationArgs(m_item.Command);
            m_item.Validate.Invoke(args);
            return args;
        }
    }
}

