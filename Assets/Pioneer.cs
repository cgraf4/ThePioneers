using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pioneer : MonoBehaviour
{
    private Pathfinding pathfinding;

    private void Awake()
    {
        pathfinding= GetComponent<Pathfinding>();
        UnitManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        UnitManager.Instance.Remove(this);
    }

    public void SetDestination(Vector3 position)
    {
        pathfinding.SetDestination(position);
    }


}
