﻿using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class PointMove : Trial {
    static float MinimumDistance = 1;

    public PointMove()
    {
        Name = "Move to Point";
        Description = "Move to the highlighted point(s)";

        BaseObjectiveCount = 1;

        pointPrefab = LoadResource<GameObject>("Point");
    }

    Vector2 lastPosition = PlayerController.Instance.transform.position;
    int objectivesComplete = 0;
    GameObject pointPrefab;

    public void PointReached()
    {
        objectivesComplete++;
        if (RequirementsMet)
            TrialHandler.Instance.EventFinished();
        else
            Setup();
    }

    public override void Setup()
    {
        Vector2 position;
        do
        {
            position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        } while (Vector3.Distance(lastPosition, position) <= MinimumDistance);
        lastPosition = position;
        Instantiate(pointPrefab, position, Quaternion.identity);
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }
}
