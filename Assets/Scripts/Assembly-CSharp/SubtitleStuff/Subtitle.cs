using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Subtitle : MonoBehaviour
{
    public TMP_Text sub;
    public SoundObject soundObjectt;
    public AudioSource audio;
    public Transform soundTran;
    public Transform camTran;
    public RectTransform rectTransform;
    public float maxScaleDistance = 40; // Adjust this value to control the maximum scaling

    private void Start()
    {
        sub.text = soundObjectt.words;
        sub.color = soundObjectt.color;
        camTran = GameObject.Find("Main Camera").transform;
        soundTran = audio.transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(camTran.position, soundTran.position);
        float scaleFactor = Mathf.Clamp(1.0f - (distance / maxScaleDistance), 0.1f, 1.0f); // Scale from 0.1 to 1.0
        rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // Rotate the subtitle towards the camera
        Vector3 cameraToSound = soundTran.position - camTran.position;
        Vector3 cameraToSoundProjection = Vector3.ProjectOnPlane(cameraToSound, camTran.forward);
        float angle = Vector3.SignedAngle(Vector3.forward, cameraToSoundProjection, camTran.up);
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        if (!audio.isPlaying)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
