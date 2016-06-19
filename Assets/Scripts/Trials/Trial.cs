using UnityEngine;
using System.Collections;

public abstract class Trial : MonoBehaviour {
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    protected int BaseObjectiveCount;
    protected int ObjectiveCountMax = int.MaxValue;

    protected int BaseObjectCount;
    protected int ObjectCountMax = int.MaxValue;

    protected float BaseTimeLimit;
    protected float TimeLimitMin = 15;

    public int ObjectiveCount { get {
        int modifiedCount = Mathf.Clamp((int)(BaseObjectiveCount * TrialHandler.ObjectiveMultiplier + TrialHandler.ObjectivesAdded), 0, int.MaxValue);
        return modifiedCount < ObjectiveCountMax ? modifiedCount : ObjectiveCountMax;
    } }

    public int ObjectCount { get {
        int modifiedCount = Mathf.Clamp((int)(BaseObjectCount * TrialHandler.ObjectMultiplier + TrialHandler.ObjectsAdded), 0, int.MaxValue);
        return modifiedCount < ObjectCountMax ? modifiedCount : ObjectCountMax;
    }}

    public virtual void Setup() { }
    public virtual bool RequirementsMet { get { return false; } }
    public virtual void Cleanup() {
        TrialHandler.Instance.UnloadTrialScene();
    }

    protected GameObject Instantiate(GameObject source, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject temp = (GameObject)GameObject.Instantiate(source, position, rotation);
        temp.transform.parent = parent;
        return temp;
    }

    protected GameObject Instantiate(GameObject source, Vector3 position, Quaternion rotation)
    {
        return Instantiate(source, source.transform.position, source.transform.rotation, TrialHandler.Instance.loadOnThis.transform);
    }
    protected GameObject Instantiate(GameObject source)
    {
        return Instantiate(source, source.transform.position, source.transform.rotation);
    }

    protected virtual T LoadResource<T>(string resource) where T : Object
    {
        return Resources.Load<T>("Trials/" + GetType().Name + "/" + resource);
    }
}
