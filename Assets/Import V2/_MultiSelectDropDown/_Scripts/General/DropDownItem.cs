using System;
using UnityEngine;

[Serializable]
public class DropDownItem
{
    [SerializeField] private string m_Caption;

    /// <summary>
    /// Caption of the Item
    /// </summary>
    public string caption
    {
        get { return m_Caption; }
        set
        {
            m_Caption = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    [SerializeField] private Sprite m_Image;

    /// <summary>
    /// Image component of the Item
    /// </summary>
    public Sprite image
    {
        get { return m_Image; }
        set
        {
            m_Image = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    [SerializeField] private bool m_IsDisabled;

    /// <summary>
    /// Is the Item currently enabled?
    /// </summary>
    public bool isDisabled
    {
        get { return m_IsDisabled; }
        set
        {
            m_IsDisabled = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    [SerializeField] private string m_Id;

    ///<summary>
    /// This Id is a way to have similar items that you can easily identify. You can leave this field empty if you don't need to identify your items furthermore
    ///</summary>
    public string id
    {
        get { return m_Id; }
        set { m_Id = value; }
    }

    public Action OnSelect = null; //Actions on item selection

    internal Action OnUpdate = null; //Actions on item Enabled/Disabled.  

    /// <summary>
    /// Panel Item Constructor
    /// </summary>
    /// <param name="caption">The text to be displayed</param>
    /// <param name="newId">Unique Identifier</param>
    /// <param name="image">The sprite to be displayed</param>
    /// <param name="disabled">Should the item start disabled</param>
    /// <param name="onSelect">Actions on item selection</param>
    /// <param name="onUpdate">Actions on item update</param>
    public DropDownItem(string caption = "", string newId = "", Sprite image = null, bool disabled = false, Action onSelect = null, Action onUpdate = null)
    {
        m_Caption = caption;
        m_Image = image;
        m_Id = newId;
        m_IsDisabled = disabled;
        OnSelect = onSelect;
        OnUpdate = onUpdate;
    }
}