using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("Attributes")]
    [SerializeField] private GameObject[] towerPrefab;

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
    }

    public GameObject GetSelectedTower()
    {
        return towerPrefab[selectedTower];
    }
}
