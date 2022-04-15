using Gameplay.GameField;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private int defaultSortingOrder = 0;
        [SerializeField] private int movingSortingOrder = 10;
        [SerializeField] private int finalLevel = 5;
        [SerializeField] private SpriteRenderer crossSprite;
        [SerializeField] private UnityEvent merged = new UnityEvent();
        [SerializeField] private UnityEvent scored = new UnityEvent();
        
        private SpriteRenderer _sprite;
        private Collider2D _collider;
        private Vector3 _velocity;
        private bool _isGrabbed;

        public ItemData Data { get; private set; }
        public Field Field { get; private set; }
        public Cell Cell { get; private set; }

        public void Init(ItemData data, Field field, Cell cell)
        {
            Data = data;
            Field = field;
            Cell = cell;
            
            _sprite.sprite = Data.Sprite;
            _sprite.sortingOrder = movingSortingOrder;
        }
        
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void Update()
        {
            var localPosition = transform.localPosition;
            
            if (!_isGrabbed)
            {
                localPosition = Vector3.SmoothDamp(localPosition, Vector3.zero, 
                    ref _velocity, .05f);
                
                transform.localPosition = localPosition;
                
                var isMoving = localPosition.magnitude > .001f;
                _sprite.sortingOrder = isMoving ? movingSortingOrder : defaultSortingOrder;
            }
        }

        public virtual bool IsMatchWith(Item item)
        {
            if (item.Data.Category != Data.Category)
                return false;

            if (item.GetItemType() != GetItemType())
                return false;

            if (item.Data.Level != Data.Level)
                return false;

            if (item.Data.NextLevelItem == null)
                return false;
            
            if (Data.NextLevelItem == null)
                return false;
            
            return true;
        }

        public void Grab()
        {
            _collider.enabled = false;
            _isGrabbed = true;
            _sprite.sortingOrder = movingSortingOrder;
        }

        public void Release()
        {
            _collider.enabled = true;
            _isGrabbed = false;
        }

        public void Merge(Item current)
        {
            Init(Data.NextLevelItem, Field, Cell);
            merged.Invoke();
            CheckLevel();
        }

        private void CheckLevel()
        {
            if (Data.Level == finalLevel)
            {
                crossSprite.enabled = true;
            }
        }

        public virtual ItemType GetItemType()
        {
            return ItemType.Normal;
        }
        
        public virtual void OnClick()
        {
            if (Data.Level == finalLevel)
            {
                Cell.SetItem(null);
                Field.AddScore(Data.ScoreToAdd);
                scored?.Invoke();
                Destroy(gameObject);
            }
        }

        public void SetCell(Cell newCell)
        {
            Cell.SetItem(null);
            newCell.SetItem(this);
            Cell = newCell;
            transform.SetParent(newCell.transform);
        }
    }
}
