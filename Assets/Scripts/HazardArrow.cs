using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardArrow : MonoBehaviour
{
    private Color m_red = new Color(1f, 0f, 0f, 0.4f);

    // Start is called before the first frame update
    void Start()
    {
        Renderer render = GetComponent<Renderer>();
        render.material.color = m_red;
    }

    // Update is called once per frame

}
