using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{

    public static TunnelManager TMinstance { get; private set; }

    private static GameManager gm;

    [SerializeField] private float velocity;
    [SerializeField] private GameObject LastTunnel;

    [Header("Planet Tunnel/Objects")]
    //index of the current planet, used to swap what set of arrays are being used for obsticle and tunnel generation
    [SerializeField] private int _currentPlanetIndex;
    [SerializeField] private GameObject _planetChangeTrigger;
    //arrays for all the different planets
    [SerializeField] private GameObject[] _Planet0TunnelPrefabs;
    [SerializeField] private GameObject[] _Planet0ObstaclePrefabs;
    [SerializeField] private GameObject[] _Planet1TunnelPrefabs;
    [SerializeField] private GameObject[] _Planet1ObstaclePrefabs;
    [SerializeField] private GameObject[] _Planet2TunnelPrefabs;
    [SerializeField] private GameObject[] _Planet2ObstaclePrefabs;
    [SerializeField] private GameObject[] _Planet3TunnelPrefabs;
    [SerializeField] private GameObject[] _Planet3ObstaclePrefabs;
    //chance to spawn an obsticle
    [SerializeField] private float chanceObstacle = 0.3f;

    //2D array initialization needs a static int value, This cannot be seen in the inspector
    private static int _numberOfPlanets = 4;

    //2D Arrays Cannot be seen in inspector! Must be assigned in code!
    private GameObject[][] tunnelPrefabs = new GameObject[_numberOfPlanets][];
    private GameObject[][] ObstaclePrefabs = new GameObject[_numberOfPlanets][];
    private void Initialize2DArrays()
    {
        //Assigning the values of the 2d Array
        //Must have one for each number of planets!!
        tunnelPrefabs[0] = _Planet0TunnelPrefabs;
        ObstaclePrefabs[0] = _Planet0ObstaclePrefabs;
        tunnelPrefabs[1] = _Planet1TunnelPrefabs;
        ObstaclePrefabs[1] = _Planet1ObstaclePrefabs;
        tunnelPrefabs[2] = _Planet2TunnelPrefabs;
        ObstaclePrefabs[2] = _Planet2ObstaclePrefabs;
        tunnelPrefabs[3] = _Planet3TunnelPrefabs;
        ObstaclePrefabs[3] = _Planet3ObstaclePrefabs;
    }

    private void Awake()
    {
        // Ensure this is the only instance. If not, destroy self.
        if (TMinstance != null && TMinstance != this) { Destroy(this); }
        else { TMinstance = this; }
        //Check that Last tunnel has been assigned
        if (LastTunnel == null) { Debug.LogError("Assign Last tunnel in inspector!!!"); }

        //Assigning the values of the 2d Array
        Initialize2DArrays();

    }

    void Start()
    {
        //get reference to GameManager
        gm = GameManager.instance;
    }

    void FixedUpdate()
    {
        velocity = gm.getCurrSpeed();
    }

    public void SpawnTunnel(GameObject _TunnelToDelete)
    {
        GameObject nextTunnel = Instantiate(
            tunnelPrefabs[_currentPlanetIndex][Random.Range(0, tunnelPrefabs[_currentPlanetIndex].Length)], //Gameobject to instantiate
            LastTunnel.transform.position + (5 - Time.deltaTime * velocity) * Vector3.forward, //TunnelPosition
            LastTunnel.transform.rotation); //Tunnel Rotation

        if (Random.value < chanceObstacle)
        {
            Instantiate(
                ObstaclePrefabs[_currentPlanetIndex][Random.Range(0, ObstaclePrefabs[_currentPlanetIndex].Length)], //Gameobject to instantiate
                LastTunnel.transform.position //center reference point
                + Random.Range(-1,2) * 5 * Vector3.right + Random.Range(-1, 2) * 5 * Vector3.up, //repositioning logic
                LastTunnel.transform.rotation);
        }
        LastTunnel = nextTunnel;
        //Destroy the tunnel that called the spawner
        Destroy(_TunnelToDelete);
    }

    public IEnumerator ChangePlanets()
    {
        var tempPlanetIndex = _currentPlanetIndex;
        _currentPlanetIndex = 0;
        SpawnTrigger();
        Debug.Log("Change Planets");
        gm.SetIsInSpace(true);
        yield return new WaitForSeconds(3);
        
        

        if (tempPlanetIndex < _numberOfPlanets - 1)
        {
            _currentPlanetIndex = tempPlanetIndex + 1;
        }
        else if (tempPlanetIndex == _numberOfPlanets - 1)
        {
            _currentPlanetIndex = 1;
        }
        SpawnTrigger();

        while (gm.GetIsInSpace() == true)
        {
            yield return null;
        }
        Debug.Log("done Change Planets");
        yield return new WaitForSeconds(3);
    }

    public void ToSpace()
    {
        var tempPlanetIndex = _currentPlanetIndex;
        _currentPlanetIndex = 0;
        return;
    }

    public void SpawnTrigger()
    {
        GameObject nextTunnel = Instantiate(
            _planetChangeTrigger, //Gameobject to instantiate
            LastTunnel.transform.position + (5 - Time.deltaTime * velocity) * Vector3.forward, //TunnelPosition
            LastTunnel.transform.rotation); //Tunnel Rotation
    }
}
