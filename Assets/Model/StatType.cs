using System.Collections.Generic;

[System.Serializable]
    public class StatType
    {
        public string type; // attack, health, defense
        public List<AttackType> attackTypes; // Only for attack
        public int basic; // For non-attack stats
        public int itemEnhancement;
        public string itemName;
        public int boostEnhancement;
    }

    [System.Serializable]
    public class AttackType
    {
        public string type; // melee, ranged, magic, etc.
        public int basic;
        public int itemEnhancement;
        public string itemName;
        public int boostEnhancement;
    }