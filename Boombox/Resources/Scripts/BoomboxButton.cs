using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomboxButton : MonoBehaviour
{
    public float amt = 10;
    public BBtypes ButtonType;
    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("HandTag")) return;

        SongManager mg = GetComponentInParent<SongManager>();

        switch (ButtonType)
        {
            case BBtypes.Rewind:
                mg.Rewind(); break;
            case BBtypes.FastForward:
                mg.FastForward(); break;
            case BBtypes.Pause:
                mg.Pause(); break;
            case BBtypes.Resume:
                mg.Play(); break;
        }
    }

    public enum BBtypes
    {
        Rewind,
        FastForward,
        Pause,
        Resume
    }
}