using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DotClouds : MonoBehaviour {
    [SerializeField] GameObject parent;
    public int id;
    static int idCounter = 1;
    Cloud[] clouds;
    public bool solved;
    bool TwitchPlaysActive;
    TextMesh[] labels;
    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXY";
    // Use this for initialization
    void Start()
    {
        id = idCounter++;
        clouds = GetComponentsInChildren<Cloud>();
        labels = parent.GetComponentsInChildren<TextMesh>();
        List<int> puzzle = Enumerable.Range(0, clouds.Length).ToList().Shuffle().Take(7).ToList();
        Debug.LogFormat("[Dot Clouds #{0}] Using clouds {1} and a lone dot from cloud {2}.", id, puzzle.Take(puzzle.Count - 1).OrderBy(x => x).Select(x => clouds[x].index).Join(", "), clouds[puzzle.Last()].index);
        for (int i = 0; i < clouds.Length; i++)
        {
            if (puzzle.Contains(i)) if (i == puzzle[puzzle.Count() - 1]) { clouds[i].Target(); } else clouds[i].Decoy();
            else clouds[i].gameObject.SetActive(false);
        }
        GetComponent<KMBombModule>().OnActivate += delegate
        {
            int counter = 0;
            foreach (TextMesh label in labels)
            {
                label.gameObject.SetActive(TwitchPlaysActive);
                {
                    if (label.gameObject.activeInHierarchy)
                    {
                        label.text = alphabet[counter].ToString();
                        counter++;
                    }
                }
            }
        };
    }
    string TwitchHelpMessage = "!1 A (presses the dot labled A; only the first letter of the command will be parsed)";
    IEnumerator ProcessTwitchCommand(string command)
    {
        char letter = command.Trim().ToUpperInvariant()[0];
        if (alphabet.Contains(letter))
        {
            yield return null;
            GetComponentsInChildren<KMSelectable>().First(x => x.gameObject.GetComponentInChildren<TextMesh>().text.Contains(letter)).OnInteract();
        }
        else
        {
            yield return "sendtochaterror Character " + letter + " invalid!";
            yield break;
        }
    }
}