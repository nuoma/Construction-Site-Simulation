using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public LineRenderer lineRenderer;       // visual cable from the crane to the hook
    public GameObject cableAnchorPosition;  // position where the cable connects to the crane

    void Update ()
    {
        // make the cable anchor follow us along the crane
        Vector3 newPos = cableAnchorPosition.transform.localPosition;
        newPos.x = transform.localPosition.x;
        cableAnchorPosition.transform.localPosition = newPos;

        // connect the line renderer from the cable anchor to us
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, cableAnchorPosition.transform.position);
    }
}