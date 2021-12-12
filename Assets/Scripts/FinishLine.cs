using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lapSign;
    public GameObject finishSign;

    private GameManager gm {
        get {
            return GameManager.instance;
        }
    }
    void Start()
    {
        var gm = GameManager.instance;
        //gm.loadTrackEvents.Add(TrackLoaded);
        RefreshSign();
    }

    public void RefreshSign(){
        if(gm.gameInfo.track.lapsAmount - gm.gameInfo.laps - 1 >= 1){
            lapSign.SetActive(true);
            finishSign.SetActive(false);
        }else{
            lapSign.SetActive(false);
            finishSign.SetActive(true);
        }
    }
    public void OnTriggerEnter(Collider other){
        if(!other.tag.Equals("Player")) return;
        Debug.Log("Crossed the finish line!");
        GameManager.instance.CompletedALap();
        StartCoroutine(RefreshAfter(1f));
    }

    IEnumerator RefreshAfter(float delay){
        yield return new WaitForSeconds(delay);
        RefreshSign();
    }
}
