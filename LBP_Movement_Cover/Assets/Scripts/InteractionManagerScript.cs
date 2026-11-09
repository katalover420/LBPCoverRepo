using System.Collections.Generic;
using UnityEngine;

public class InteractionManagerScript : MonoBehaviour
{
    public SackboyController sackboy;
    public List<GameObject> interactions = new List<GameObject>();
   // public MonoBehaviour interactionScript;
    public Iinteraction interactionInterface;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerInteracted()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sackboy = other.GetComponent<SackboyController>();
            for (int i = 0; i < interactions.Count; i++)
            {
                interactionInterface = interactions[i].GetComponent<Iinteraction>();
                interactionInterface.interactionPlay();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        sackboy = null;
        for (int i = 0; i < interactions.Count; i++)
        {
            interactionInterface = interactions[i].GetComponent<Iinteraction>();
            interactionInterface.interactionStop();
        }
    }
}
