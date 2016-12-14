using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleListView : MonoBehaviour
{
    public BundleView m_bundleViewPrefab;
    public RectTransform m_bundlesContainer;

    public event System.Action<string> BundleClicked;

    public void SetBundles(BundleData[] bundles)
    {
        Clear();

        if (bundles == null)
            return;

        foreach (var bundleData in bundles)
        {
            var bundleView = Instantiate(m_bundleViewPrefab, m_bundlesContainer);
            bundleView.transform.localScale = Vector3.one;
            bundleView.SetBundle(bundleData);
            bundleView.Clicked += HandleBundleViewClicked;
        }
    }

    public void Clear()
    {
        foreach (Transform child in m_bundlesContainer)
            Destroy(child.gameObject);
    }

    private void HandleBundleViewClicked(string bundleId)
    {
        if (BundleClicked != null)
            BundleClicked(bundleId);

        Debug.LogFormat("clicked {0}", bundleId);
    }
}
