using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingsManager : Singleton<BuildingsManager>
{
    [SerializeField] private Material previewMaterial;
    [SerializeField] private BuildingSO[] allBuildings;
    [SerializeField] private List<GameObject> selectableBuildings;
    
    private int idSelected;
    

    private void Start()
    {
        idSelected = 0;
        selectableBuildings = new List<GameObject>();
        LoadBuildings();
    }

    private void LoadBuildings()
    {
        foreach (var building in allBuildings)
        {
            var b = Instantiate(building.Prefab, new Vector3(0, -50, 0), 
                Quaternion.identity, transform);
            
            var meshes = b.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshes)
            {
                meshRenderer.material = previewMaterial;
            }
            b.gameObject.SetActive(false);
            selectableBuildings.Add(b);
        }
    }

    public void SelectBuilding(int selection)
    {
        Debug.Log("HUHU " + selection);
        selectableBuildings[idSelected].SetActive(false);
        idSelected = selection;
        selectableBuildings[idSelected].SetActive(true);
    }
}
