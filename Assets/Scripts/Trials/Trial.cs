﻿using UnityEngine;
using System.Collections;

public abstract class Trial : MonoBehaviour {
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    protected int BaseObjectiveCount;
    protected int ObjectiveCountMax = int.MaxValue;
    public int ObjectiveCountAdded = 0;
    public float ObjectiveCountModifier = 1;

    protected int BaseObjectCount;
    protected int ObjectCountMax = int.MaxValue;
    public int ObjectCountAdded = 0;
    public float ObjectCountModifier = 1;

    public int ObjectiveCount { get {
        int modifiedCount = (int)((BaseObjectiveCount + ObjectiveCountAdded) * ObjectiveCountModifier);
        return modifiedCount < ObjectiveCountMax ? modifiedCount : ObjectiveCountMax;
    } }

    public int ObjectCount { get {
        int modifiedCount = (int)((BaseObjectCount + ObjectCountAdded) * ObjectCountModifier);
        return modifiedCount < ObjectCountMax ? modifiedCount : ObjectCountMax;
    }}

    public abstract void Setup();
    public abstract bool RequirementsMet { get; }
    public virtual void Cleanup() {
        TrialHandler.Instance.UnloadTrialScene();
    }

    protected GameObject Instantiate(GameObject source, Vector3 position, Quaternion rotation)
    {
        GameObject temp = (GameObject)GameObject.Instantiate(source, position, rotation);
        temp.transform.parent = TrialHandler.Instance.loadOnThis.transform;
        return temp;
    }
    protected GameObject Instantiate(GameObject source)
    {
        return Instantiate(source, source.transform.position, source.transform.rotation);
    }

    protected T LoadResource<T>(string resource) where T : UnityEngine.Object
    {
        return Resources.Load<T>("Trials/" + GetType().Name + "/" + resource);
    }
}
