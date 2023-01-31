using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SeasonColorBundle
{
    public Season Season;
    public Color Color;
}

[System.Serializable]
public struct RessourcePrefabBundle
{
    public Ressource Type;
    public GameObject Prefab;
}