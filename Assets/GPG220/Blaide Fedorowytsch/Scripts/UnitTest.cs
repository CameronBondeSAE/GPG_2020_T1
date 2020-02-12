using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using UnityEngine;

public class UnitTest : GPG220.Luca.Scripts.Unit.MovableUnit, ISelectable
{

    public bool moving = false;
    public Vector3 target;
    
    
    
    public bool Selectable()
    {
        return true;
    }

   public  bool GroupSelectable()
   {
       return true;
   }

    public void OnSelected()
    {
    }

    public void OnDeSelected()
    {
        
    }

    public void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        moving = true;
        target = worldPosition;

    }

    void Move(Vector3 v)
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, v, 0.5f);
    }

    void FixedUpdate()
    {
        if (moving)
        {
            if (Vector3.Distance(this.gameObject.transform.position, target) > 0.1f)
            {
                Move(target);
            }
            else
            {
                moving = false;
            }
        }
    }

}
