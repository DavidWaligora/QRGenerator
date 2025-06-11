using QRCoder;
using System.Diagnostics;
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

    // Navigate to /src directory
    // Get current directory(bin/ Debug / netX.X)
    string binDir = Directory.GetCurrentDirectory();

    string? projectRoot = Directory.GetParent(binDir)?.Parent?.Parent?.FullName;
    string targetDir = Path.Combine(projectRoot!, "src");

    Directory.CreateDirectory(targetDir);
    string fileName = Path.Combine(targetDir, $"{url}.png");
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