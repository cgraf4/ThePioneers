using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class BuildingsManager : Singleton<BuildingsManager>
{
    [SerializeField] private int rotationStep;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private Material previewMaterial;
    [SerializeField, Expandable] private BuildingSO[] allBuildings;
    [SerializeField, ReadOnly] private List<Building> selectableBuildings;
    [SerializeField, ReadOnly] private List<Building> placedBuildings;

    [Header("Seasons")]
    [SerializeField] private int currentSeason;
    [SerializeField] private SeasonColorBundle[] seasonColors;
    [SerializeField] private float seasonDuration;
    [SerializeField] private float seasonChangeDuration;
    [SerializeField] private Material groundMaterial;

    private Dictionary<Building, MeshRenderer[]> meshRenderers; // Caching the meshRenderer in the Building class would also be possible and useful
    // This is just to show another way to handle object-specific data in a managed way
    private int idSelected;
    private int currentRotation;
    private bool canPlace;

    private void Start()
    {
        idSelected = -1;
        selectableBuildings = new List<Building>();
        groundMaterial.color = seasonColors[currentSeason].Color;
        LoadBuildings();
        StartCoroutine(SeasonCycle());
    }

    private void Update()
    {
        if (idSelected < 0) return;

        Vector3 test = new Vector3(5, 5, 5);
        test.RemoveVectorComponentExtended(VectorValue.X);

        selectableBuildings[idSelected].transform.position = InputHandler.Instance.hitInfo.point;
        selectableBuildings[idSelected].transform.rotation =
                Helper.SetUpAndYAxisRotation(selectableBuildings[idSelected].transform.forward, InputHandler.Instance.hitInfo.normal);

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

    private IEnumerator SeasonCycle()
    {
        float changeTimer = 0f;
        float percentage = 0f;

        while (true)
        {
            yield return new WaitForSeconds(seasonDuration);
            
            // wenn mehr als max dann currentSeason = 0;
            currentSeason = (currentSeason + 1) % 4; // 1 2 3 0 1 2 3 0 1 2 3 0 1 2 3 0 1 2 3 

            Color start = seasonColors[currentSeason - 1 < 0 ? 3 : currentSeason - 1].Color;
            Color end = seasonColors[currentSeason].Color;
            
            changeTimer = 0f;
            
            while (changeTimer < seasonChangeDuration)
            {
                // Percentage = currentTimer / maxDuration
                changeTimer += Time.deltaTime;
                percentage = changeTimer / seasonChangeDuration;
                groundMaterial.color = Color.Lerp(start, end, percentage);

                yield return null;
            }
        }

        yield return null;
    }

    private void LoadBuildings()
    {
        meshRenderers = new Dictionary<Building, MeshRenderer[]>();

        foreach (var building in allBuildings)
        {
            var b = Instantiate(building.Prefab, new Vector3(0, -50, 0), Quaternion.identity, transform);

            var meshes = b.transform.GetComponentsInChildren<MeshRenderer>();

            foreach (var meshRenderer in meshes)
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

        if (Physics.OverlapBox(currentBounds.center, currentBounds.extents, Quaternion.identity, blockingLayer).Length > 1)
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

        var spawnedBuilding = Instantiate(allBuildings[idSelected].Prefab, InputHandler.Instance.hitInfo.point, Quaternion.identity, transform);

        // Potential rotation iprovement: 
        // Cast rays down from each corner of the building (we already have bounds)
        // Get normal from each hit
        // Get normal median and set up vector accordingly to median

        selectableBuildings[idSelected].gameObject.SetActive(false);
        placedBuildings.Add(spawnedBuilding);

        spawnedBuilding.transform.localRotation = Quaternion.Euler(spawnedBuilding.transform.rotation.x, currentRotation, spawnedBuilding.transform.rotation.z);
        spawnedBuilding.transform.rotation = Helper.SetUpAndYAxisRotation(spawnedBuilding.transform.forward, InputHandler.Instance.hitInfo.normal);

        SelectBuilding(-1);
        currentRotation = 0;

        InputHandler.Instance.SetBuildMode(false);
    }

    public void Rotate(int direction)
    {
        if (idSelected < 0) return;

        currentRotation += direction * rotationStep;

        selectableBuildings[idSelected].transform.eulerAngles = new Vector3(selectableBuildings[idSelected].transform.localRotation.x, currentRotation,
                                                                            selectableBuildings[idSelected].transform.localRotation.z);
    }
}
