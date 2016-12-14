using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsScene : MonoBehaviour
{
    public BundleListView m_bundleListView;

    private void Awake()
    {
    }

    void Start()
    {
        InitBundleList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitBundleList()
    {
        var bundles = Game.GetInstance().ImageManager.Bundles;
        if (bundles == null)
            return;

        m_bundleListView.SetBundles(bundles);
    }
}
