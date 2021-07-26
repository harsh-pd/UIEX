
using Fordi.UI.MenuControl;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Fordi
{
    /// <summary>
    /// While adding new types, make sure not to change the order of existing types
    /// </summary>
    public enum ResourceType
    {
        ANIMATION
    }

    public enum MenuCommandType
    {
        SELECTION,
        ANIMATION
    }

    public abstract class ResourceComponent
    {
        public string Name;
        public string Description;
        public ResourceType ResourceType;
        public Sprite Preview;
        public Sprite LargePreview;
        public string SpecialCommand;
    }

    [Serializable]
    public class ExperienceResource : ResourceComponent
    {
        public static implicit operator MenuItemInfo(ExperienceResource resource)
        {
            return new MenuItemInfo
            {
                Path = resource.Name,
                Text = resource.Name,
                Command = resource.Name,
                Icon = resource.Preview,
                Data = resource,
                CommandType = MenuCommandType.SELECTION,
            };
        }
    }
}