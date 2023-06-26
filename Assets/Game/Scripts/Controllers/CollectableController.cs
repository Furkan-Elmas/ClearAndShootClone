using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;

public class CollectableController : MonoBehaviour
{
    private P3dChannelCounterEvent channelCounterEvent;
    private P3dChannelCounter channelCounter;


    private void Awake()
    {
        channelCounter = GetComponent<P3dChannelCounter>();
        channelCounterEvent = GetComponent<P3dChannelCounterEvent>();
    }

    private void Update()
    {
        print(channelCounterEvent.Ratio);
    }

    private void OnDisable()
    {
        
    }

    private void CanBeCollected()
    {

    }
}
