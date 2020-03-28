using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Path = Pathfinding.AStar.New.Path;

namespace DataSaving
{
    public class TestingJson : MonoBehaviour
    {
        [SerializeField]
        private new string name;
        [SerializeField]
        private int level ;
        [SerializeField]
        private float health;

        public InputField nameText;
        public InputField levelText;
        public InputField healthText;
        

        public void CreatePerson()
        {
            name = nameText.text;
            level = int.Parse(levelText.text);
            health = float.Parse(healthText.text);
            
            SavingJson person = new SavingJson();
            person.name = name;
            person.level = level;
            person.health = health;


            string json = JsonUtility.ToJson(person);

            person = JsonUtility.FromJson<SavingJson>(json);
            Debug.Log(json);
            
            //File.WriteAllText(Path.Combine(Application.persistentDataPath, "Person"), json);
        }

    }

}
