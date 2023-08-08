using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingReal : MonoBehaviour
{
    public Transform target;
    public float maxDistance;
    public float darkenSpeed;
    
    private float currentDistance;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        currentDistance = Vector3.Distance(transform.position, target.position);

        if (currentDistance <= maxDistance)
        {
            float darkenAmount = (maxDistance - currentDistance) * darkenSpeed;
            meshRenderer.material.color -= new Color(darkenAmount, darkenAmount, darkenAmount, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        meshRenderer.material.color = Color.black;
    }

    private void OnCollisionExit(Collision collision)
    {
        meshRenderer.material.color = Color.white;
    }
}