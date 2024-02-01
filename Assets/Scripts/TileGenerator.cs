using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public string name;
    public List<GameObject> levels;
}

public class TileGenerator : MonoBehaviour
{
    public List<Level> tilePrefabs;
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnPos = 0;
    [SerializeField] private float tileLength = 100;

    public int spawnType;
    public int spawnCount;

    [SerializeField] private Transform player;
    private int startTiles = 3;

    void Start()
    {
        for (int i = 0; i < startTiles; i++)
        {
            SpawnTile(Random.Range(0, tilePrefabs[spawnType].levels.Count));
        }
    }

    void Update()
    {
        if (player.position.y - 20 > spawnPos - (startTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Count));
            DeleteTile();
        }
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject nextTile = Instantiate(tilePrefabs[spawnType].levels[tileIndex], transform.up * spawnPos, transform.rotation);
        activeTiles.Add(nextTile);
        spawnPos += tileLength;

        spawnCount++;

        if(spawnCount > 6)
        {
            spawnType++;
            if(spawnType > tilePrefabs.Count - 1)
            {
                spawnType = tilePrefabs.Count - 1;
            }
            spawnCount = 0;
        }
    }
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}