using UnityEngine;

public class CellBlock : MonoBehaviour
{
    Renderer m_Renderer;
    Color m_OriginalColor;

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_OriginalColor = m_Renderer.material.color;
    }

    void OnMouseEnter()
    {
        Color highlightedColor = m_OriginalColor;
        highlightedColor.a = 0.33f;
        m_Renderer.material.color = highlightedColor;
    }

    void OnMouseExit()
    {
        m_Renderer.material.color = m_OriginalColor;
    }
}
