using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emcee : MonoBehaviour
{
    private static Emcee _MC;

    [SerializeField]
    private DataJockey DJ;
    [SerializeField]
    private MainMenu MainMenu;
    [SerializeField]
    private BlockManager BlockManager;

    public BlockManager getBlockManager()
    {
        return BlockManager;
    }

    public DataJockey getDataJockey()
    {
        return DJ;
    }

    public static Emcee Instance { get { return _MC; } }

    private void Awake()
    {
        if (_MC != null && _MC != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _MC = this;
        }
    }
}
