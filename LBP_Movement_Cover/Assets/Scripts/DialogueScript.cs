using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueScript : MonoBehaviour, Iinteraction
{
    public string dialogueText;
    public TMP_Text dialogueTMP;
    public float lerpTime;
    public Vector3 scaleBeginning;
    public Vector3 scaleEnd;

    public bool play;
    public bool stop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueTMP.text = dialogueText;
        //StartCoroutine(dialogueSpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void interactionPlay()
    {
        Debug.Log("interaction played");
            StartCoroutine(dialogueSpawnCoroutine());
        
        
    }
    public void interactionStop()
    {
        Debug.Log("interaction stopped");

            StartCoroutine(dialogueExitCoroutine());
        
    }

    public IEnumerator dialogueSpawnCoroutine()
    {
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, scaleEnd, time);
            yield return null;
        }
    }
    public IEnumerator dialogueExitCoroutine()
    {
        float time = 0;
        while (time < lerpTime)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, scaleBeginning, time);
            yield return null;
        }
    }
}
