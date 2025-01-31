using Godot;
using Newtonsoft.Json;
using QRCoder;

namespace FabulaUltimaGMTool
{
    public static class QRCodeGenerator
    {
        private static string ToJson<T>(T data)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };

            return JsonConvert.SerializeObject(data, serializerSettings);
        }

        public static Texture2D ToQRCode<T>(this T data)
        {
            var json = ToJson(data);

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            var encodedData = System.Convert.ToBase64String(plainTextBytes);
            using var qrGenerator = new QRCoder.QRCodeGenerator();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(encodedData, QRCoder.QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new SvgQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            var image = new Image();
            image.LoadSvgFromString(qrCodeImage);
            Texture2D texture = ImageTexture.CreateFromImage(image);
            return texture;
        }
    }
}
