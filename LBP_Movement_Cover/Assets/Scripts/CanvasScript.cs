using UnityEngine;

public class CanvasScript : MonoBehaviour, Iinteraction
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interactionPlay()
    {
        animator.SetBool("played", true);
    }

    public void interactionStop()
    {
        animator.SetBool("played", false);

    }
}
