using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject grabber;
    public float minDistance = 5f;
    public bool triggered = false;
    private float distance;

    void OnMouseDown() {
        distance = Vector3.Distance(this.transform.position, grabber.transform.position);
        if (distance <= minDistance) {
            triggered = !triggered;
            Debug.Log(triggered);
        }
    }
}
