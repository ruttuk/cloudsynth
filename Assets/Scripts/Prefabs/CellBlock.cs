using UnityEngine;

public class CellBlock : MonoBehaviour
{
    Renderer m_Renderer;
    Color m_OriginalColor;
    Emcee MC;

    public int row { get; set; }
    public int rowIndex { get; set; }

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_OriginalColor = m_Renderer.material.color;
        MC = Emcee.Instance;
    }

    void OnMouseEnter()
    {
        Color highlightedColor = m_OriginalColor;
        highlightedColor.a = 0.33f;
        m_Renderer.material.color = highlightedColor;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MC.getDataJockey().UpdateSong(null, row, rowIndex, "DELETE_BLOCK");
            Destroy(gameObject);
        }
    }

    void OnMouseExit()
    {
        m_Renderer.material.color = m_OriginalColor;
    }
}
