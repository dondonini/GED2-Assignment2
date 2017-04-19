using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColorOnStart : MonoBehaviour
{

    private MeshRenderer m_mr;
    private Color defaultColor;

    [SerializeField]
    private Color selectedColor;

    private bool amISelected = false;

    // Use this for initialization
    void Start ()
    {
        selectedColor = Color.black;
        m_mr = gameObject.GetComponent<MeshRenderer>();
        m_mr.material.color = Random.ColorHSV();
        defaultColor = m_mr.material.color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleColor()
    {

        if (amISelected)
        {
            m_mr.material.color = defaultColor;
        }
        else
        {
            m_mr.material.color = selectedColor;
        }

        amISelected = !amISelected;
    }
}
