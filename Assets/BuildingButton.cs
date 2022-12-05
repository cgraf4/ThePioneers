using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private int buildingId;
    private Button _button;
    
    private void Awake()
    {
        buildingId = transform.GetSiblingIndex();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(
            () => BuildingsManager.Instance.SelectBuilding(buildingId));
        _button.onClick.AddListener(
            () => InputHandler.Instance.SetBuildMode(true));
    }
}
