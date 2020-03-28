using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuContextDemo : MonoBehaviour
{
    [ContextMenuItem("Randomize Name", "RandomizeString")]
    public string Name;

    //these strings control the name of the pop up and what
    //function to call when the pop up is pressed
    [ContextMenuItem("Randomize Level", "RandomizeInt")]
    public int level;

    
    
    private void RandomizeString()
    {
        Name = "Some Random Name";
        
    }

    private void RandomizeInt()
    {
        level = Random.Range(1, 10000);
    }
    
    
}
