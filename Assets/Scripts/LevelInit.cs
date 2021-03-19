using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelInit : MonoBehaviour
{
    public GameManager gm;
    public List<Spawner> spawners;
    
    private void Awake()
    {
        gm.spawners = spawners;
    }
}