using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscData : MonoBehaviour {

    private int m_DiscNum = 0;

    public int GetDiscNum()
    {
        return m_DiscNum;
    }

    public void SetDiscNum(int newNum)
    {
        m_DiscNum = newNum;
    }
}
