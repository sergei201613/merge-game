using System.Runtime.CompilerServices;
using Gameplay.Common;
using Gameplay.Items;
using Gameplay.Items.Storages;
using UnityEngine;

namespace Gameplay.GameField
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private byte width;
        [SerializeField] private byte height;
        [SerializeField] private float gap = 1;
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Color oddCellColor;
        [SerializeField] private Color evenCellColor;
        [SerializeField] private Storage storagePrefab;
        [SerializeField] private ItemData[] storages;

        private Cell[,] _cells;
        private GameMode _gameMode;

        private void Awake()
        {
            _gameMode = FindObjectOfType<GameMode>();
            
            Generate();
        }

        public bool TryGetNearestFreeCell(Vector2Int index, out Cell cell)
        {
            Cell nearestCell = null;
            var nearestCellDist = Mathf.Infinity;
            
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var curCell = _cells[i, j];

                    if (!curCell.IsFree())
                        continue;
                    
                    var dist = Vector2Int.Distance(curCell.Index, index);
                    
                    if (dist < nearestCellDist)
                    {
                        nearestCellDist = dist;
                        nearestCell = curCell;
                    }
                } 
            }

            cell = nearestCell;
            return nearestCell != null;
        }

        public void AddScore(int value)
        {
            _gameMode.AddScore(value); 
        }
        
        private void Generate()
        {
            _cells = new Cell[height, width];

            var xOffset = CellOffset(width);
            var yOffset = CellOffset(height);

            var midRow = (byte)(height / 2);
            var midCol = (byte)(width / 2);
            
            for (byte i = 0; i < height; i++)
            {
                for (byte j = 0; j < width; j++)
                {
                    var cell = Instantiate(cellPrefab, transform);
                    
                    var x = j * gap - xOffset;
                    var y = i * gap - yOffset;
                        
                    var position = new Vector3(x, y);
                    
                    cell.transform.position = position;
                    cell.Init(this, new Vector2Int(i, j));
                    cell.SetColor((j + i) % 2 == 0 ? evenCellColor : oddCellColor);
                    
                    _cells[i, j] = cell;

                    if (i == midRow && j == midCol)
                    {
                        cell.PutItem(storagePrefab, GetRandomStorage());
                    }
                } 
            } 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ItemData GetRandomStorage()
        {
            return storages[Random.Range(0, storages.Length)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float CellOffset(int dimensionLength)
        {
            if (dimensionLength % 2 == 1)
                // ReSharper disable once PossibleLossOfFraction
                return dimensionLength / 2 * gap;
            else
                // ReSharper disable once PossibleLossOfFraction
                return (dimensionLength / 2 - gap / 2) * gap;
        }
    }
}
