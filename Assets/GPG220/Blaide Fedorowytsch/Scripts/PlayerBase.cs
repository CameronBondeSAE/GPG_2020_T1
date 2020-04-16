using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts
{
    public class PlayerBase : NetworkBehaviour
    {
        public List<UnitBase> units;
        [SyncVar]
        public Color playerColour;
		[SyncVar]
        public string playerName;

    }
}
