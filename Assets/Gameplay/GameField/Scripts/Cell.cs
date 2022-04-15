using Gameplay.Items;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.GameField
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer bgSprite;
        
        private Item _item;
        
        public Field Field { get; private set; }
        
        public Vector2Int Index { get; private set; }

        public Item Item => _item;

        public void Init(Field field, Vector2Int index)
        {
            Field = field;
            Index = index;
        }

        public void PutItem(Item prefab, ItemData data)
        {
            _item = Instantiate(prefab, transform); 
            _item.Init(data, Field, this);
        }
        
        public void PutItem(Item prefab, ItemData data, Vector3 startPos)
        {
            _item = Instantiate(prefab, startPos, quaternion.identity, transform); 
            _item.Init(data, Field, this);
        }

        public bool IsFree()
        {
            return _item == null;
        }

        public void SetItem(Item item)
        {
            _item = item;
        }

        public void SetColor(Color color)
        {
            bgSprite.color = color;
        }
    }
}
