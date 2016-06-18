using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class HideAndSeek : Trial
{
    static float MinimumDistance = 1;
    Vector2 lastPosition;
    Vector2 endPosition;
    public HideAndSeek()
    {
        Name = "Hide And Seek";
        Description = "Find The Exit";

        BaseObjectiveCount = 1;
        pointPrefab = LoadResource<GameObject>("Point");
        detectorPrefab = LoadResource<GameObject>("Detector");
    }

    int objectivesComplete = 0;
    GameObject pointPrefab;
    GameObject detectorPrefab;
    GameObject detector;
    SpriteRenderer detectorRenderer;
    Boolean begin = false;

    public override void Setup()
    {
        if (lastPosition == null)
            lastPosition = PlayerController.Instance.transform.position;
        do
        {
            endPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        } while (Vector3.Distance(lastPosition, endPosition) <= MinimumDistance);
        lastPosition = endPosition;
        
        detector = Instantiate(detectorPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
        detector.transform.parent = PlayerController.Instance.transform;

        detectorRenderer = detector.GetComponent<SpriteRenderer>();
        begin = true;
    }

    void Update()
    {
        if(!begin)
        {
            return;
        }

        float distance = Vector2.Distance(PlayerController.Instance.transform.position, endPosition);
        if (distance <= 0.1)
        {
            objectivesComplete++;
            endPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            if (RequirementsMet)
                TrialHandler.Instance.EventFinished();
        }
        else if (distance < 1)
        {

            detectorRenderer.color = Color.green;
        }
        else if (distance < 3)
        {
            detectorRenderer.color = Color.yellow;
        }
        else
        {
            detectorRenderer.color = Color.red;
        }
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }
}