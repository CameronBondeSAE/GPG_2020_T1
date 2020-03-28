using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

namespace Pathfinding.Boids
{
    public class Flock : MonoBehaviour
    {
        [Header("Flock Setup")]
        public FlockAgent agentPrefab;
        List<FlockAgent> agents = new List<FlockAgent>();
        public FlockBehaviour behaviour;

        [Range(10, 500)] public int startingCount = 100;
        
        //change to determine agent spawn density
        //lower is closer
        private const float AgentDensity = 0.8f;

        [Header("Agent Stats")] 
        [Range(1f, 100f)] 
        public float driveFactor = 10f;
        [Range(1f, 100f)] 
        public float maxSpeed = 5f;
        [Range(1f, 10f)] 
        public float neightbourRadius = 3f;
        [Range(0f, 1f)] 
        public float avoidanceRadiusMultiplier = 0.5f;

        private float squareMaxSpeed;
        private float squareNeighbourRadius;
        private float squareAvoidanceRadius;

        public float SquareAvoidanceRadius
        {
            get { return squareAvoidanceRadius; }
        }
        private void Start()
        {
            squareMaxSpeed = maxSpeed * maxSpeed;
            squareNeighbourRadius = neightbourRadius * neightbourRadius;
            squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

            for (int i = 0; i < startingCount; i++)
            {
                Vector3 newPos = Random.insideUnitSphere * startingCount * AgentDensity;
                FlockAgent newAgent = Instantiate(agentPrefab, new Vector3(newPos.x,transform.position.y + 1,newPos.z),
                    Quaternion.Euler(Vector3.up * Random.Range(0f, 360)), transform);
                newAgent.name = "Agent " + i;
                newAgent.Initialize(this);
                agents.Add(newAgent);
            }
        }

        private void Update()
        {
            //TODO : Move to seperate function later    
            foreach (FlockAgent agent in agents)
            {
                List<Transform> context = GetNearbyObjects(agent);
                Vector3 move = behaviour.CalculateMove(agent, context, this);
                move *= driveFactor;
                if (move.sqrMagnitude > squareMaxSpeed)
                {
                    move = move.normalized * maxSpeed;
                }
                //comment the below vector if you want flock to move up and down as well
                move = new Vector3(move.x, transform.position.y,move.z);
                agent.Move(move);
            }
        }

        private List<Transform> GetNearbyObjects(FlockAgent agent)
        {
            List<Transform> context = new List<Transform>();
            Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neightbourRadius);
            foreach (Collider c in contextColliders)
            {
                if (c != agent.AgentCollider)
                {
                    context.Add(c.transform);
                }
            }

            return context;
        }


    }
}
