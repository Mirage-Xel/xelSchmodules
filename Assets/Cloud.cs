using UnityEngine;
public class Cloud : MonoBehaviour {
    KMSelectable[] dots;
    DotClouds module;
    public string index;
	// Use this for initialization
	void Awake () {
        dots = GetComponentsInChildren<KMSelectable>();
        module = GetComponentInParent<DotClouds>();
    }
	
	// Update is called once per frame
	public void Decoy () {
        foreach (KMSelectable dot in dots)       
            dot.OnInteract += delegate { if (!module.solved)
                {
                    module.GetComponent<KMBombModule>().HandleStrike();
                    Debug.LogFormat("[Dot Clouds #{0}] Pressed a dot from cloud {1}. Strike!", module.id, index);
                }
            return false; };
	}
    public void Target()
    {
        foreach (KMSelectable dot in dots)
            dot.gameObject.SetActive(false);
        KMSelectable solution = dots.PickRandom();
        solution.gameObject.SetActive(true);
        solution.OnInteract += delegate { if (!module.solved)
            {
                module.GetComponent<KMBombModule>().HandlePass();
                module.GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                Debug.LogFormat("[Dot Clouds #{0}] Pressed the lone dot. Module solved", module.id);
                module.solved = true;
            }
            return false; };
    }
}