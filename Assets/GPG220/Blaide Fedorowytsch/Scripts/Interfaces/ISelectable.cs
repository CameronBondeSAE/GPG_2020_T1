using System.Collections.Generic;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Interfaces
{
    public interface ISelectable
    {
        /// <summary>
        /// Determines whether or not the unit can currently be selectable, should default to true.
        /// </summary>
        /// <returns> a bool, used by selection system to determine if it can select this unit.</returns>
        bool Selectable(); 
        /// <summary>
        /// used by selection system to determine if the unit can be selected as part of a group.
        /// </summary>
        /// <returns> a bool, used by selection system to determine if the unit can be selected as part of a group.</returns>
        bool GroupSelectable();
        /// <summary>
        /// Called by the selection Manager when the unit has been selected.
        /// </summary>
        void OnSelected( List<ISelectable> selectionGroup);

        void OnSelected();
        void OnDeSelected();
        void OnExecuteAction( Vector3 clickedWorldPosition, GameObject clickedGameObject);
    }
}
