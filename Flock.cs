using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [Header("Flock Attributes")]
    public FlockAgent agentPrefab;
    public List<FlockAgent> globalAgents = new List<FlockAgent>();
    public FlockBehavior behavior;

    [Range(10, 500)]
    public int flockSize = 250;
    public const float agentDensity = 0.08f;

    [Header("Flock Behavior Variables")]

    [Range(1f, 100f)]
    public float driveFactor = 10f;

    [Range(1f, 100f)]
    public float maxSpeed = 5f;

    [Range(1f, 10f)]
    public float neighborRadius = 1.5f;

    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;
    public float PublicSquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i = 0; i < flockSize; i++) 
        {
            FlockAgent newAgent = Instantiate(agentPrefab,
                                              Random.insideUnitCircle * flockSize * agentDensity,
                                              Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)),
                                              transform);
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            globalAgents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in globalAgents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;

            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }

            agent.Move(move);

        }
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);

        foreach(Collider2D coll in contextColliders)
        {
            if(coll != agent.publicAgentCollider)
            {
                context.Add(coll.transform);
            }
        }

        return context;
    }
}