using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsUIManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingsPanel;
    [SerializeField] private Button[] buildingButtons;

    private void Awake()
    {
        buildingButtons = new Button[buildingsPanel.transform.childCount];
        for (int i = 0; i < buildingsPanel.transform.childCount; i++)
        {
            buildingButtons[i] = 
                buildingsPanel.transform.GetChild(i).GetComponent<Button>();
            
            buildingButtons[i].onClick.AddListener(() =>BuildingsManager.Instance.SelectBuilding(i));
        }
    }
}
