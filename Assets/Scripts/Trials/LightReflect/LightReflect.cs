using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

// TODO: Check for impossible setup (i.e. only mirror in between source and target)
public class LightReflect : Trial
{
    static float MinimumDistance = 1.5f;
    static int MaxInstantiateAttempts = 100;

    public LightReflect()
    {
        Name = "Light Reflect";
        Description = "Rotate the mirrors to reflect the light into the portal";

        BaseObjectCount = 1;

        lightSourcePrefab = LoadResource<GameObject>("LightSource");
        mirrorPrefab = LoadResource<GameObject>("Mirror");
        lightTargetPrefab = LoadResource<GameObject>("LightTarget");
    }

    GameObject lightSourcePrefab;
    GameObject mirrorPrefab;
    GameObject lightTargetPrefab;

    LightTargetController lightTarget;

    public void LightSourceUpdate(int objectsHit, GameObject lastObjectHit)
    {
        lightTarget.BeingFilled = objectsHit == ObjectCount + 1 && lastObjectHit == lightTarget.gameObject;
    }

    void Update()
    {
        if (RequirementsMet)
            TrialHandler.Instance.EventFinished();
    }

    public override void Setup()
    {
        Vector2 position;
        bool validPosition;
        int attemptsMade = 0;
        for (int i = 0; i < ObjectCount + 2; i++)
        {            
            do
            {
                attemptsMade++;
                validPosition = true; 
                position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                foreach (Transform t in transform)
                    if (Vector3.Distance(position, t.position) <= MinimumDistance)
                        validPosition = false;
            } while (attemptsMade < MaxInstantiateAttempts && !validPosition);

            if (i == 0)
                Instantiate(lightSourcePrefab, position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            else if (i == ObjectCount + 1)
                lightTarget = Instantiate(lightTargetPrefab, position, Quaternion.identity).GetComponent<LightTargetController>();
            else
                Instantiate(mirrorPrefab, position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
        }
    }

    public override bool RequirementsMet
    {
        get { return lightTarget != null ? lightTarget.Full : false; }
    }
}
