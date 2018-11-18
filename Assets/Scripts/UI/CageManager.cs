using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageManager : MonoBehaviour {

    [System.Serializable]
    public struct CageInfo
    {
        public GameObject cage;
        public HazardTimer timer;
    }
    public CageInfo[] cages;

	void Start ()
    {
        int index = Match.cageID;
        index %= cages.Length;
        for(int i = 0; i < cages.Length; i++)
        {
            if(index == i) {
                cages[i].cage.SetActive(true);
                //if(cages[i].timer) cages[i].timer.CallTimer();
            } else {
                cages[i].cage.SetActive(false);
            }
        }
	}
	
    public void CallTimer()
    {
        int index = Match.cageID;
        index %= cages.Length;
        if (cages[index].timer) cages[index].timer.CallTimer();
    }

}
