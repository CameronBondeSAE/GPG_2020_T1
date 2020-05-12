using System;
using GPG220.Luca.Scripts.Unit;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Goap
{
    public class PeasantUnit : UnitBase
    {
        private void Start()
        {
            Initialize();
            health.deathEvent += HealthOndeathEvent;
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

    }
}
