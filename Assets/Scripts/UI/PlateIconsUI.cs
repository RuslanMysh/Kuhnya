using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKithcenObject plateKithcenObject;
    [SerializeField] private Transform iconTamplate;

    private void Start()
    {
        plateKithcenObject.OnIngredientAdded += PlateKithcenObject_OnIngredientAdded;
    }

    private void PlateKithcenObject_OnIngredientAdded(object sender, PlateKithcenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if(child == iconTamplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKithcenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(iconTamplate, transform);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

}
