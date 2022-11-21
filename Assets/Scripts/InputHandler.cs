using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private LayerMask groundLayers;
    
    private Camera mainCam;
    private RaycastHit[] hitInfos = new RaycastHit[1];
    
    private void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(CastRayFromMousePosition())
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
            Debug.DrawRay(destination, Vector3.up, Color.red, .5f);
            Debug.Log(destination);
            return true;
        }

        return false;
    }
}
