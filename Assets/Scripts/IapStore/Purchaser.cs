using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[Serializable]
public class Receipt
{
    public string Store;
    public string TransactionID;
    public string Payload;
}

//public class Receipt
//{
//    public string Store;
//    public string TransactionID;
//    public string Payload;
//}

public class Purchaser : IStoreListener
{
    private IStoreController m_storeController;
    private IExtensionProvider m_storeExtensionProvider;

    private bool m_isInitializing = false;
    private bool m_isPurchasing = false;
    private bool m_isRestoring = false;

    public event Action<bool, string> PurchaseFinished;
    public event Action<bool> InitializationFinished;

    public bool IsRestoreAvailable()
    {
        return GameSettings.IsRestoreAvailable;
    }

    public bool IsInitializing()
	{
		return m_isInitializing;
	}
	
	public bool IsInitialized()
    {
        return m_storeController != null && m_storeExtensionProvider != null;
    }

    public bool IsPurchasing()
    {
        return m_isPurchasing;
    }

    public bool IsRestoring()
    {
        return m_isRestoring;
    }

    public bool IsReady()
    {
        return
            IsInitialized() &&
            !IsPurchasing() &&
            !IsRestoring();
    }

    public void InitializePurchasing()
    {
		if (IsInitialized() || IsInitializing())
			return;

        m_isInitializing = true;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		
		foreach (var itemId in IapIds.Ids)
			builder.AddProduct(itemId, ProductType.NonConsumable);

        builder.AddProduct("test.pixitallfree.consume_01", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProductId(string productId)
    {
		if (!IsReady())
			return;
		
		Product product = m_storeController.products.WithID(productId);
		if (product == null || !product.availableToPurchase)
		{
			Debug.LogErrorFormat("Product not found or not available to purchase: {0)", productId);
			return;
		}

        m_isPurchasing = true;
		
		Debug.LogFormat("Purchasing product asychronously: '{0}'", product.definition.id);
		m_storeController.InitiatePurchase(product);
	}
	
    public void RestorePurchases()
    {
		if (!IsReady())
			return;
		
        if (Application.platform != RuntimePlatform.IPhonePlayer &&
            Application.platform != RuntimePlatform.OSXPlayer)
			return;

        m_isRestoring = true;
        
		Debug.Log("RestorePurchases started ...");

		// Fetch the Apple store-specific subsystem.
		var apple = m_storeExtensionProvider.GetExtension<IAppleExtensions>();
		// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
		// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
		apple.RestoreTransactions((result) =>
		{
            m_isRestoring = false;

            // The first phase of restoration. If no more responses are received on ProcessPurchase then 
            // no purchases are available to be restored.
            Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
		});
    }

    public IapStore.Product GetProductById(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        if (!IsInitialized())
            return null;

        var product = m_storeController.products.WithStoreSpecificID(id);
        if (product == null)
            return null;

        return ConvertProduct(product);
    }

    public IapStore.Product[] GetProducts()
    {
        if (!IsInitialized())
            return null;

        List<IapStore.Product> products = new List<IapStore.Product>();

        foreach (var productId in IapIds.Ids)
        {
            var product = m_storeController.products.WithStoreSpecificID(productId);
            if (product == null)
                continue;

            products.Add(ConvertProduct(product));
        }

        return products.ToArray();
    }

    private IapStore.Product ConvertProduct(Product product)
    {
        return new IapStore.Product(
            product.definition.storeSpecificId,
            product.metadata.localizedPriceString,
            product.hasReceipt);
    }

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_isInitializing = false;

        Debug.Log("OnInitialized: PASS");

        m_storeController = controller;
        m_storeExtensionProvider = extensions;

        if (InitializationFinished != null)
            InitializationFinished(true);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        m_isInitializing = false;

        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);

        if (InitializationFinished != null)
            InitializationFinished(false);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        m_isPurchasing = false;

        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        if (args == null || args.purchasedProduct == null || args.purchasedProduct.definition == null || args.purchasedProduct.metadata == null)
            return PurchaseProcessingResult.Complete;

        var metadata = args.purchasedProduct.metadata;

#if UNITY_IPHONE
        GameAnalyticsSDK.GameAnalytics.NewBusinessEventIOS(
            metadata.isoCurrencyCode,
            (int)metadata.localizedPrice,
            metadata.localizedTitle,
            args.purchasedProduct.definition.id,
            "levels",
            args.purchasedProduct.receipt);
#elif UNITY_ANDROID
        GameAnalyticsSDK.GameAnalytics.NewBusinessEventGooglePlay(
            metadata.isoCurrencyCode,
            (int)metadata.localizedPrice,
            metadata.localizedTitle,
            args.purchasedProduct.definition.id,
            "levels",
            args.purchasedProduct.receipt,
            args.purchasedProduct.receipt);
#endif

        Debug.LogFormat("******************** receipt: {0}", args.purchasedProduct.receipt);

        Receipt receipt = JsonUtility.FromJson<Receipt>(args.purchasedProduct.receipt);

        if (receipt != null)
        {
            Debug.LogFormat("******************** Store: {0}", receipt.Store);
            Debug.LogFormat("******************** TransactionID: {0}", receipt.TransactionID);
            Debug.LogFormat("******************** Payload: {0}", receipt.Payload);
        }
        else
            Debug.LogFormat("******************** receipt is null");

        if (PurchaseFinished != null)
            PurchaseFinished(true, args.purchasedProduct.definition.id);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        m_isPurchasing = false;

        Debug.LogErrorFormat("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason);

        if (PurchaseFinished != null)
            PurchaseFinished(false, product.definition.storeSpecificId);
    }
}