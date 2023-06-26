using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GenerationManager gm = FindObjectOfType<GenerationManager>();

        gm.WinGame();
    }
}
