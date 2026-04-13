using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public Transform entrance;
    [SerializeField]
    public Transform exit;

    public float detectionRange = 2f;
    public float exitRange = 2f;
    public float minDistanceFromEntrance = 15f;

    List<NavMeshAgent> agents = new ();

    NavMeshTriangulation triangulation;
    Vector3 entrancePos;

    float detectionRangeSqr;
    float exitRangeSqr;
    float minDistanceSqr;

    bool gameWon = false;
    
    System.Random random = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        entrancePos = entrance.position;
        detectionRangeSqr = detectionRange * detectionRange;
        exitRangeSqr = exitRange * exitRange;
        minDistanceSqr = minDistanceFromEntrance * minDistanceFromEntrance;

        FindAllEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;

        //Detección de captura por enemigo
        bool playerCaught = false;
        
        foreach (var agent in agents)
        {
            if(!agent.enabled) continue;

            if ((agent.transform.position - playerPos).sqrMagnitude < detectionRangeSqr)
            {
                playerCaught = true;
                break;
            }
        }
        if (playerCaught)
        {
            //Si el jugador es atrapado, se reinicia la posición del jugador y de los enemigos
            MovePlayerToEntrance();
            RelocateAllNPC();

            return;
        }

        //Jugador en la salida
        if ((playerPos - exit.position).sqrMagnitude < exitRangeSqr)
        {
            if (!gameWon)
            {
                Debug.Log("Player has reached the exit! You win!");
                gameWon = true;
            }
        }

        //Persecucion
        foreach (var agent in agents)
        {
            if(agent.enabled && !agent.isStopped) 
            {
                agent.SetDestination(playerPos);
            }
        }
    }

    //Metodo para llevar el Player a la entrada
    void MovePlayerToEntrance()
    {
        var cc = player.GetComponent<NavMeshAgent>();

        if (cc != null)
        {
            cc.enabled = false;
        }
        player.transform.position = entrancePos;
        if (cc != null)
        {
            cc.enabled = true;
        }

        Debug.Log("Player moved to entrance: " + entrancePos);
    }

    //Metodo para posicionar a los enemigos
    void RelocateAllNPC()
    {
        if (triangulation.vertices.Length == 0) return;
        
        foreach (var agent in agents)
        {
            agent.enabled = false;
            agent.transform.position = GetValidRandomPosition();
            agent.enabled = true;
        }
    }

    //Metodo para calcular posiciones validas para los enemigos
    Vector3 GetValidRandomPosition()
    {
        Vector3 pos;
        do 
        {
            int i = random.Next(0, triangulation.indices.Length /3) * 3;
            Vector3 v1 = triangulation.vertices[triangulation.indices[i]];
            Vector3 v2 = triangulation.vertices[triangulation.indices[i + 1]];
            Vector3 v3 = triangulation.vertices[triangulation.indices[i + 2]];

            float r1 = (float)random.NextDouble();
            float r2 = (float)random.NextDouble();

            if (r1 + r2 > 1)
            {
                r1 = 1f - r1;
                r2 = 1f - r2;
            }

            pos = v1 + r1 * (v2 - v1) + r2 * (v3 - v1);
        } while (Vector3.SqrMagnitude(pos - entrancePos) < minDistanceSqr);
        return pos;
    }

    //Metodo para ubicar los objetos enemigo
    void FindAllEnemies()
    {
        agents.Clear();
        foreach(var agent in FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None))
        {
            if (agent.CompareTag("Enemy"))
            {
                agents.Add(agent);
            }
        }
    }
}
