using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public bool playMenuMusic;

	void Start ()
    {
        if (playMenuMusic)
        {
            SoundtrackManager sm = SoundtrackManager.instance;
            if (sm)
            {
                sm.PlayMenuTheme();
            }
        }	
	}

}
