using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Items.Storages 
{
    public class Storage : Item
    {
        [SerializeField] public Item prefab;
        [SerializeField] public ItemData[] items;

        private Item GetItem()
        {
            var id = Random.Range(0, items.Length);

            if (Field.TryGetNearestFreeCell(Cell.Index, out var cell))
            {
                cell.PutItem(prefab, items[id], transform.position);
                return cell.Item;
            }

            return null;
        }

        public override void OnClick()
        {
            GetItem();
        }

        public override ItemType GetItemType()
        {
            return ItemType.Storage;
        }
    }
}
