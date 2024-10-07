using FirstProject.Beastiary;
using FirstProject.Npc;
using Godot;
using Newtonsoft.Json;
using QRCoder;
using System;

public partial class EquipmentQrCodeDisplay : TextureRect, INpcEquipmentReader
{
    public Action OnEquipmentUpdated { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

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

	private static string ToJson(NpcEquipment npcEquipment)
	{
		string type;
		bool checkHandedNess = false;
        string rangeValue = null;
        if (npcEquipment.Category.IsWeapon)
		{
			type = "Weapon";
			checkHandedNess = true;
			rangeValue = npcEquipment.BasicAttack.IsRanged ? "Ranged" : "Melee";

        }
		else if (npcEquipment.Category.IsArmor)
		{
			type = "Armor";
		} 
		else if(npcEquipment.Category.Name == "Shield")
		{
			type = "Shield";
            checkHandedNess = true;
        }
		else
		{
			type = "Accessory";
		}


		var equipment = new
		{
			name = npcEquipment.Name,
			cost = npcEquipment.Cost,
			quality = npcEquipment.Quality,
			type = type,
			accuracy = npcEquipment.BasicAttack != null ? $"\u3010{npcEquipment.BasicAttack.Attribute1?.ShortenAttribute()} + {npcEquipment.BasicAttack.Attribute2?.ShortenAttribute()}\u3011" : null,
			damage = npcEquipment.BasicAttack != null ? $"\u3010HR + {npcEquipment.BasicAttack.DamageMod}\u3011{npcEquipment.BasicAttack.DamageType.Name}" : null,
			handedness = checkHandedNess ? $"{ToEnglish(npcEquipment.NumHands)} Handed"  : null,
			range = rangeValue,
			martial = npcEquipment.IsMartial,
			category = npcEquipment.Category.Name,
			defense = npcEquipment.Modifiers?.DefenseModifier,
			mDefense = npcEquipment.Modifiers?.MagicDefenseModifier,
			initiative = npcEquipment.Modifiers?.InitiativeModifier,
			dice1 = npcEquipment.BasicAttack?.Attribute1?.ShortenAttribute(),
            dice2 = npcEquipment.BasicAttack?.Attribute2?.ShortenAttribute(),
            accuracyConstant = npcEquipment.BasicAttack?.AttackMod,
            damageConstant = npcEquipment.BasicAttack?.DamageMod,
        };
	
		var serializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.None
		};

        return JsonConvert.SerializeObject(equipment, serializerSettings);
	}

	private static bool IsValid(NpcEquipment equipment)
	{
		if (string.IsNullOrWhiteSpace(equipment.Name)) return false;
        if (equipment.Category.IsWeapon)
        {
			if (equipment.BasicAttack?.Attribute1 == null) return false;
            if (equipment.BasicAttack?.Attribute2 == null) return false;
        }
		else if (equipment.Category.IsArmor)
		{
			if (equipment.Modifiers == null) return false;
		}
        return true;
	}

	public void HandleEquipmentChanged(NpcEquipment equipment)
	{
		if (!IsValid(equipment)) return;
		var data = ToJson(equipment);
		GD.Print(data);
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
        var encodedData = System.Convert.ToBase64String(plainTextBytes);
		using QRCodeGenerator qrGenerator = new QRCodeGenerator();
		using QRCodeData qrCodeData = qrGenerator.CreateQrCode(encodedData, QRCodeGenerator.ECCLevel.Q);
		using var qrCode = new SvgQRCode(qrCodeData);
		var qrCodeImage = qrCode.GetGraphic(20);
		var image = new Image();
		image.LoadSvgFromString(qrCodeImage);
        Texture2D texture = ImageTexture.CreateFromImage(image);		
        this.Texture = texture;
    }

    public void HandleEquipmentInitialized(NpcEquipment equipment)
    {
        // do nothing
    }
}
