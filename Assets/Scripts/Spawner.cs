using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    // True: Will start spawning enemies as soon as the game starts
    // Does not stop for anything

    // False: Only starts if the player enters the box collider
    // Stops if the player leaves the box collider or the player dies
    [SerializeField] private bool bAlwaysSpawn;

    // Size of the box collider used when the game starts
    // only use this to addjust the size of the box collider
    [SerializeField] private int iTriggerSize = 1;
    [SerializeField] private float fTimeBetweenWaves;
    [SerializeField] private float fSpawnInterval;
    [SerializeField] private int iNumOfEnemies;
    [SerializeField] private GameObject[] _enemies;

    // List of possable spawn points
    // filled out on start gets all the children gameobjects of this gameobject
    // all spawn points must be a child of this game object to be used as spawn points

    // Spawn points do not need to be in the box collider area
    // they can be placed anywhere in the level

    // Spawn points do not need to be set in the inspector
    [SerializeField] private Transform[] _spawnPoints;

    private BoxCollider _trigger;
    private int iEnemiesSpawned;

    private Coroutine spawnFunction;

    private void Start() 
    {
        _trigger = GetComponent<BoxCollider>();
        _trigger.size = new Vector3(iTriggerSize, 1, iTriggerSize);

        // gets all the spawn points 
        _spawnPoints = new Transform[transform.childCount];

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPoints[i] = transform.GetChild(i).transform;
        }

        // starts spawning enemies
        // disables the box collider
        if (bAlwaysSpawn)
        {
            StartSpawing();
            _trigger.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            StartSpawing();
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            StopSpawning();
        }
    }

    public void StartSpawing()
    {
        spawnFunction = StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        while(true)
        {
            yield return new WaitForSeconds(fSpawnInterval);

            GameObject enemy = _enemies[Random.Range(0, _enemies.Length)];
            Transform place = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

            Instantiate(enemy, place.position, place.rotation);

            iEnemiesSpawned++;

            if (iEnemiesSpawned == iNumOfEnemies)
            {
                yield return new WaitForSeconds(fTimeBetweenWaves);
                iEnemiesSpawned = 0;
            }
        }
    }

    public void StopSpawning()
    {
        if (bAlwaysSpawn || spawnFunction == null)
        {
            return;
        }

        StopCoroutine(spawnFunction);
    }

    public void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(iTriggerSize, 1, iTriggerSize));    
    }
}
