using UnityEngine;
using System.Collections;

using Random = UnityEngine.Random;

[Boss(0)]
public class Blimmy : Boss {
    public Blimmy()
    {
        Name = "Blimmy the Jimmy Blob";
        Description = "This Titty Boi Gon' Get You";

        littleBlimmyPrefab = LoadResource<GameObject>("LittleBlimmy");
    }

    bool loaded = false;
    GameObject littleBlimmyPrefab;

    public override void Setup()
    {
        Instantiate(LoadResource<GameObject>("Blimmy"));
        loaded = true;
    }

    void Update()
    {
        if (RequirementsMet)
            TrialHandler.Instance.TrialFinished();
    }

    public void BlimmyHurt(bool enraged)
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1));
        Instantiate(littleBlimmyPrefab, spawnPosition, Quaternion.identity);

        if (enraged)
        {
            foreach (LittleBlimmyController lb in
                TrialHandler.Instance.loadOnThis.transform.GetComponentsInChildren<LittleBlimmyController>())
                lb.attacking = true;
        }
    }

    public override bool RequirementsMet {
        get {
            return loaded && TrialHandler.Instance.loadOnThis.transform.GetComponentsInChildren<EnemyController>().Length == 0;
        }
    }
}
