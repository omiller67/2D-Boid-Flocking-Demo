using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    private Collider2D agentCollider;
    public Collider2D publicAgentCollider { get { return agentCollider; } }

    private Collider collider3D;

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        collider3D = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    public void Move(Vector2 target)
    {
        transform.up = target;
        transform.position += (Vector3)target * Time.deltaTime;
    }
}
