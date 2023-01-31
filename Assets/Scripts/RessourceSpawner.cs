using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RessourceSpawner : MonoBehaviour
{
    [SerializeField] private int spawnAmount;
    [SerializeField] private GameObject groundPlane;
    [SerializeField] private RessourcePrefabBundle[] allRessources;

    private Dictionary<Ressource, GameObject> ressourceDictionary;

    private int size;
    private Vector3 middlePoint = new Vector3(-500, 0, 0);

    void Start()
    {
        ressourceDictionary = new Dictionary<Ressource, GameObject>();
        for (int i = 0; i < allRessources.Length; i++)
        {
            ressourceDictionary.Add(allRessources[i].Type, allRessources[i].Prefab);
        }

        size = 10 * (int)groundPlane.transform.localScale.x;
        SpawnRessources(Ressource.Iron);
    }

    private void SpawnRessources(Ressource ressource)
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 position = middlePoint + new Vector3(Random.Range(-size * 0.5f, size * 0.5f),
                0,
                Random.Range(-size * 0.5f, size * 0.5f));
                
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                Instantiate(ressourceDictionary[ressource], hit.position, Quaternion.identity, transform);
            }
        }
    }
}
