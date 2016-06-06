using UnityEngine;
using System.Collections;

public abstract class Trial {
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    protected int BaseObjectiveCount;

    public int ObjectiveCountAdded = 0;
    public float ObjectiveCountModifier = 1;

    public int ObjectiveCount { get { return (int)((BaseObjectiveCount + ObjectiveCountAdded) * ObjectiveCountModifier); } }

    public abstract void Setup();
    public abstract bool RequirementsMet { get; }
    public virtual void Finish()
    {
        TrialHandler.main.EventFinished();
    }

    protected T LoadResource<T>(string resource) where T : UnityEngine.Object
    {
        return Resources.Load<T>("Trials/" + GetType().Name + "/" + resource);
    }
}
