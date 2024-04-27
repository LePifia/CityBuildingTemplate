using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] BuildingPreset preset;

    public int GetCost()
    {
        return preset.cost;
    }

    public int GetCostPerTurn()
    {
        return preset.costPerTurn;
    }

    public GameObject GetPrefab()
    {
        return preset.prefab;
    }

    public int GetPopulation()
    {
        return preset.population;
    }

    public int GetJobs()
    {
        return preset.jobs;
    }

    public int GetFood()
    {
        return preset.food;
    }

}
