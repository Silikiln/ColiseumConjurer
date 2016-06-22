using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class Chase : Trial
{
    static float MinimumDistance = 1;
    Vector2 lastPosition;
    Vector2 chasePosition;
    Vector2 startPosition;
    float targetRadius = 1.5f;
    float movementDuration;
    float movement = 0;
    float startTime;
    float distance;

    public Chase()
    {
        Name = "Chase";
        Description = "Follow The Leader";

        BaseObjectiveCount = 1;
        chasePrefab = LoadResource<GameObject>("ChasePoint");
        detectorPrefab = LoadResource<GameObject>("ChaseDetector");
    }
    
    GameObject chasePrefab;
    GameObject detector;
    GameObject detectorPrefab;
    SpriteRenderer detectorRenderer;
    ChasePointController chasePoint;

    bool moving;

    public override void Setup()
    {
        do
        {
            chasePosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        } while (Vector3.Distance(lastPosition, chasePosition) <= MinimumDistance);
        lastPosition = chasePosition;

        chasePoint = Instantiate(chasePrefab, chasePosition, Quaternion.identity).GetComponent<ChasePointController>();
        detector = Instantiate(detectorPrefab, chasePoint.gameObject.transform.position, Quaternion.identity);
        detector.transform.parent = chasePoint.gameObject.transform;

        detectorRenderer = detector.GetComponent<SpriteRenderer>();
        float currentRadius = detectorRenderer.sprite.bounds.extents.x;
        float newScaleScalar = targetRadius / currentRadius;
        Vector3 newScale = Vector3.one * newScaleScalar;
        newScale.z = 1;
        detector.transform.localScale = newScale;

        base.Setup();
    }

    void Update()
    {
        if (RequirementsMet)
            TrialHandler.Instance.TrialFinished();

        if (CurrentState == TrialState.Loading)
            return;

        distance = Vector2.Distance(PlayerController.Instance.transform.position, chasePoint.transform.position);
        if (distance < 1.5)
        {
            chasePoint.BeingFilled = true;
        }
        else
        {
            chasePoint.BeingFilled = false;
        }

        if (moving)
        {
            movement += (Time.deltaTime) / movementDuration;
            chasePoint.transform.position = Vector3.Lerp(startPosition, chasePosition, movement);
        }
        else
        {
            chasePosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            startPosition = chasePoint.transform.position;
            movementDuration = 2;
            //print(movement);
            movement = 0;
            moving = true;
            
        }

        if (movement >= 1)
        {
            moving = false;
        }
    }
    public override bool RequirementsMet
    {
        get { return chasePoint != null ? chasePoint.Full : false; }
    }
}