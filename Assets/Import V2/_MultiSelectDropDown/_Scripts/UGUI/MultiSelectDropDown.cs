using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("UI/MultiSelectDropDown")]
public class MultiSelectDropDown : MonoBehaviour
{
    // All the variables seen in the inspector

    #region Displayed Variables

    [Header("User Experience")] [SerializeField]
    private bool m_AutoUpdateContentSize;

    [SerializeField] bool
        m_AutoOpenPanel = false,
        m_ClosePanelOnSelection = false,
        m_AllowMultiSelect = true,
        m_DisplayMainButtonImage = true,
        m_HasSelectionLimit = true,
        m_AutoSelectIndex = false;

    [SerializeField] private int m_MaxSelectedElements = 5, m_SpecificStartIndex = 0;

    [Header("Formatting")]
    // If an option is disabled, it's text will have this color instead
    [SerializeField]
    private Color m_DisabledTextColor;

    [SerializeField] private Color m_DefaultTextColor;
    [SerializeField] private string m_MultipleSelectedItemsCaption = "Multiple Elements";

    [SerializeField] private string m_NoSelectedItemsCaption = "Please select an item";


    [SerializeField] private int m_ItemsToDisplay = 5;
    [SerializeField] private float m_ScrollBarWidth = 20.0f;

    [Header("UI Elements")] [SerializeField]
    private DropDownButton_TMP m_MainButton;

    [SerializeField] private RectTransform m_OverlayRT, m_ScrollPanelRT, m_ScrollBarRT, m_SlidingAreaRT, m_ItemsPanelRT;

    [Header("Data")] [SerializeField] private List<DropDownItem> m_Items;

    #endregion

    // Object Pooling Settings

    #region Object Pooling

    // This is my personal ComponentPool System, it's easy it use, define it with any Component Type, set the size and prefab in the inspector
    // Then use Rent() to get an item, Return(item) to return this item and Reset() to Return all the items.

    [Header("Object Pooling")] [SerializeField]
    private ObjectPool<DropDownButton> m_ItemsPool;

    [SerializeField] private GameObject m_Template;
    [SerializeField] private int m_PoolSize;

    #endregion

    // All the variables which do not appear in the inspector

    #region Hidden Variables

    private bool m_IsPanelActive = false;
    private bool m_HasDrawnOnce = false;
    private Canvas m_Canvas;
    private RectTransform m_RectTransform, m_CanvasRT;
    private ScrollRect m_ScrollRect;
    private List<DropDownButton> m_PanelItems;
    private List<int> m_SelectedIndexes = new List<int>();

    #endregion

    // Helper functions and properties

    #region Helpers and Properties

    /// <summary>
    /// Get the currently selected items
    /// </summary>
    public DropDownItem[] selectedItems =>
        m_SelectedIndexes.Count > 0 && m_Items.Count > 0 && m_Items.Any(x => m_SelectedIndexes.Contains(m_Items.IndexOf(x)))
            ? m_Items.Where(x => m_SelectedIndexes.Contains(m_Items.IndexOf(x))).ToArray()
            : null;


    public float scrollBarWidth
    {
        get { return m_ScrollBarWidth; }
        set
        {
            m_ScrollBarWidth = value;
            RedrawPanel();
        }
    }

    public int itemsToDisplay
    {
        get { return m_ItemsToDisplay; }
        set
        {
            m_ItemsToDisplay = value;
            RedrawPanel();
        }
    }

    #endregion

    // All the Events Classes for callbacks are here

    #region Events

    // You can pass on any kind of object, I made four classes but you can make as many as you want depending on your needs

    [System.Serializable]
    public class SelectionChangedEvent : UnityEngine.Events.UnityEvent<object>
    {
    }

    [System.Serializable]
    public class ElementAddedEvent : UnityEngine.Events.UnityEvent<object>
    {
    }

    [System.Serializable]
    public class ElementRemovedEvent : UnityEngine.Events.UnityEvent<object>
    {
    }

    [System.Serializable]
    public class OnEmptyEvents : UnityEngine.Events.UnityEvent<object>
    {
    }

    #endregion

    // All the Events set to be Fired

    #region Callbacks

    // If you're note familiar with this, add any of the events in your variables and register functions in them.
    // Then call it with MyEvent?.Invoke(your params), the ? in the middle is a nullcheck to see if anyone has registered to the event.
    // "your params" can be any kind of object, or whatever parameters you've set in the events you created

    [Header("Callbacks")] public SelectionChangedEvent OnSelectionChanged; // Broadcast Event when item is changed;
    public ElementAddedEvent OnElementAdded; // Broadcast Event when a new item is added to the selected items list;
    public ElementRemovedEvent OnElementRemoved; // Broadcast Event when an item is removed from the selected items list;
    public OnEmptyEvents OnListEmptied; // Broadcast Event when the count of selected items drops from (> 0) to 0;

    #endregion

    #region Demo

    //Setup is called in start for demo purpose but I recommend you call it whenever you need instead.
    public void Start()
    {
        Setup();
    }

    #endregion

    #region Core

    /// <summary>
    /// Start initiation process and updated UI components if needed.
    /// </summary>
    public void Setup()
    {
        var initializationCompleted = Initialize();
        if (!initializationCompleted)
        {
            return;
        }

        if (m_AutoSelectIndex && m_Items.Count > 0)
        {
            OnItemClickedWithoutNotify(m_SpecificStartIndex);
        }

        if (m_AutoOpenPanel)
        {
            ToggleDropdownPanel();
        }
    }

    /// <summary>
    /// Initialize the Drop Down list assignations.
    /// </summary>
    /// <returns>Returns whether it succeeded or failed.</returns>
    private bool Initialize()
    {
        try
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_OverlayRT.gameObject.SetActive(false);
            m_Canvas = GetComponentInParent<Canvas>();
            m_CanvasRT = m_Canvas.GetComponent<RectTransform>();
            m_ScrollRect = m_ScrollPanelRT.GetComponent<ScrollRect>();
            m_ScrollRect.scrollSensitivity = m_RectTransform.sizeDelta.y / 2;
            m_ScrollRect.movementType = ScrollRect.MovementType.Clamped;
            m_ScrollRect.content = m_ItemsPanelRT;
            m_ItemsPool = new ObjectPool<DropDownButton>(m_PoolSize, m_Template);
            m_MainButton.text.text = m_NoSelectedItemsCaption;
        }
        catch (System.NullReferenceException ex)
        {
            Debug.LogException(ex);
            Debug.LogError("Something went wrong during the setup of object " + name + ". Returned false at init");
            return false;
        }

        m_PanelItems = new List<DropDownButton>();

        RebuildPanel();
        RedrawPanel();
        return true;
    }

    /// <summary>
    /// Add a collection of items to the list, can be either a DropDownItem, a string or a sprite.
    /// </summary>
    public void AddItems(IEnumerable<object> list)
    {
        var ddItems = new List<DropDownItem>();
        foreach (var obj in list)
        {
            if (obj is DropDownItem)
            {
                ddItems.Add((DropDownItem) obj);
            }
            else if (obj is string)
            {
                ddItems.Add(new DropDownItem(caption: (string) obj));
            }
            else if (obj is Sprite)
            {
                ddItems.Add(new DropDownItem(image: (Sprite) obj));
            }
            else
            {
                throw new System.Exception("Only ComboBoxItems, Strings, and Sprite types are allowed");
            }
        }

        m_Items.AddRange(ddItems);
        m_Items = m_Items.Distinct().ToList(); //remove any duplicates
        RebuildPanel();
    }

    /// <summary>
    /// Add an item to the list, can be either a DropDownItem, a string or a sprite.
    /// </summary>
    public void AddItem(object obj)
    {
        if (obj is DropDownItem)
        {
            m_Items.Add((DropDownItem) obj);
        }
        else if (obj is string)
        {
            m_Items.Add(new DropDownItem(caption: (string) obj));
        }
        else if (obj is Sprite)
        {
            m_Items.Add(new DropDownItem(image: (Sprite) obj));
        }
        else
        {
            throw new System.Exception("Only DropDownItems, Strings, and Sprite types are allowed");
        }

        m_Items = m_Items.Distinct().ToList(); //remove any duplicates
        RebuildPanel();
    }

    /// <summary>
    /// When an item is clicked, update the data containers and the UI accordingly
    /// </summary>
    /// <param name="indx">Index of the clicked item</param>
    private void OnItemClicked(int indx)
    {
        if (m_Items[indx].isDisabled)
            return;
        var currentCount = selectedItems?.Length ?? 0;
        if (!m_AllowMultiSelect)
        {
            if (m_SelectedIndexes.Count > 0)
            {
                // Currently event is fired with an integer, but you can change this for any kind of object
                OnElementRemoved?.Invoke(m_SelectedIndexes[0]);
                m_SelectedIndexes.Clear();
            }

            m_SelectedIndexes.Add(indx);
            // Currently event is fired with an integer, but you can change this for any kind of object
            OnElementAdded?.Invoke(indx);
        }
        else
        {
            if (m_SelectedIndexes.Contains(indx))
            {
                m_SelectedIndexes.Remove(indx);
                // Currently event is fired with an integer, but you can change this for any kind of object
                OnElementRemoved?.Invoke(indx);
            }
            else
            {
                if (m_SelectedIndexes.Count < m_MaxSelectedElements || !m_HasSelectionLimit)
                {
                    m_SelectedIndexes.Add(indx);
                    // Currently event is fired with an integer, but you can change this for any kind of object
                    OnElementAdded?.Invoke(indx);
                }
            }
        }

        if (currentCount > 0 && (selectedItems == null || selectedItems.Length == 0))
        {
            // Currently event is fired with an integer, but you can change this for any kind of object
            OnListEmptied?.Invoke(indx);
        }

        // Currently event is fired with an integer, but you can change this for any kind of object
        OnSelectionChanged?.Invoke(indx);
        if (m_ClosePanelOnSelection)
            ToggleDropdownPanel(true);
        UpdateSelected();
    }

    /// <summary>
    /// Same as Original Method but without broadcasting the events
    /// </summary>
    /// <param name="indx"></param>
    private void OnItemClickedWithoutNotify(int indx)
    {
        if (m_Items[indx].isDisabled)
            return;
        if (!m_AllowMultiSelect)
        {
            if (m_SelectedIndexes.Count > 0)
            {
                m_SelectedIndexes.Clear();
            }

            m_SelectedIndexes.Add(indx);
        }
        else
        {
            if (m_SelectedIndexes.Contains(indx))
            {
                m_SelectedIndexes.Remove(indx);
            }
            else
            {
                if (m_SelectedIndexes.Count < m_MaxSelectedElements || !m_HasSelectionLimit)
                    m_SelectedIndexes.Add(indx);
            }
        }

        UpdateSelected();
    }

    /// <summary>
    /// Display the drop down list
    /// </summary>
    public void ToggleDropdownPanel(bool directClick = false)
    {
        m_OverlayRT.transform.localScale = new Vector3(1, 1, 1);
        m_ScrollBarRT.transform.localScale = new Vector3(1, 1, 1);
        m_IsPanelActive = !m_IsPanelActive;
        m_OverlayRT.gameObject.SetActive(m_IsPanelActive);
        if (m_IsPanelActive)
        {
            transform.SetAsLastSibling();
        }
        else if (directClick)
        {
            // If you want to implement a specific behaviour when the player clicks somewhere outside
        }
    }

    #endregion

    #region UI Components Update

    /// <summary>
    /// Rebuild panel's content
    /// </summary>
    private void RebuildPanel()
    {
        m_ItemsPool.Reset();
        if (m_Items.Count == 0) return;

        int indx = m_PanelItems.Count;
        while (m_PanelItems.Count < m_Items.Count)
        {
            var newItem = m_ItemsPool.Rent();
            newItem.name = "Item " + indx;
            newItem.transform.SetParent(m_ItemsPanelRT, false);

            m_PanelItems.Add(newItem);
            indx++;
        }

        for (int i = 0; i < m_PanelItems.Count; i++)
        {
            if (i < m_Items.Count)
            {
                var item = m_Items[i];

                m_PanelItems[i].text.text = item.caption;
                m_PanelItems[i].text.color = item.isDisabled ? m_DisabledTextColor : m_DefaultTextColor;
                m_PanelItems[i].image.enabled = true;
                if (item.image == null) m_PanelItems[i].image.enabled = false;
                ; //hide the button image  
                m_PanelItems[i].image.sprite = item.image;
                m_PanelItems[i].image.color = (item.image == null) ? new Color(1, 1, 1, 0)
                    : item.isDisabled ? new Color(1, 1, 1, .5f)
                    : Color.white;
                var ii = i; //have to copy the variable for use in anonymous function
                m_PanelItems[i].button.onClick.RemoveAllListeners();
                m_PanelItems[i].button.onClick.AddListener(() =>
                {
                    OnItemClicked(ii);
                    if (item.OnSelect != null) item.OnSelect();
                });
            }

            m_PanelItems[i].gameObject.SetActive(i < m_Items.Count); // if we have more thanks in the panel than Items in the list hide them
        }
    }

    /// <summary>
    /// Update the UI elements and the main button (name, color etc)
    /// </summary>
    private void UpdateSelected()
    {
        var buttonColors = m_MainButton.button.colors;
        for (var i = 0; i < m_ItemsPanelRT.childCount; i++)
        {
            m_PanelItems[i].buttonImage.color = (m_SelectedIndexes.Contains(i)) ? buttonColors.highlightedColor : buttonColors.normalColor;
        }

        //Get the first selected item
        var firstItem = selectedItems?.FirstOrDefault();
        //Update the main button background image if desired
        if (firstItem != null && firstItem.image != null && m_DisplayMainButtonImage)
        {
            m_MainButton.image.enabled = true;
            m_MainButton.image.sprite = firstItem.image;
            m_MainButton.image.color = Color.white;
        }
        else
        {
            m_MainButton.image.enabled = false;
        }

        // Change the button caption to either NoItems, the first item select or multiple items. Feel free to change this behaviour to your needs!
        m_MainButton.text.text = firstItem != null && selectedItems?.Length > 0
            ? (selectedItems?.Length > 1 ? m_MultipleSelectedItemsCaption : firstItem.caption)
            : m_NoSelectedItemsCaption;
    }

    /// <summary>
    /// Redraw the panel in which the items are rendered
    /// </summary>
    private void RedrawPanel()
    {
        if (!m_AutoUpdateContentSize)
            return;
        float scrollbarWidth = m_Items.Count > itemsToDisplay ? m_ScrollBarWidth : 0f; //hide the scrollbar if there's not enough items

        if (!m_HasDrawnOnce || m_RectTransform.sizeDelta != m_MainButton.rectTransform.sizeDelta)
        {
            m_HasDrawnOnce = true;

            m_ScrollPanelRT.SetParent(transform, true); //break the scroll panel from the overlay
            m_ScrollPanelRT.anchoredPosition = new Vector2(0, -m_RectTransform.sizeDelta.y); //anchor it to the bottom of the button

            //make the overlay fill the screen
            m_OverlayRT.SetParent(m_Canvas.transform, false); //attach it to top level object
            m_OverlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_CanvasRT.sizeDelta.x);
            m_OverlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_CanvasRT.sizeDelta.y);

            m_OverlayRT.SetParent(transform, true); //reattach to this object
            m_ScrollPanelRT.SetParent(m_OverlayRT, true); //reattach the scrollpanel to the overlay            
        }

        if (m_Items.Count < 1) return;

        float dropdownHeight = m_RectTransform.sizeDelta.y * Mathf.Min(m_ItemsToDisplay, m_Items.Count);

        m_ScrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownHeight);
        m_ScrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_RectTransform.sizeDelta.x - 10);
        m_ScrollPanelRT.anchoredPosition = new Vector2(m_ScrollPanelRT.anchoredPosition.x + 5f, m_ScrollPanelRT.anchoredPosition.y);
        m_ItemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_ScrollPanelRT.sizeDelta.x - scrollbarWidth - 5);
        m_ItemsPanelRT.anchoredPosition = new Vector2(5, 0);

        m_ScrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollbarWidth);
        m_ScrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownHeight);

        m_SlidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        m_SlidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, dropdownHeight - m_ScrollBarRT.sizeDelta.x);
    }

    #endregion
}