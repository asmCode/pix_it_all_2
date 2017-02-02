using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasedBundles
{
    private PurchasedBundlesData m_data;

    public void Load()
    {
        var path = Paths.GetPurchasedBundlesDataPath();
        m_data = JsonLoader.LoadFromFile<PurchasedBundlesData>(path);
        if (m_data == null)
            m_data = new PurchasedBundlesData();
    }

    public bool IsPurchased(string productId)
    {
        if (m_data == null || m_data.ProductIds == null)
            return false;

        return Array.Exists(m_data.ProductIds, (t) => { return t == productId; });
    }

    public void SetPurchased(string productId)
    {
        if (m_data == null)
            return;

        if (IsPurchased(productId))
            return;

        int newSize = m_data.ProductIds != null ? m_data.ProductIds.Length + 1 : 1;
        Array.Resize(ref m_data.ProductIds, newSize);
        m_data.ProductIds[m_data.ProductIds.Length - 1] = productId;

        Save();
    }

    private void Save()
    {
        var path = Paths.GetPurchasedBundlesDataPath();
        JsonLoader.SaveToFile(path, m_data);
    }
}
