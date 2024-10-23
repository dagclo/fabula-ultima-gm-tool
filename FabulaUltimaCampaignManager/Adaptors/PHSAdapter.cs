using FabulaUltimaNpc;
using FirstProject.Beastiary;
using FirstProject.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaGMTool.Adaptors
{
    public static class PHSAdapter
    {
        private static string ToEnglish(int num)
        {
            switch (num)
            {
                case 1: return "One";
                case 2: return "Two";
                default:
                    throw new NotImplementedException();
            }
        }

        public static object ToDataFormat(SpellTemplate spell)
        {
            return new
            {
                name = spell.Name,
                source = "Custom",
                mp = spell.MagicPointCost,
                target = spell.Target,
                duration = spell.Duration,
                description = spell.Description,
                offensive = spell.IsOffensive
            };
        }

        public static dynamic ToDataFormat(NpcEquipment npcEquipment)
        {
            string type;
            bool checkHandedNess = false;
            string rangeValue = null;
            int? defaultModValue = null;
            string emptyStringDefault = null;
            int? emptyIntDefault = null;
            if (npcEquipment.Category.IsWeapon)
            {
                type = "Weapon";
                checkHandedNess = true;
                rangeValue = npcEquipment.BasicAttack.IsRanged ? "Ranged" : "Melee";
                defaultModValue = 0;
            }
            else if (npcEquipment.Category.Name == "Shield")
            {
                type = "Shield";
                emptyStringDefault = string.Empty;
                rangeValue = emptyStringDefault;
                emptyIntDefault = 0;
            }
            else if (npcEquipment.Category.IsArmor)
            {
                return AsArmor(npcEquipment);
            }
            else
            {
                return AsAccessory(npcEquipment);
            }


            var equipment = new
            {
                name = npcEquipment.Name,
                cost = npcEquipment.Cost,
                quality = npcEquipment.Quality,
                type = type,
                accuracy = npcEquipment.BasicAttack != null ? $"\u3010{npcEquipment.BasicAttack.Attribute1?.ShortenAttribute()} + {npcEquipment.BasicAttack.Attribute2?.ShortenAttribute()}\u3011" : emptyStringDefault,
                damage = npcEquipment.BasicAttack != null ? $"\u3010HR + {npcEquipment.BasicAttack.DamageMod}\u3011{npcEquipment.BasicAttack.DamageType.Name}" : emptyStringDefault,
                handedness = checkHandedNess ? $"{ToEnglish(npcEquipment.NumHands)} Handed" : emptyStringDefault,
                range = rangeValue,
                martial = npcEquipment.IsMartial,
                category = npcEquipment.Category.IsWeapon ? npcEquipment.Category.Name : emptyStringDefault,
                defense = npcEquipment.Category.IsArmor ? (npcEquipment.Modifiers?.DefenseModifier ?? defaultModValue) : 0,
                mDefense = npcEquipment.Category.IsArmor ? (npcEquipment.Modifiers?.MagicDefenseModifier ?? defaultModValue) : 0,
                dice1 = npcEquipment.BasicAttack?.Attribute1?.ShortenAttribute() ?? emptyStringDefault,
                dice2 = npcEquipment.BasicAttack?.Attribute2?.ShortenAttribute() ?? emptyStringDefault,
                accuracyConstant = npcEquipment.BasicAttack?.AttackMod ?? emptyIntDefault,
                damageConstant = npcEquipment.BasicAttack?.DamageMod ?? emptyIntDefault,
                basic = true,              
                initiative = npcEquipment.Category.IsArmor ? npcEquipment.Modifiers.InitiativeModifier : 0,
            };

            return equipment;
        }

        private static dynamic AsArmor(NpcEquipment npcEquipment)
        {
            return new
            {
                name = npcEquipment.Name,
                cost = npcEquipment.Cost,
                defenseDice = npcEquipment.Modifiers.DefenseOverrides ? string.Empty : "DEX",
                defenseConstant = npcEquipment.Modifiers.DefenseModifier,
                mDefenseDice = "INS",
                mDefenseConstant = npcEquipment.Modifiers.MagicDefenseModifier,
                initiative = npcEquipment.Modifiers.InitiativeModifier,
                quality = npcEquipment.Quality,
                martial = npcEquipment.IsMartial,
                basic = true
            };
        }

        private static dynamic AsAccessory(NpcEquipment npcEquipment)
        {
            return new
            {
                name = npcEquipment.Name,
                cost = npcEquipment.Cost,
                quality = npcEquipment.Quality
            };
        }
    }
}
