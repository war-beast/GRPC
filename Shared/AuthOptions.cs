using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shared
{
	public class AuthOptions
	{
		public static string ISSUER => "grpcApp"; // издатель токена
		public static string AUDIENCE => "grpcAppUser"; // потребитель токена
		const string KEY = "lkfmngksfkgsfkglksfkgsfknsf495rjt4t4TtREG$$#%$rgkdgadgklfdmkgmndjgier";   // ключ для шифрации
		public static int LIFETIME => 1; // время жизни токена - 1 день
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
		}
	}
}
