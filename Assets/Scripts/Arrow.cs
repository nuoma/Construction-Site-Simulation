using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Color m_green = new Color(0f, 1f, 0f, 0.5f);
    public bool rotate = true;
    public bool bounce = false;

    // Start is called before the first frame update
    void Start()
    {
        Renderer render = GetComponent<Renderer>();
        render.material.color = m_green;
    }


    // Update is called once per frame
    void Update()
    {
        
        float bounceValue = Mathf.PingPong(Time.time, 1) / 3;
        if (rotate && bounce)
        {
            transform.position = new Vector3(transform.position.x, bounceValue, transform.position.z);
            transform.Rotate(0, 70 * Time.deltaTime, 0); //rotates 50 degrees per second around Y axis
        }
        else if (rotate && !bounce)
        {
            transform.Rotate(0, 70 * Time.deltaTime, 0); //rotates 50 degrees per second around Y axis
        }
        else if (!rotate && bounce)
        {
            transform.position = new Vector3(transform.position.x, bounceValue, transform.position.z);
        }
    }

   

}
