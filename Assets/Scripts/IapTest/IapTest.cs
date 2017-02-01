using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IapTest : MonoBehaviour
{
    private Purchaser m_purchaser = new Purchaser();

    public Text m_isInitialized;
    public Text m_isInitializing;
    public Text m_isPurchasing;
    public Transform m_productsContainer;

    public Button m_productPrefab;

    // Use this for initialization
    void Start()
    {
        RefreshStats();
        RefreshPurchases();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        m_purchaser.InitializationFinished += M_purchaser_InitializationFinished;
        m_purchaser.PurchaseFinished += M_purchaser_PurchaseFinished;
    }

    private void OnDisable()
    {
        m_purchaser.InitializationFinished -= M_purchaser_InitializationFinished;
        m_purchaser.PurchaseFinished -= M_purchaser_PurchaseFinished;
    }

    private void M_purchaser_PurchaseFinished(bool arg1, string arg2)
    {
        RefreshStats();
        RefreshPurchases();
    }

    private void M_purchaser_InitializationFinished(bool obj)
    {
        if (obj)
        {
            RefreshStats();
            RefreshPurchases();
        }
    }

    public void UIEvent_Initialize()
    {
        if (m_purchaser.IsInitialized() || m_purchaser.IsInitializing())
            return;

        m_purchaser.InitializePurchasing();
    }

    public void UIEvent_Restore()
    {
        if (m_purchaser.IsInitialized() || m_purchaser.IsInitializing())
            return;

        m_purchaser.RestorePurchases();
    }

    private void RefreshStats()
    {
        m_isInitialized.text = m_purchaser.IsInitialized().ToString();
        m_isInitializing.text = m_purchaser.IsInitializing().ToString();
        m_isPurchasing.text = m_purchaser.IsPurchasing().ToString();
    }

    private void RefreshPurchases()
    {
        foreach (Transform child in m_productsContainer)
        {
            GameObject.Destroy(child.gameObject);
        }


        if (!m_purchaser.IsInitialized())
            return;

        var products = m_purchaser.GetProducts();

        foreach (var product in products)
        {
            Button prod = Instantiate(m_productPrefab, Vector3.zero, Quaternion.identity);
            var label = prod.GetComponentInChildren<Text>();
            label.text = string.Format("id={0}, price={1}, owned={2}", product.Id, product.LocalizedPrice, product.Owned);
            prod.transform.SetParent(m_productsContainer.transform);
            prod.onClick.AddListener(() =>
            {
                m_purchaser.BuyProductId(product.Id);
            });
        }
    }
}
