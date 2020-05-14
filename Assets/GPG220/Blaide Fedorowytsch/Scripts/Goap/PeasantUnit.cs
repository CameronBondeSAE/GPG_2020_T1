using System;
using GPG220.Luca.Scripts.Resources;
using GPG220.Luca.Scripts.Unit;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Goap
{
    public class PeasantUnit : UnitBase
    {
        private UnitBase king;
        private void Start()
        {
            Initialize();
            health.deathEvent += HealthOndeathEvent;
            inventory.ResQuantityChangedEvent += SendToKing;
            king = owner.GetComponent<HumanPlayer>().king;
        }

        private void HealthOndeathEvent(Health obj)
        {
            Destroy(gameObject);
        }

        public override bool Selectable()
        {
            return false;
        }

        public override bool GroupSelectable()
        {
            return false;
        }

        public void SendToKing(Inventory inventory1, ResourceType resourceType, int amtChange)
        {
            inventory.RemoveResources(resourceType, amtChange);
            king.inventory.AddResources(resourceType, amtChange);
        }

    }
}
