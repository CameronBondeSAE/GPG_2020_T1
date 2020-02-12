using System;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class TestUnitBase : MonoBehaviour, ISelectable
    {
        public UnitSelectionManager usm;

        private void Start()
        {
            usm = FindObjectOfType<UnitSelectionManager>();
        }

        public virtual bool Selectable()
        {
            return true;
        }

        public virtual  bool GroupSelectable()
        {
            return true;
        }

        public virtual void OnSelected()
        {
        }

        public virtual void OnDeSelected()
        {
        
        }

        public virtual void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
        }
    }
}
