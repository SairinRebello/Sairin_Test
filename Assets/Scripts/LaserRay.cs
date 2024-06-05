using System;
using UnityEngine;

public class LaserRay : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void FixedUpdate()
    {
        Vector3 laserEnd;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, -transform.up);
        if(Physics.Raycast(ray, out hit, maxDistance,layerMask))
        {
            laserEnd = hit.point;
            GameManager.Instance.OnGameOver?.Invoke(GameManager.Instance, EventArgs.Empty);
        }
        else
        {
            laserEnd = transform.position + (-transform.up * maxDistance);
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserEnd);
    }
}
