using FabulaUltimaGMTool.Adaptors;
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

    private static string ToJson(dynamic data)
	{
		var serializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.None
		};

        return JsonConvert.SerializeObject(data, serializerSettings);
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
		var data = PHSAdapter.ToDataFormat(equipment);
		var json = ToJson(data);
		
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
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
