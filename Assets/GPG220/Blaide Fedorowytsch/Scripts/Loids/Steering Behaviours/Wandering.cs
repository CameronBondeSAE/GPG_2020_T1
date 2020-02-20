using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Loids.Steering_Behaviours
{
    public class Wandering : SteeringBehaviourBase
    {
        public float forceMultiplier = 1;

        public float timeScale = 1;
    
        // Start is called before the first frame update


        // Update is called once per frame
        void FixedUpdate()
        {
            float x, y, z;
            x = Mathf.PerlinNoise(0.1f, Time.time * timeScale);
            y = Mathf.PerlinNoise(0.2f, Time.time * timeScale);
            z = Mathf.PerlinNoise(0.3f, Time.time * timeScale);
            rB.AddTorque( new Vector3(x,y,z) * forceMultiplier);
        }
    }
}
