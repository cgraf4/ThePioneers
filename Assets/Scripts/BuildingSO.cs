using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "CGTools/Buildings", order = 0)]
public class BuildingSO : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private Building prefab;
    [SerializeField] private int health;
    
    public string Id => id;
    public Building Prefab => prefab;
    public int Health => health;
}