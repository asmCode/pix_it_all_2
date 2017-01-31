using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IapTest : MonoBehaviour
{

    public Purchaser purchaser;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyClicked()
    {
        // purchaser.BuyNonConsumable();
        purchaser.BuyConsumable();
    }
}
