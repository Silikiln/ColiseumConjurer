using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

public class SimonSays : Trial
{
    static Color[] PointColors = { Color.yellow, new Color(1, .65f, 0), Color.red, Color.magenta, Color.blue, Color.green };
    static float PatternRevealDelay = .5f;

    public SimonSays()
    {
        Name = "Solomon Says";
        Description = "Walk over the panels to repeat the pattern";

        BaseObjectiveCount = 3;

        pointPrefab = LoadResource<GameObject>("SimonPoint");
    }

    bool ready = false;

    SimonPointController[] points;
    int[] actualPattern;
    List<int> playerPattern;
    bool showingPattern = false;
    IEnumerator shownPattern;
    GameObject pointPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !showingPattern)
            StartCoroutine(ShowPattern());
    }

    public void PointReached(int id)
    {
        if (!ready) return;

        if (showingPattern)
        {
            StopCoroutine(shownPattern);
            showingPattern = false;
            for (int i = 0; i < points.Length; i++) points[i].UseShadedColor(true);
        }

        playerPattern.Add(id);
        if (InvalidPattern)
        {
            playerPattern.Clear();
            shownPattern = ShowPattern();
            StartCoroutine(shownPattern);
        }
        else if (RequirementsMet)
            TrialHandler.Instance.EventFinished();
    }

    bool InvalidPattern
    {
        get
        {
            if (playerPattern.Count > actualPattern.Length) return true;
            for (int i = 0; i < playerPattern.Count; i++)
                if (playerPattern[i] != actualPattern[i]) return true;

            return false;
        }
    }

    IEnumerator ShowPattern()
    {
        if (!ready) yield break;
        for (int i = 0; i < points.Length; i++) points[i].UseShadedColor(true);

        showingPattern = true;
        for (int i = 0; i < actualPattern.Length; i++)
        {
            points[actualPattern[i]].UseShadedColor(false);
            yield return new WaitForSeconds(PatternRevealDelay);
            points[actualPattern[i]].UseShadedColor(true);
            yield return new WaitForSeconds(.25f);
        }
        showingPattern = false;
    }

    public override void Setup()
    {
        float radius = 3;
        float delta = (2 * Mathf.PI) / PointColors.Length;
        float angle = 0;
        
        Vector3 position;
        points = new SimonPointController[PointColors.Length];
        for (int i = 0; i < PointColors.Length; i++, angle -= delta) {
            position = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
            points[i] = ((GameObject)GameObject.Instantiate(pointPrefab, position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg))).GetComponent<SimonPointController>();
            points[i].Initialize(i, PointColors[i]);
        }

        playerPattern = new List<int>();
        actualPattern = new int[ObjectiveCount];
        for (int i = 0; i < ObjectiveCount; i++)
            actualPattern[i] = Random.Range(0, PointColors.Length);

        ready = true;

        shownPattern = ShowPattern();
        StartCoroutine(shownPattern);
    }

    public override void Cleanup()
    {
        for (int i = 0; i < PointColors.Length; i++)
            Destroy(points[i].gameObject);
    }

    public override bool RequirementsMet
    {
        get { return !InvalidPattern && playerPattern.Count == actualPattern.Length; }
    }
}
