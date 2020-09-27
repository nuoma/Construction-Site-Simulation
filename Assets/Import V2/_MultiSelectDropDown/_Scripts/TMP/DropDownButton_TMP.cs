using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Button))]
public class DropDownButton_TMP : MonoBehaviour
{
    public RectTransform rectTransform;
    public Button button;
    public TextMeshProUGUI text;
    public Image buttonImage;
    
    public Image image;
    
    // This is your Drop Down Item Prefab
    // Include, Remove anything you want in here to fit your needs
}