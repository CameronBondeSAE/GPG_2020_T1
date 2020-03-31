using GPG220.Luca.Scripts.Unit;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    /// <summary>
    /// A basic test unit, action lerps to the returned world position.
    /// 
    /// </summary>
    public class TestUnit : UnitBase
	{
		[SyncVar]
		public int testNumber;
		
        private void Start()
        {
            Initialize();
            health.deathEvent += Die;
			
			InvokeRepeating("MoveTestNumber", 3f, 3f);
        }

		private void MoveTestNumber()
		{
			if (isServer)
			{
				testNumber = Random.Range(1,100);
				// ownerNetID = Random.Range(1, 100);
			}
		}
		
		

		void Update()
        {
            if (isServer)
            {
                RpcMove(transform.position);
            }
        }

        public void Die(Health h)
        {
            Destroy(gameObject);
        }
        
        [ClientRpc]
        public void RpcMove(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}
