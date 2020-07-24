using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSounds : MonoBehaviour
{
    public GameObject soundObject;
    public int threshold;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            int rand = Random.Range(0,100);
            if (rand < threshold) {
                soundObject.GetComponent<AudioSource>().Play(0);
            }
        }
    }
}
