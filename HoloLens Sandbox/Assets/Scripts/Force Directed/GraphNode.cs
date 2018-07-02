using UnityEngine;

public abstract class GraphNode : MonoBehaviour {

    protected static ForceDirectionManager manager;

    protected abstract void DoGravity();
    protected abstract void DoRepulse();
    protected abstract void DoFreeze();

    protected virtual void Start()
    {
        manager = FindObjectOfType<ForceDirectionManager>();
    }
    void FixedUpdate()
    {
        if (manager.RepulseActive)
        {
            DoRepulse();
        }else if (manager.GravityActive)
        {
            DoGravity();
        }
        else
        {
            DoFreeze();
        }
    }
}
