using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using UnityEngine;

public class UnitTest : GPG220.Luca.Scripts.Unit.MovableUnit, ISelectable
{
    public bool Selectable()
    {
        return true;
    }

   public  bool GroupSelectable()
   {
       return true;
   }

    //  int SelectionPriority();
    public void OnSelected()
    {
    }

    public void OnDeSelected()
    {
        
    }

    public void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        Debug.Log("I" + gameObject + " would do my action on :" + g +" at :" + worldPosition );
    }

}
