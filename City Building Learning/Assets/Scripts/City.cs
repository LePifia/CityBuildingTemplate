using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class City : MonoBehaviour
{
    public static City Instance { get; private set; }

    [SerializeField] int money;
    [SerializeField] int day;
    [SerializeField] int curPopulation;
    [SerializeField] int curJobs;
    [SerializeField] int curFood;
    [SerializeField] int maxPopulation;
    [SerializeField] int incomePerJob;

    [SerializeField] TextMeshProUGUI moneyDisplay;
    [SerializeField] TextMeshProUGUI dayDisplay;
    [SerializeField] TextMeshProUGUI populationDisplay;
    [SerializeField] TextMeshProUGUI jobsDisplay;
    [SerializeField] TextMeshProUGUI foodDisplay;

    public List<Building> buildings = new List<Building>();

    public int GetCityMoney()
    {
        return money;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpdateStatText();
    }
    public void OnPlaceBuilding(Building building)
    {
        money -= building.GetCost();

        maxPopulation += building.GetPopulation();
        curJobs += building.GetJobs();
        curFood += building.GetFood();

        buildings.Add(building);

        UpdateStatText();
    }

    public void OnRemoveBuilding(Building building)
    {
        maxPopulation -= building.GetPopulation();
        curJobs -= building.GetJobs();

        buildings.Remove(building);
        Destroy(building.gameObject);

        UpdateStatText();
    }

    void UpdateStatText()
    {
        moneyDisplay.text = ("Money: " + money);
        dayDisplay.text = ("Day: " + day);
        populationDisplay.text = ("Pop: " + curPopulation + (" / ") + maxPopulation);
        jobsDisplay.text = ("Jobs: " + curJobs);
        foodDisplay.text = ("Food: " + curFood);
    }

    public void EndTurn()
    {
        day++;
        CalculateMoney();
        CalculateJobs();
        CalculateFood();
        CalculatePopulation();

        UpdateStatText() ;
    }

    void CalculateMoney()
    {
        money += curJobs * incomePerJob;

        foreach(Building building in buildings)
        {
            money -= building.GetCostPerTurn();
        }
    }
    void CalculatePopulation()
    {
        if (curFood >= curPopulation && curPopulation < maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood/4), maxPopulation);
        }
        else if (curFood < curPopulation)
        {
            curPopulation = curFood;
        }
    }

    void CalculateJobs()
    {
        curJobs = Mathf.Min(curPopulation, curJobs);
    }
    void CalculateFood()
    {
        curFood = 0;

        foreach(Building building in buildings)
        {
            curFood += building.GetFood();
        }
    }
}
