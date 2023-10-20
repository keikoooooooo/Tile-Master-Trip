using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Tile tilePrefab;


    public int maxSpawn;
    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var temp = maxSpawn;
        while (temp > 0)
        {
            temp -= 1;
            
            var tile = Instantiate(tilePrefab, RandPosition(), quaternion.identity);
            TileData.Add(tile.collider, tile);
            
            yield return new WaitForSeconds(.2f);
        }
    }

    private Vector3 RandPosition()
    {
        return transform.position + new Vector3(Random.Range(-.1f, .1f),
            Random.Range(-.1f, .1f), Random.Range(-.1f, .1f));
    }
    
    
}
