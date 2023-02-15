using System;
using UnityEngine;

public abstract class Pioneer : MonoBehaviour
{
    public enum PioneerStates { Idle, Walking, Working }

    private PioneerStates currentState;

    protected Pathfinding pathfinding;
    protected Animator animator;


    protected virtual void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        UnitManager.Instance.Add(this);

        currentState = PioneerStates.Idle;
    }

    protected virtual void OnDestroy()
    {
        UnitManager.Instance.Remove(this);
    }

    public void SetDestination(Vector3 position)
    {
        pathfinding.SetDestination(position);
    }

    public void SetState(PioneerStates newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case PioneerStates.Idle:
                DoIdle();
                break;
            case PioneerStates.Walking:
                DoWalking();
                break;
            case PioneerStates.Working:
                DoWorking();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PlayAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    protected void DoIdle()
    {

    }

    protected void DoWalking()
    {

    }

    protected abstract void DoWorking();
}
