using System.Text;

namespace StickyNotes.Scripts;

public class Cryptography
{
    private const int Key = 321847689;

    public static string Encrypt(string input)
    {
        StringBuilder encrypted = new StringBuilder();

        foreach (char c in input)
        {
            encrypted.Append((char)(c + Key));
        }

        return encrypted.ToString();
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