using System;
using System.Linq;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : Singleton<InputHandler>
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private LayerMask groundLayer;

    private Camera mainCam;
    public RaycastHit GroundHit;

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
            CastRayFromMousePosition(out RessourceSource source);

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
            if (CastRayFromMousePosition(out RessourceSource source))
            {
                if (source == null)
                {
                    UnitManager.Instance.SetPioneerDestination(destination);
                }
                else if (source != null)
                {
                    UnitManager.Instance.HarvestRessource(source);
                }
            }
        }
    }

    bool CastRayFromMousePosition(out RessourceSource source)
    {
        source = null;
        var ray = mainCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, .5f);

        RaycastHit[] rayHits = Physics.RaycastAll(ray, Mathf.Infinity, targetLayers);

        if (rayHits.Length > 0) // Wenn wir etwas hitten müssen wir prüfen, was genau denn da ist und können priorisieren
        {
            for (int i = 0; i < rayHits.Length; i++)
            {
                if (rayHits[i].collider.gameObject.TryGetComponent(out RessourceSource hitSource))
                {
                    source = hitSource;
                    return true;
                }
            }

            /*
             *  Hier könnten Ihre weiteren Prios/Prüfungen stehen!
             */

            // Wenn die Prioritäten nicht erfüllt sind, dann iterieren wir und suchen nach Ground (niedrigste prio)
            for (int i = 0; i < rayHits.Length; i++)
            {
                if (groundLayer.Contains(rayHits[i].collider.gameObject.layer))
                {
                    destination = rayHits[i].point;
                    GroundHit = rayHits[i];
                    Debug.DrawRay(destination, rayHits[i].normal, Color.red, .5f);
                    return true;
                }
            }
        }

        // Nix getroffen dann false
        return false;
    }

    public void SetBuildMode(bool val)
    {
        inBuildMode = val;
    }
}
