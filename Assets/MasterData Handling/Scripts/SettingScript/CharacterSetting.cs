using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MasterDataHandling
{
    [CreateAssetMenu(fileName = "Character", menuName = "MasterData Handling/Character Setting", order = 0)]
    public class CharacterSetting : ScriptableObject
    {
        [SerializeField] string characterName;
        public string CharacterName => characterName;
        [SerializeField] List<float> attack;
        public List<float> Attack => attack;
        [SerializeField] List<float> defense;
        public List<float> Defense => defense;
        [SerializeField] List<float> health;
        public List<float> Health => health;

        string[] keys => new string[]{
            nameof(attack),
            nameof(defense),
            nameof(health)
        };

        [ContextMenu("Load MasterData")]
        void Load()
        {
            var result = MasterDataLoader.Load<float>(
                characterName, "MasterData Handling/MasterData",
                keys);

            if (result == null) return;

            this.attack = result[nameof(attack)];
            this.defense = result[nameof(defense)];
            this.health = result[nameof(health)];
        }
    }
}