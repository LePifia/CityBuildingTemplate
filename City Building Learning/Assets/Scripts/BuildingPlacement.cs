using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBulldozer;

    private BuildingPreset curBuildingPreset;

    private float indicatorUpdateRate = .05f;
    private float lastUpdateRate;
    private Vector3 curIndicatorPos;

    [SerializeField] GameObject placementIndicator;
    [SerializeField] GameObject bulldozerIndicator;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildingPlacement();
        }

        if (Time.time - lastUpdateRate > indicatorUpdateRate)
        {
            lastUpdateRate = Time.time;
            curIndicatorPos = Selector.Instance.GetCurTilePosition();
        }

        if (currentlyPlacing)
        {
            placementIndicator.transform.position = curIndicatorPos;
            currentlyBulldozer = false;
        }
        else if (currentlyBulldozer)
        {
            bulldozerIndicator.transform.position = curIndicatorPos;
            currentlyPlacing = false;
        }
        

        if (Input.GetMouseButtonDown(0) && currentlyPlacing)
        {
            PlaceBuilding();
        }

        if (Input.GetMouseButtonDown(0) && currentlyBulldozer)
        {
            Bulldoze();
        }

    }
    public void BeginNewBuildingPlacement(BuildingPreset preset)
    {
        if (City.Instance.GetCityMoney() < preset.cost)
            return;

        currentlyPlacing = true;
        curBuildingPreset = preset;

        placementIndicator.SetActive(true);
        placementIndicator.transform.position = new Vector3 (0, -99, 0);


    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    public void ToggleBulldozer()
    {
        currentlyBulldozer = !currentlyBulldozer;
        bulldozerIndicator.SetActive(currentlyBulldozer);
        bulldozerIndicator.transform.position = new Vector3(0, -99, 0);
    }

    void PlaceBuilding()
    {
        GameObject buildingObj = Instantiate(curBuildingPreset.prefab, curIndicatorPos, Quaternion.identity);
        City.Instance.OnPlaceBuilding(buildingObj.GetComponent<Building>());

        CancelBuildingPlacement();
    }

    void Bulldoze()
    {
        Building buildingToDestroy = City.Instance.buildings.Find(x => x.transform.position == curIndicatorPos);

        if (buildingToDestroy != null )
        {
            City.Instance.OnRemoveBuilding(buildingToDestroy);
        }
    }
}
