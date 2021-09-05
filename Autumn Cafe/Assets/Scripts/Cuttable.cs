using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    public bool radial;
    public float health;
    public float maxHealth;


    [System.Serializable]
    public class SpawnableEntry
    {
        public GameObject spawnable;
        public float yRotationOffset;
    }

    public List<SpawnableEntry> Spawnables = new List<SpawnableEntry>();
}
