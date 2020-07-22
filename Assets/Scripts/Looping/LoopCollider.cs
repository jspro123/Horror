using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopCollider : MonoBehaviour
{
    private LoseLooping loseLooping;

    private void Start()
    {
        loseLooping = FindObjectOfType<LoseLooping>();
    }


    private void OnTriggerEnter(Collider other)
    {
        loseLooping.HandleMovement(this.gameObject);
    }
}
