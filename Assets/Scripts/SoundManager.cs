using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] AudioClip[] audioClips;
    private Stack<AudioSource> stack = new Stack<AudioSource>();
    public void PlaySound(Sound index,float volume = 1)
    {
        /*
        if (index == Sound.)
        {
        }
        else if (index == Sound.)
        {
        }
        */

        AudioClip clip = audioClips[(int)index];
        GameObject g;
        AudioSource aud;
        
        if (stack.Count == 0)
        {
            g = new GameObject();
            aud = g.AddComponent(typeof(AudioSource)) as AudioSource;
        }
        else
        {
            aud = stack.Pop();
            aud.gameObject.SetActive(true);
            g = aud.gameObject;
        }
        
        g.name = "[Sound:" + clip.name + "(" + index + ")]";
        float dRand = Random.Range(-0.1f,0.1f);
        g.transform.parent = transform;
        g.transform.position =  Camera.main.transform.position;
        aud.volume = volume*0.2f+dRand*0.1f;
        aud.pitch += dRand;
        aud.clip = clip;
        aud.Play();
        StartCoroutine(DisableSoundAfterPlayed(aud));

    }

    public IEnumerator DisableSoundAfterPlayed(AudioSource aud)
    {
        yield return new WaitForSeconds(aud.clip.length);
        aud.gameObject.SetActive(false);
        stack.Push(aud);
    }

}


public enum Sound
{
    click,
    swoosh,
    pull
}
 