using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Больше одного DeliveryCounter!");
        }
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKithcenObject plateKithcenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKithcenObject);

                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
