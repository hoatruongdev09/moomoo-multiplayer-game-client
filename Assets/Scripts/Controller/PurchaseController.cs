using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
public class PurchaseController : MonoBehaviour, IStoreListener {
    private IStoreController controller;
    private IExtensionProvider provider;
    private void Start () {
        if (controller == null) {
            Initialize ();
        }
    }
    private void Initialize () {
        if (IsInitialized ()) {
            return;
        }
        var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
        builder.AddProduct ("100_gold_coins", ProductType.Consumable, new IDs { { "100_gold_coins_google", GooglePlay.Name }, { "100_gold_coins_mac", MacAppStore.Name }, { "100_gold_coins_ios", AppleAppStore.Name }
        });
        UnityPurchasing.Initialize (this, builder);
    }
    private bool IsInitialized () {
        return controller != null && provider != null;
    }
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions) {
        this.controller = controller;
        provider = extensions;
    }

    public void OnInitializeFailed (InitializationFailureReason error) {
        Debug.LogError ($"In App Purchase initialize failed: {error.ToString()}");
    }

    public void OnPurchaseFailed (Product i, PurchaseFailureReason p) {
        Debug.Log ($"Purchase {i.definition.id} Failed: {p.ToString()}");
    }

    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e) {
        return PurchaseProcessingResult.Complete;
    }
}