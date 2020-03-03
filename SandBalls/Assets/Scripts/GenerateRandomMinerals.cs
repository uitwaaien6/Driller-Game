﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomMinerals : MonoBehaviour
{
    int mineralAmount = 60;
    Terrain terr;
    GameObject mineralsParent;
    DrillerPathfinding drillerPathFindindg;

    [SerializeField] GameObject mineral;
    [SerializeField] Material[] materials;
    
    void Start()
    {
        terr = GameObject.Find("Terrain").GetComponent<Terrain>();
        drillerPathFindindg = FindObjectOfType<DrillerPathfinding>(); // todo will serialize it later
        mineralsParent = GameObject.Find("MineralsParent");
        GenerateMinerals();
        ProcessCoroutines();
    }

    public void GenerateMinerals()
    {
        for (int i = 0; i < mineralAmount; i++)
        {
            Vector3 randomTerrainPos = new Vector3();
            randomTerrainPos.x = Random.Range(terr.terrainData.size.x - (terr.terrainData.size.x - 10f), terr.terrainData.size.x - 10f);
            randomTerrainPos.z = Random.Range(terr.terrainData.size.z - (terr.terrainData.size.z - 10f), terr.terrainData.size.z - 10f);
            randomTerrainPos.y = 2.6f;
            GameObject instantiatedMineral = Instantiate(mineral, randomTerrainPos, Quaternion.identity);
            PickRandomMineralColor(instantiatedMineral);
            instantiatedMineral.transform.SetParent(mineralsParent.transform);
        }
    }

    private void PickRandomMineralColor(GameObject instantiatedMineral)
    {
        Material[] materialsToPlace = new Material[instantiatedMineral.GetComponent<MeshRenderer>().materials.Length];
        int ramdonNumber = Random.Range(0, materials.Length);
        Material randomMaterial = materials[ramdonNumber];
        for (int j = 0; j < materialsToPlace.Length; j++) { materialsToPlace[j] = randomMaterial; }
        instantiatedMineral.GetComponent<MeshRenderer>().materials = materialsToPlace;
    }

    private IEnumerator CheckMineralAmount()
    {
        while (true)
        {
            GameObject[] mineralAmountInHierarchy = GameObject.FindGameObjectsWithTag("Mineral");
            if (mineralAmountInHierarchy.Length <= 6)
            {
                GenerateMinerals();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ProcessCoroutines()
    {
        StartCoroutine(drillerPathFindindg.PickRandomPosOrClosestMineral());
        StartCoroutine(CheckMineralAmount());
    }
}