using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DotClouds : MonoBehaviour {
    public int id;
    static int idCounter = 1;
    Cloud[] clouds;
    public bool solved;
	// Use this for initialization
	void Start () {
        id = idCounter++;
        clouds = GetComponentsInChildren<Cloud>();
        List<int> puzzle = Enumerable.Range(0, clouds.Length).ToList().Shuffle().Take(7).ToList();
        Debug.LogFormat("[Dot Clouds #{0}] Using clouds {1} and a lone dot from cloud {2}.", id, puzzle.Take(puzzle.Count-1).Select(x => x+1).Join(", "), puzzle.Last()+1);
        for (int i = 0; i < clouds.Length; i++)
        {
            if (puzzle.Contains(i)) if (i == puzzle[puzzle.Count()-1]) { clouds[i].Target(); } else clouds[i].Decoy();
            else clouds[i].gameObject.SetActive(false);
        }
	}
}
