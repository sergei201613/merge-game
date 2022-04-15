using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "New Item Data", menuName = "Items/Create Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private new string name = "New Item";
        [SerializeField] private int scoreToAdd = 5;
        [SerializeField] private byte level = 1;
        [SerializeField] private ItemCategory category;
        [SerializeField] private ItemData nextLevelItem;
        [SerializeField] private Sprite sprite;

        public string Name => name;
        public byte Level => level;
        public ItemCategory Category => category;
        public ItemData NextLevelItem => nextLevelItem;
        public Sprite Sprite => sprite;
        public int ScoreToAdd => scoreToAdd;
    }
}