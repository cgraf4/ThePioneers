using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    [SerializeField] private List<Pioneer> allPioneers;

    protected override void Awake()
    {
        base.Awake();
        allPioneers = new List<Pioneer>();
    }

    public void Add(Pioneer p)
    {
        if (allPioneers.Contains(p))
            return;

        allPioneers.Add(p);
    }

    public void Remove(Pioneer p)
    {
        if(allPioneers.Contains(p))
            allPioneers.Remove(p);
    }

    public void SetPioneerDestination(Vector3 position)
    {
        foreach (var p in allPioneers)
        {
            p.SetDestination(position);
        }
    }

    public void HarvestRessource(RessourceSource source)
    {
        Debug.Log("BUDDELN");
    }
}
