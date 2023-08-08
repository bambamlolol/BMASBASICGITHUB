using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaTowerSecret : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip secret;
    // Start is called before the first frame update
    void Start()
    {
        int value = UnityEngine.Random.Range(1, 1001);
        if (value == 1000)
        {
            audioSource.clip = secret;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
