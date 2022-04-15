using System.Runtime.CompilerServices;
using Gameplay.GameField;
using Gameplay.Items;
using UnityEngine;

namespace Gameplay.Player
{
    public class ItemDragger : MonoBehaviour
    {
        [SerializeField] private LayerMask itemLayer;
        [SerializeField] private LayerMask cellLayer;
        
        private Item _current;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            HandleItemDragging();
            HandleMouseClicks();
        }

        private void HandleItemDragging()
        {
            if (_current == null) return;
            _current.transform.position = CursorWorldPosition();
        }

        private void HandleMouseClicks()
        {
            if (Input.GetMouseButtonDown(0))
                ClickItem();
            
            if (Input.GetMouseButtonDown(1))
                GrabItem();
            
            if (Input.GetMouseButtonUp(1))
                ReleaseItem();
        }

        private void ClickItem()
        {
            var item = ItemUnderCursor();

            if (item != null)
                item.OnClick();
        }

        private void GrabItem()
        {
            _current = ItemUnderCursor();

            if (_current != null)
                _current.Grab();
        }

        private void ReleaseItem()
        {
            if (_current == null) return;
            
            var itemUnderCursor = ItemUnderCursor();
            if (itemUnderCursor != null)
            {
                if (itemUnderCursor.IsMatchWith(_current))
                {
                    itemUnderCursor.Merge(_current);
                    Destroy(_current.gameObject);
                }
                else
                {
                    _current.Release();
                    _current = null;
                }
            }
            else
            {
                var cellUnderCursor = CellUnderCursor();
                if (cellUnderCursor != null)
                {
                    _current.SetCell(cellUnderCursor);
                }
                _current.Release();
                _current = null;
            }
        }

        private Item ItemUnderCursor()
        {
            var cell = CellUnderCursor();
            if (cell != null)
            {
                var item = cell.Item;
                return item == _current ? null : item;
            }

            return null;
        }
        
        private Cell CellUnderCursor()
        {
            var mousePos = CursorWorldPosition();
            var hit = Physics2D.Raycast(mousePos, Vector2.zero, 100, cellLayer);
            
            if (hit.collider == null) return null;
            
            hit.collider.TryGetComponent<Cell>(out var cell);
            return cell;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Vector2 CursorWorldPosition()
        {
            return _camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
