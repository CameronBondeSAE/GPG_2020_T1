using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Luca.Scripts.Resources
{
    /// <summary>
    /// This Scriptable Object class represents a resource type. It stores base data about the resource.
    /// </summary>
    [CreateAssetMenu(fileName = "New Resource", menuName = "Game/Resource")]
    public class ResourceType : SerializedScriptableObject
    {
        private static List<ResourceType> _allResources;
        public static List<ResourceType> GetAllResources()
        {
            if (_allResources == null || _allResources.Count == 0)
            {
                _allResources = UnityEngine.Resources.FindObjectsOfTypeAll<ResourceType>().Where(res => res.loadResource).ToList(); // Kinda Hacky
            }

            return _allResources;
        }


        [SerializeField] private int resourceId = 0; // Info used int instead of a string identifier since string comparison usually are quite performance intensive.
        public int ResourceId => resourceId;

        [SerializeField] private string resourceName = "";
        public string ResourceName => resourceName;

        [SerializeField] private string resourceDescription = "";
        public string ResourceDescription => resourceDescription;
    
        [SerializeField] private Sprite resourceThumbnail;
        public Sprite ResourceThumbnail => resourceThumbnail;
        
        [SerializeField, PropertyTooltip("If set to true, the property will be loaded in-game.")]
        private bool loadResource = true;
        public bool LoadResource => loadResource;

    }
}
