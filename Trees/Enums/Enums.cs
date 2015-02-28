using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTreesLib
{
    [Serializable]
    public enum TreeState
    {
        RightHeavy,
        LeftHeavy,
        Balanced
    }

    [Serializable]
    public enum TreeTraversalOrder
    {
        InOrder,
        PostOrder,
        PreOrder
    }
}
