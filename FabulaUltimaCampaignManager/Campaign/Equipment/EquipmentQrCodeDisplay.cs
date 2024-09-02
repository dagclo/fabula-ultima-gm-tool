using FirstProject.Npc;
using Godot;
using QRCoder;
using System;

public partial class EquipmentQrCodeDisplay : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void HandleEquipmentUpdated(NpcEquipment equipment)
	{
		var data = equipment.Quality ?? "nothing here";
		using QRCodeGenerator qrGenerator = new QRCodeGenerator();
		using QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
		using var qrCode = new SvgQRCode(qrCodeData);
		var qrCodeImage = qrCode.GetGraphic(20);
		var image = new Image();
		//var image = Image.lo
		image.LoadSvgFromString(qrCodeImage);
        Texture2D texture = ImageTexture.CreateFromImage(image);
        this.Texture = texture;
    }
}
