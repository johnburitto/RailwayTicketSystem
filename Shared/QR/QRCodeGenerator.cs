using IronBarCode;
using Newtonsoft.Json;
using System.Text;

namespace Shared.QR
{
    public static class QRCodeGenerator
    {
        public static byte[] Generate(object? obj) 
        {
            var qr = BarcodeWriter.CreateBarcode(Encoding.Default.GetBytes(JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                { 
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })), BarcodeEncoding.QRCode, 512, 512);

            return qr.ToPngBinaryData();
        }
    }
}
