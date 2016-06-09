using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class PointMove : Trial {
    public PointMove()
    {
        Name = "Move to Point";
        Description = "Move to the highlighted point(s)";

        BaseObjectiveCount = 1;

        pointPrefab = LoadResource<GameObject>("Point");
    }

    Vector3 lastPosition;
    int objectivesComplete = 0;
    GameObject pointPrefab;

    public void PointReached()
    {
        objectivesComplete++;
        if (RequirementsMet)
            Finish();
        else
            Setup();
    }

    public override void Setup()
    {
        Vector3 position;
        do
        {
            position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        } while (lastPosition != null && Vector3.Distance(lastPosition, position) > 1);
        lastPosition = position;
        Instantiate(pointPrefab, position, Quaternion.identity);
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }
}
