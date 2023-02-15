using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : Singleton<BuildingsManager>
{
    [Header("Rotation")]
    [SerializeField] private float rotationStep;
    [SerializeField] private float rotationDuration;

    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private Material previewMaterial;
    [SerializeField, Expandable] private BuildingSO[] allBuildings;
    [SerializeField, ReadOnly] private List<Building> selectableBuildings;
    [SerializeField, ReadOnly] private List<Building> placedBuildings;

    private Dictionary<Building, MeshRenderer[]> meshRenderers; // Caching the meshRenderer in the Building class would also be possible and useful
                                                                // This is just to show another way to handle object-specific data in a managed way
    private int idSelected;
    private float currentRotation;
    private bool canPlace;
    private Coroutine rotationRoutine;

    private void Start()
    {
        idSelected = -1;
        selectableBuildings = new List<Building>();
        LoadBuildings();
    }

    private void Update()
    {
        if (idSelected < 0) return;

        Vector3 test = new Vector3(5, 5, 5);
        test.RemoveVectorComponentExtended(VectorValue.X);

        selectableBuildings[idSelected].transform.position = InputHandler.Instance.GroundHit.point;
        selectableBuildings[idSelected].transform.rotation =
                Helper.SetUpAndYAxisRotation(selectableBuildings[idSelected].transform.forward, InputHandler.Instance.GroundHit.normal);

        //if (CheckForCollisions())
        //{
        //    meshRenderers[selectableBuildings[idSelected]].material.color = Color.red;
        //}
        //else
        //{
        //    meshRenderers[selectableBuildings[idSelected]].material.color = Color.green;
        //}
        // This if-else statement is basically the same as the 'Inline-If-Statement' inside the following loop

        for (int i = 0; i < meshRenderers[selectableBuildings[idSelected]].Length; i++)
        {
            meshRenderers[selectableBuildings[idSelected]][i].material.color = CheckForCollisions() ? new Color(1, 0, 0, .5f) : new Color(0, 1, 0, .5f);
        }
    }

    private void LoadBuildings()
    {
        meshRenderers = new Dictionary<Building, MeshRenderer[]>();

        foreach (var building in allBuildings)
        {
            Building b = Instantiate(building.Prefab, new Vector3(0, -50, 0), Quaternion.identity, transform);

            MeshRenderer[] meshes = b.transform.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer meshRenderer in meshes)
            {
                meshRenderer.material = previewMaterial;
            }

            meshRenderers.Add(b, meshes);

            b.gameObject.SetActive(false);
            selectableBuildings.Add(b);
        }
    }

    private bool CheckForCollisions()
    {
        Bounds currentBounds = selectableBuildings[idSelected].Collider.bounds;

        if (Physics.OverlapBox(currentBounds.center, currentBounds.extents, selectableBuildings[idSelected].transform.rotation, blockingLayer).Length > 1)
        {
            canPlace = false;
            return true;
        }

        canPlace = true;
        return false;
    }

    public void SelectBuilding(int selection)
    {
        currentRotation = 0;

        if (idSelected >= 0) selectableBuildings[idSelected].gameObject.SetActive(false);

        idSelected = selection;

        if (idSelected >= 0) selectableBuildings[selection].gameObject.SetActive(true);
    }

    public void PlaceBuilding()
    {
        if (!canPlace) return;

        var spawnedBuilding = Instantiate(allBuildings[idSelected].Prefab, InputHandler.Instance.GroundHit.point, Quaternion.identity, transform);

        // Potential rotation iprovement: 
        // Cast rays down from each corner of the building (we already have bounds)
        // Get normal from each hit
        // Get normal median and set up vector accordingly to median

        selectableBuildings[idSelected].gameObject.SetActive(false);
        placedBuildings.Add(spawnedBuilding);

        spawnedBuilding.transform.localRotation = Quaternion.Euler(spawnedBuilding.transform.rotation.x, currentRotation, spawnedBuilding.transform.rotation.z);
        spawnedBuilding.transform.rotation = Helper.SetUpAndYAxisRotation(spawnedBuilding.transform.forward, InputHandler.Instance.GroundHit.normal);

        SelectBuilding(-1);
        currentRotation = 0;

        InputHandler.Instance.SetBuildMode(false);
    }

    public void Rotate(float direction)
    {
        if (idSelected < 0) return;

        currentRotation += direction * rotationStep;

        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
        rotationRoutine = StartCoroutine(RotateRoutine());
    }

    private IEnumerator RotateRoutine()
    {
        float timer = 0f;
        float percentage = 0f;
        Quaternion start = selectableBuildings[idSelected].transform.rotation;
        Quaternion end = Quaternion.Euler(new Vector3(selectableBuildings[idSelected].transform.rotation.x, currentRotation,
                                                                            selectableBuildings[idSelected].transform.rotation.z));

        while (timer < rotationDuration)
        {
            timer += Time.deltaTime;
            percentage = timer / rotationDuration;

            selectableBuildings[idSelected].transform.rotation = Quaternion.Slerp(start, end, percentage);

            yield return null;
        }

        rotationRoutine = null;
    }
}
