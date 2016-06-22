using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class ShapeSlot : Trial
{
    static float MinimumDistance = 1.5f;

    public ShapeSlot()
    {
        Name = "Shape Slots";
        Description = "Move the shape(s) into the matching slot(s)";

        BaseObjectiveCount = 1;
        BaseObjectCount = 0;

        shapePrefab = LoadResource<GameObject>("Shape");
        slotPrefab = LoadResource<GameObject>("Slot");
    }

    int objectivesComplete = 0;
    GameObject shapePrefab;
    GameObject slotPrefab;

    // TODO: Add multiple possible shapes to up the complexity
    //Sprite[] shapeSprites;

    public void SlotFilled()
    {
        objectivesComplete++;
        if (RequirementsMet)
            TrialHandler.Instance.TrialFinished();
    }

    public override void Setup()
    {
        Vector3 shapePosition, slotPosition;
        for (int i = 0; i < ObjectiveCount; i++)
        {            
            do
            {
                shapePosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                slotPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            } while (Vector3.Distance(shapePosition, slotPosition) <= MinimumDistance);
            Instantiate(shapePrefab, shapePosition, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            Instantiate(slotPrefab, slotPosition, Quaternion.Euler(0, 0, Random.Range(0, 359)));
        }
        base.Setup();
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }
}
