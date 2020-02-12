using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Interfaces
{
    public interface ISelectable
    {
        bool Selectable(); 
        bool GroupSelectable();
        //  int SelectionPriority();
        void OnSelected();
        void OnDeSelected();
        void OnExecuteAction( Vector3 worldPosition, GameObject g);
  
    }
}
