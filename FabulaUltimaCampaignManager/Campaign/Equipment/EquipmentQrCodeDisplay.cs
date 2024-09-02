using FirstProject.Npc;
using Godot;
using Newtonsoft.Json;
using QRCoder;
using System;

public partial class EquipmentQrCodeDisplay : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private static string ToJson(NpcEquipment equipment)
	{
		var obj = new
		{
			name = equipment.Name,
			cost = equipment.Cost,
			quality = equipment.Quality,
		};
		return JsonConvert.SerializeObject(obj);
	}

	public void HandleEquipmentUpdated(NpcEquipment equipment)
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
}
