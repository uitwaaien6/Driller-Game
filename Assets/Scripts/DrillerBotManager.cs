﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillerBotManager : MonoBehaviour
{
    TerrainDeformer terrainDeformer;
    GenerateRandomMinerals generateRandomMinerals;

    private void Start()
    {
        terrainDeformer = FindObjectOfType<TerrainDeformer>();
        generateRandomMinerals = FindObjectOfType<GenerateRandomMinerals>();
        ProcessCoroutines();
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(UpdateDrillerPathfindingsInOtherScripts());
        StartCoroutine(UpdateDrillerBotsCountAndApplyPathfinding()); // this coroutine has to be started after minerals generated
    }

    public IEnumerator UpdateDrillerBotsCountAndApplyPathfinding()
    {
        while (true)
        {
            DrillerPathfinding[] drillerPathfindings = FindAllDrillerPathfindingsInScene();
            foreach (DrillerPathfinding drillerPathfinding in drillerPathfindings)
            {
                if (!drillerPathfinding.isCoroutineStarted)
                {
                    StartCoroutine(drillerPathfinding.PickRandomPosOrClosestMineral());
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public IEnumerator UpdateDrillerPathfindingsInOtherScripts()
    {
        while (true)
        {
            DrillerPathfinding[] drillerPathFindings = FindAllDrillerPathfindingsInScene();
            terrainDeformer.drillerPathfindings = drillerPathFindings;
            generateRandomMinerals.drillerPathfindings = drillerPathFindings;
            yield return new WaitForSeconds(1f);
        }
    }

    public DrillerPathfinding[] FindAllDrillerPathfindingsInScene()
    {
        DrillerPathfinding[] drillerPathfindings = FindObjectsOfType<DrillerPathfinding>();
        return drillerPathfindings;
    }

    public void ApplyPathfindToAllDrillerPathfindings()
    {
        foreach (DrillerPathfinding drillerPathfinding in FindAllDrillerPathfindingsInScene()) 
        {
            if (!drillerPathfinding.isCoroutineStarted)
            {
                StartCoroutine(drillerPathfinding.PickRandomPosOrClosestMineral());
            }
        }
    }
}