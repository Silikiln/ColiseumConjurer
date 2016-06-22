using UnityEngine;
using System.Collections;

public abstract class Trial : MonoBehaviour {
    public enum TrialState { Loading, Active, Ending }

    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public TrialState CurrentState = TrialState.Loading;

    protected int BaseObjectiveCount;
    protected int ObjectiveCountMin = 1;
    protected int ObjectiveCountMax = int.MaxValue;

    protected int BaseObjectCount;
    protected int ObjectCountMin = 1;
    protected int ObjectCountMax = int.MaxValue;

    protected float BaseTimeLimit = 60;
    protected float TimeLimitMin = 15;
    protected float TimeLimitMax = float.MaxValue;

    public int ObjectiveCount { get {
        return Mathf.Clamp((int)(BaseObjectiveCount * TrialHandler.ObjectiveMultiplier + TrialHandler.ObjectivesAdded), ObjectiveCountMin, ObjectiveCountMax);
    } }

    public int ObjectCount { get {
        return Mathf.Clamp((int)(BaseObjectCount * TrialHandler.ObjectMultiplier + TrialHandler.ObjectsAdded), ObjectCountMin, ObjectCountMax);
    } }

    public float TimeLimit { get {
            return Mathf.Clamp(BaseTimeLimit * TrialHandler.TimeLimitMultiplier + TrialHandler.TimeLimitAdded, TimeLimitMin, TimeLimitMax);
    } }

    public virtual void Setup() { CurrentState = TrialState.Active; }
    public virtual bool RequirementsMet { get { return false; } }
    public virtual void Cleanup() {
        CurrentState = TrialState.Ending;
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
