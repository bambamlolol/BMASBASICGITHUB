using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject subPrefab;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlaySound(SoundObject soundObject) {
        source.PlayOneShot(soundObject.soundClip);
        createSub(soundObject);
    }

    public void createSub(SoundObject sO) {
        GameObject sub = GameObject.Instantiate(subPrefab, Vector3.zero, Quaternion.identity);
        sub.GetComponentInChildren<Subtitle>().audio = source;
        sub.GetComponentInChildren<Subtitle>().soundObjectt = sO;
        //sub.GetComponentInChildren<Subtitle>().target = gameObject;
    }
}
