using UnityEngine;
[CreateAssetMenu()]
public class CurringRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
