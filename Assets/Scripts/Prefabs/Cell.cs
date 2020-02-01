using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    Vector3 m_Position;
    Renderer m_Renderer;
    Color m_OriginalColor;
    Emcee MC;

    public int row { get; set; }
    public int rowIndex { get; set; }

    void Start()
    {
        m_Position = transform.position;
        m_Renderer = GetComponent<Renderer>();
        m_OriginalColor = m_Renderer.material.color;
        MC = Emcee.Instance;
    }

    void OnMouseEnter()
    {
       m_Renderer.material = Resources.Load<Material>("Materials/Blocks/" + MC.getBlockManager().getCurrentlySelectedBlock().getMaterial());
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                m_Renderer.material.color = Color.green;
                if (MC.getDataJockey().IsSongLoaded())
                {
                    Block current = MC.getBlockManager().placeCellBlock(m_Position, row, rowIndex);
                    MC.getDataJockey().UpdateSong(current, row, rowIndex, "ADD_BLOCK");
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            //m_Renderer.material = Resources.Load<Material>("Materials/Blocks" + MC.getBlockManager().GetCurrentlySelected().getMaterial());
        }
    }

    void OnMouseExit()
    {
        m_Renderer.material.color = m_OriginalColor;
    }
}
