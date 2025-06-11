using QRCoder;
using static QRCoder.PayloadGenerator;

bool end = false;

while (!end)
{
    Console.Write("Enter the website URL to generate a QR code (or type 'exit' to quit) \"example: (test.com)\": ");
    string? url = Console.ReadLine()?.Trim();
    url = url?.ToLower();
    if (string.IsNullOrEmpty(url) || url == "exit")
    {
        end = true;
        continue;
    }


    string? urlResult = null;
    while (urlResult == null)
    {
        urlResult = CheckCorrectUrl(url);
    }

    Console.WriteLine("Generating QR code...");
    Console.WriteLine("Please wait...");

    try
    {
        GenerateQRCode(urlResult);
    }
    catch (Exception ex)
    {
        Console.Write("An error occurred while generating the QR code: " + ex.Message);
        continue;
    }
}
void GenerateQRCode(string url)
{
    Url generator = new(url);
    string payload = generator.ToString();

    QRCodeGenerator qrGenerator = new();
    QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

    // Use PNG format
    using PngByteQRCode qrCode = new(qrCodeData);
    byte[] qrCodeImage = qrCode.GetGraphic(20);

    // Save to base directory
    string directory = Path.Combine("QRGenerator", "QRCodes");
    Directory.CreateDirectory(directory); // Ensure directory exists

    string fileName = Path.Combine(directory, $"{url}.png");
    File.WriteAllBytes(fileName, qrCodeImage);

    Console.WriteLine($"QR code saved successfully to: {fileName}");
}
string? CheckCorrectUrl(string url)
{
    Console.Write($"Is this the correct URL? \"{url}\" (y/n): ");
    string? answer = Console.ReadLine()?.Trim().ToLower();

    if (answer == "y")
    {
        return url;
    }
    else if (answer == "n" || answer == "no")
    {
        Console.WriteLine("Please enter the correct URL.");
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
    }
    return null;
}