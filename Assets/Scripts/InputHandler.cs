using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : Singleton<InputHandler>
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private LayerMask groundLayers;

    private Camera mainCam;
    private RaycastHit[] hitInfos = new RaycastHit[1];

    public RaycastHit hitInfo => hitInfos[0];

    private bool inBuildMode;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        mainCam = Camera.main;
        inBuildMode = false;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (inBuildMode)
        {
            CastRayFromMousePosition();

            if (Input.GetMouseButtonDown(1)) // Right Click
            {
                BuildingsManager.Instance.SelectBuilding(-1);
                SetBuildMode(false);
                return;
            }

            if (Input.GetMouseButtonUp(0)) // Left Click
            {
                BuildingsManager.Instance.PlaceBuilding();
                return;
            }

            if (Input.GetKey(KeyCode.Q))
            {
                BuildingsManager.Instance.Rotate(-1);
            }

            if (Input.GetKey(KeyCode.E))
            {
                BuildingsManager.Instance.Rotate(1);
            }
            //if (Input.GetAxis("Mouse ScrollWheel") != 0)
            //{
            //    BuildingsManager.Instance.Rotate(Input.mouseScrollDelta.y);
            //}

            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (CastRayFromMousePosition())
                UnitManager.Instance.SetPioneerDestination(destination);
        }
    }

    bool CastRayFromMousePosition()
    {
        var ray = mainCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, .5f);

        var hits = Physics.RaycastNonAlloc(ray, hitInfos, Mathf.Infinity, groundLayers);

        if (hits == 1)
        {
            destination = hitInfos[0].point;
            Debug.DrawRay(destination, hitInfos[0].normal, Color.red, .5f);
            return true;
        }

        return false;
    }

    public void SetBuildMode(bool val)
    {
        inBuildMode = val;
    }
}
