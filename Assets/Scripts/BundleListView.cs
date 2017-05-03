using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleListView : MonoBehaviour
{
    public BundleView m_bundleViewPrefab;
    public RectTransform m_myCollectionBundlesContainer;
    public RectTransform m_storeBundlesContainer;

    public event System.Action<string> BundleClicked;

    public void SetBundles(BundleViewData[] bundles)
    {
        Clear();

        if (bundles == null)
            return;

        foreach (var bundleViewData in bundles)
        {
            var container = bundleViewData.IsAvailable ? m_myCollectionBundlesContainer : m_storeBundlesContainer;

            var bundleView = Instantiate(m_bundleViewPrefab, container);
            bundleView.transform.localScale = Vector3.one;
            bundleView.SetBundle(bundleViewData);
            bundleView.Clicked += HandleBundleViewClicked;
        }
    }

    public void Clear()
    {
        foreach (Transform child in m_myCollectionBundlesContainer)
            Destroy(child.gameObject);

        foreach (Transform child in m_storeBundlesContainer)
            Destroy(child.gameObject);
    }

    private void HandleBundleViewClicked(string bundleId)
    {
        if (BundleClicked != null)
            BundleClicked(bundleId);
    }
}
