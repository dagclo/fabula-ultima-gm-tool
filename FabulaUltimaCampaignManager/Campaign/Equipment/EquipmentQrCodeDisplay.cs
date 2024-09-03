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

	private static string ToJson(NpcEquipment npcEquipment)
	{
		var equipment = new
		{
			name = npcEquipment.Name,
			cost = npcEquipment.Cost,
			quality = npcEquipment.Quality,
            //defenseConstant = npcEquipment.Modifiers.ini
        };
		var wrapper = new
		{
			category = npcEquipment.Category.Name,
			equipment = equipment
		};
		var serializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.None
		};

        return JsonConvert.SerializeObject(wrapper, serializerSettings);
	}

	public void HandleEquipmentChanged(NpcEquipment equipment)
	{
		var data = ToJson(equipment);
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
