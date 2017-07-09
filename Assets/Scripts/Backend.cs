using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backend : Singleton<Backend>
{
    private const string ReportFirstRunUrl = "http://semiseriousgames.com/pixitall/backend/first_run.php";
    private const string ReportPurchaseUrl = "http://semiseriousgames.com/pixitall/backend/purchase.php";

    public void ReportFirstRun()
    {
        HttpPostRequest.Send(ReportFirstRunUrl, null, null);
    }

    public void ReportPurchase(string storeId, string localizedPrice)
    {
        var message = new ReportPurchaseMessage();
        message.StoreId = storeId;
        message.LocalizedPrice = localizedPrice;

        var json = JsonUtility.ToJson(message);

        HttpPostRequest.Send(ReportPurchaseUrl, json, null);
    }
}
