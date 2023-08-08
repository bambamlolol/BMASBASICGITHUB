using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGPTLighting : MonoBehaviour
{
    public List<GameObject> lightPoints;
    private Renderer[] childRenderers;
    public Color targetColor;

    private void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        foreach (Renderer renderer in childRenderers)
        {
            Material material = renderer.material;

            foreach (GameObject point in lightPoints)
            {
                float distance = Vector3.Distance(transform.position, point.transform.position);
                float colorChangeFactor = Mathf.Clamp01(distance / 10);
                material.color = Color.Lerp(Color.white, targetColor, colorChangeFactor);
            }
        }
    }
}