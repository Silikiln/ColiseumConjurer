using UnityEngine;
using System.Collections;
using System;

using Random = UnityEngine.Random;

public class HideAndSeek : Trial
{
    static float MinimumDistance = .1f;
    static float ExpandDuration = 1;
    static float ExpandDelta = .1f;
    static float ExpandAddedScale = 1f;
    
    public HideAndSeek()
    {
        Name = "Hide And Seek";
        Description = "Find The Exit";

        BaseObjectiveCount = 3;
        detectorPrefab = LoadResource<GameObject>("Detector");
    }

    int objectivesComplete = 0;
    GameObject detectorPrefab;
    GameObject detector;
    SpriteRenderer detectorRenderer;
    Vector2 targetPosition;
    IEnumerator objectiveCompleteCoroutine;
    bool loaded = false;

    public override void Setup()
    {
        targetPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);

        detector = Instantiate(detectorPrefab, PlayerController.Instance.transform.position, Quaternion.identity);
        detector.transform.parent = PlayerController.Instance.transform;

        detectorRenderer = detector.GetComponent<SpriteRenderer>();
        loaded = true;
    }

    void Update()
    {
        if (!loaded) return;

        float distance = Vector2.Distance(PlayerController.Instance.transform.position, targetPosition);
        if (distance <= MinimumDistance)
        {
            objectivesComplete++;
            if (RequirementsMet)
                TrialHandler.Instance.TrialFinished();
            else
            {
                targetPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                if (objectiveCompleteCoroutine != null)
                    StopCoroutine(objectiveCompleteCoroutine);
                objectiveCompleteCoroutine = ObjectiveCompleteNotice();
                StartCoroutine(objectiveCompleteCoroutine);
            }
        }

        if (distance < 1.5)
            detectorRenderer.color = Color.Lerp(Color.green, Color.yellow, distance / 1.5f);
        else if (distance < 3)
            detectorRenderer.color = Color.Lerp(Color.yellow, Color.red, (distance - 1.5f) / 1.5f);
        else        
            detectorRenderer.color = Color.red;
    }

    public override bool RequirementsMet
    {
        get { return objectivesComplete >= ObjectiveCount; }
    }

    IEnumerator ObjectiveCompleteNotice()
    {
        float timeElapsed = 0;

        while (timeElapsed < ExpandDuration)
        {
            yield return new WaitForSeconds(ExpandDelta);
            timeElapsed += ExpandDelta;
            Vector3 scale = Vector3.one * (1 + ExpandAddedScale * timeElapsed / ExpandDuration);
            scale.z = 1;
            detector.transform.localScale = scale;
        }

        timeElapsed = 0;
        while (timeElapsed < ExpandDuration)
        {
            yield return new WaitForSeconds(ExpandDelta);
            timeElapsed += ExpandDelta;
            Vector3 scale = Vector3.one * (1 + ExpandAddedScale - ExpandAddedScale * timeElapsed / ExpandDuration);
            scale.z = 1;
            detector.transform.localScale = scale;
        }
    }
}