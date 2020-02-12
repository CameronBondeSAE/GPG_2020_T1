using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using ISelectable = GPG220.Blaide_Fedorowytsch.Scripts.Interfaces.ISelectable;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class UnitTest : TestUnitBase
    {

        public bool moving = false;
        public Vector3 target;
        public List<ISelectable> selctionGroup;

        public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
        {
            moving = true;
            target = worldPosition;
        }

        public override void OnSelected()
        {
            selctionGroup = usm.selectedIselectables;
        }

        void Move(Vector3 v)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
        }

        Vector3 OffsetPosition()
        {
            Vector3 total = new Vector3();

            foreach (ISelectable s in selctionGroup)
            {
                total += ((MonoBehaviour) s).gameObject.transform.position;
            }

            return transform.position - total / selctionGroup.Count;
        }


        void FixedUpdate()
        {
            if (moving)
            {
                if (Vector3.Distance(this.gameObject.transform.position, target + OffsetPosition()) > 0.1f)
                {
                    Move(target + OffsetPosition());
                }
                else
                {
                    moving = false;
                }
            }
        }

    }
}
