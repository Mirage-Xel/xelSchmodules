using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class KeypadMasked : MonoBehaviour {
    int id;
    static int idCounter = 1;
    [SerializeField] GameObject[] keyObjs;
    [SerializeField] MeshRenderer[] leds;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Material[] ledMaterials;
    Key[] keys = new Key[4];
    List<int> sortOrder;
    int progress;
    bool globalStriking;

        // Use this for initialization
	void Start () {
        id = idCounter++;
        GenerateKeys();
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = keyObjs[i];
            int dummyplug = i;
            obj.GetComponent<KMSelectable>().OnInteract += delegate
            {
                if (!sortOrder.Take(progress).Contains(dummyplug) && !globalStriking)
                {
                    Debug.LogFormat("[Keypad Masked #{0}] Pressed key {1}.", id, dummyplug + 1);
                    StartCoroutine(KeyAnim(dummyplug, dummyplug != sortOrder[progress]));
                    if (dummyplug == sortOrder[progress])
                    {
                        progress++;
                        Debug.LogFormat("[Keypad Masked #{0}] That was correct.", id);
                        if (progress == 4)
                        {
                            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                            GetComponent<KMBombModule>().HandlePass();
                            Debug.LogFormat("[Keypad Masked #{0}] Module solved.", id);
                        }
                    }
                    else
                    {
                        GetComponent<KMBombModule>().HandleStrike();
                        Debug.LogFormat("[Keypad Masked #{0}] That was incorrect. Strike!", id);
                    }
                }
                return false;
            };
            obj.GetComponentInChildren<SpriteRenderer>().sprite = sprites[keys[i].Sym1];
            obj.GetComponentInChildren<SpriteMask>().sprite = sprites[keys[i].Sym2];
        } 
	}
	
	// Update is called once per frame
	void GenerateKeys () {
        
        List<int> symbols = Enumerable.Range(0, 26).ToList().Shuffle().Take(8).ToList();
        for (int i = 0; i < 8; i += 2)
        {
            keys[i/2] = new Key(symbols[i], symbols[i + 1]);
            Debug.LogFormat("[Keypad Masked #{0}] Key {1} uses symbols {2} and {3}.", id, i / 2 + 1, symbols[i]+1, symbols[i + 1]+1);
        }
        sortOrder = keys.Select(x => keys.OrderBy(y => y).IndexOf(z => z.Equals(x))).ToList();
        Debug.LogFormat("[Keypad Masked #{0}] The order to press the keys is {1}.", id, sortOrder.Select(x => x+1).Join(", "));
	}
    private IEnumerator KeyAnim(int key, bool striking)
    {
        float elapsed = 0f;
        const float duration = 1.5f;
        if (striking)
        {
            leds[key].material = ledMaterials[2];
            globalStriking = true;
        }
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            keyObjs[key].transform.localPosition = new Vector3(0f, Easing.InOutQuart(elapsed, 0f, -0.005f, duration), 0f);
            yield return null;
        }
        // properly set final position
        keyObjs[key].transform.localPosition = new Vector3(0f, -0.005f, 0f);
        if (striking)
        {
            while (elapsed < duration * 1.5)
            {
                elapsed += Time.deltaTime;
                keyObjs[key].transform.localPosition = new Vector3(0f, Easing.InOutQuart(elapsed, -0.005f, 0f, duration*2), 0f);
                yield return null;
            }
            keyObjs[key].transform.localPosition = new Vector3(0f, 0f, 0f);
        }
       leds[key].material = striking ? ledMaterials[0] : ledMaterials[1];
       globalStriking = false;
       yield break;
    }
    private string TwitchHelpMessage = "use '!{0} 1234' to press the keys in reading order.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        string validcmds = "1234";
        if (command.Contains(' '))
        {
            yield return "sendtochaterror @{0}, invalid command.";
            yield break;
        }
        else
        {
            for (int i = 0; i < command.Length; i++)
            {
                if (!validcmds.Contains(command[i]))
                {
                    yield return "sendtochaterror Invalid command.";
                    yield break;
                }
            }
            for (int i = 0; i < command.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (command[i] == validcmds[j])
                    {
                        yield return null;
                        yield return new WaitForSeconds(1f);
                        keyObjs[j].GetComponent<KMSelectable>().OnInteract();
                    }
                }
            }
        }
    }
}
