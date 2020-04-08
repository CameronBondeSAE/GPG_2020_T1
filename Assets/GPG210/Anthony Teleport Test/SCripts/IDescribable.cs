using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anthony
{
    public interface IDescribable
    {
        string Name
        {
            get;
        }

        string Description
        {
            get;
        }

        Sprite Image
        {
            get;
            set;
        }
    }
    //Use in class maybe
    /*get { return "UnitName";}
    set {}*/
}

