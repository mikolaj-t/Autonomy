using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    public CollectibleType type;
    public TextMeshPro speedText;
    public bool speedTrap;
    public float speedValue;
    public bool hasBeenCollected;
    void Start()
    {
        if(speedTrap){
            speedText.text = speedValue + "<size=3.5>km/h</size>";
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other){
        if(!other.tag.Equals("Player")) return;
        if(hasBeenCollected) return;
        if(speedTrap && Car.instance.rb.velocity.magnitude * Car.DISPLAY_SPEED_MULTIPLIER < speedValue) return;
        Debug.Log("Collectible collected");
        hasBeenCollected = true;
        GameManager.instance.CollectedCollectible();
        Destroy(this.gameObject, 1f);
    }
}
