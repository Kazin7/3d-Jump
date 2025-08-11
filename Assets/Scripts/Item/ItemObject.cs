using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        UseItem();
        Destroy(gameObject);
    }
    public void UseItem()
    {
        if (data.type != ItemType.Consumable || data.consumable == null) return;

        switch (data.consumable.type)
        {
            case ConsumableType.Health:
                CharacterManager.Instance.Player.condition.Heal(data.consumable.value);
                break;

            case ConsumableType.Speed:
                CharacterManager.Instance.Player.controller
                    .ApplySpeedBoost(data.consumable.value, data.consumable.duration);
                break;

            case ConsumableType.Jump:
                CharacterManager.Instance.Player.controller
                    .ApplyJumpBoost(data.consumable.value, data.consumable.duration);
                break;
        }
    }
}