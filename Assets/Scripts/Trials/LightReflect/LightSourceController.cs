using UnityEngine;
using System.Collections;

public class LightSourceController : MonoBehaviour {

    static int MaxBounce = 25;
    static float lightSourceOffset = .15f;
    ImprovedLineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = transform.GetComponentInChildren<ImprovedLineRenderer>();
        lineRenderer.AddPoint(transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        lineRenderer.Clear();
        lineRenderer.AddPoint(transform.position);

        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 lightDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        RaycastHit2D raycastHit = new RaycastHit2D();
        raycastHit.point = (Vector2)transform.position + lightDirection * (GetComponent<Collider2D>().bounds.extents.x / 2);

        int bounceCount = 0;
        do
        {
            bounceCount++;
            raycastHit = Physics2D.Raycast(raycastHit.point + lightDirection * lightSourceOffset, lightDirection);
            if (raycastHit.collider)
            {
                lineRenderer.AddPoint(raycastHit.point);
                lightDirection = Vector2.Reflect(lightDirection, raycastHit.normal);
            }
        } while (bounceCount < MaxBounce && raycastHit.collider 
            && raycastHit.collider.gameObject != gameObject && raycastHit.collider.gameObject.layer == 10);

        if (raycastHit.collider)
            ((LightReflect)TrialHandler.CurrentTrial).LightSourceUpdate(bounceCount, raycastHit.collider.gameObject);
    }
}
