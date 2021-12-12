using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody;
    private TextMeshProUGUI text;
    private const float speedMultiplier = 5f;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // prev extra light
        var displaySpeed = (int)(carRigidbody.velocity.magnitude * Car.DISPLAY_SPEED_MULTIPLIER);
        text.text = "<font-weight=700><i>" + displaySpeed + "<size=36>km/h</size>";
        text.fontSize = Mathf.Lerp(50f, 100f, Mathf.Min(displaySpeed/100f, 1f));
    }
}
