using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscValue : MonoBehaviour
{
    [SerializeField]
    private int value;

    public int Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
        }
    }
}
