using System.Text;

namespace StickyNotes;

public class Cryptography
{
    private const int Key = 3;

    public static string Encrypt(string input)
    {
        try
        {
            StringBuilder encrypted = new StringBuilder();

            foreach (char c in input)
            {
                encrypted.Append((char)(c + Key));
            }

            return encrypted.ToString();
        }
        catch
        {
            return null;
        }
    }

    public static string Decrypt(string input)
    {
        StringBuilder decrypted = new StringBuilder();

        foreach (char c in input)
        {
            decrypted.Append((char)(c - Key));
        }

        return decrypted.ToString();
    }
}