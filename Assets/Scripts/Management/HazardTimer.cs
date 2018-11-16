using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardTimer : MonoBehaviour
{
    public List<CageHazard> cageHazards;
    public float time;

    public void CallTimer()
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

	private IEnumerator StartTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            foreach (CageHazard c in cageHazards)
            {
                c.Call();
            }
        }
    }
}
