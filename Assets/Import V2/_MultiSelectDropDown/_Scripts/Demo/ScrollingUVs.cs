using UnityEngine;
using UnityEngine.UI;

public class ScrollingUVs : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    [SerializeField] private float m_ScrollSpeed = 0.001f;
    [SerializeField] private Vector2 m_OffsetDirection = new Vector2(-1, 1);
    private bool orientation = false;

    private void Awake()
    {
        m_Image.material = new Material(m_Image.material);
        m_Image.material.mainTextureOffset = Vector2.zero;
    }

    private void Update()
    {
        var material = m_Image.material;
        var direction = orientation ? m_OffsetDirection : -m_OffsetDirection;
        material.mainTextureOffset += direction * (Time.fixedDeltaTime * m_ScrollSpeed);
        if (Mathf.Abs(material.mainTextureOffset.x) > 0.025f)
        {
            orientation = !orientation;
        }
    }
}