using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLevelUp : MonoBehaviour
{
 public int Kills;
 public Material[] material;
 private Renderer rend;

 public void Start()
 {
  Kills = 0;
  rend = GetComponent<Renderer>();
  rend.enabled = true;
  rend.sharedMaterial = material[0];
 }

 public void Update()
 {
  if (Kills <= 3)
  {
   // set colour to red
   Debug.Log("Base Level (0)");
   rend.sharedMaterial = material[0];
  }


  if (Kills >= 4 && Kills <= 11)
  {
   // set colour to blue
   Debug.Log("Level 1");
   rend.sharedMaterial = material[1];
  }
  
  
  
  
  if (Kills >= 12)
  {
   //set colour to yellow
   Debug.Log("Level 2 (Max Level)");
   rend.sharedMaterial = material[2];
  }
 }
}
