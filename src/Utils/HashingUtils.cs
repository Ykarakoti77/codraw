using System.Security.Cryptography;
using System.Text;

namespace Codraw.Utils;

public static class Hashing_md5
{
    public static string ComputeHash(string userId, string password)
    {
        byte[] tmpHash1 = MD5.HashData(Encoding.ASCII.GetBytes(userId));
        var saltedPassword = Convert.ToHexString(tmpHash1) + password;

        byte[] tmpHash2 = MD5.HashData(Encoding.ASCII.GetBytes(saltedPassword));
        return Convert.ToHexString(tmpHash2);
    }
}
