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

    public bool IsPurchased(string storeId)
    {
        if (m_data == null || m_data.StoreIds == null)
            return false;

        return Array.Exists(m_data.StoreIds, (t) => { return t == storeId; });
    }

    public void SetPurchased(string storeId)
    {
        if (m_data == null)
            return;

        if (IsPurchased(storeId))
            return;

        int newSize = m_data.StoreIds != null ? m_data.StoreIds.Length + 1 : 1;
        Array.Resize(ref m_data.StoreIds, newSize);
        m_data.StoreIds[m_data.StoreIds.Length - 1] = storeId;

        Save();
    }

    private void Save()
    {
        var path = Paths.GetPurchasedBundlesDataPath();
        JsonLoader.SaveToFile(path, m_data);
    }
}
