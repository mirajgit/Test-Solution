using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Test.UI.Controllers
{
    public class SplController : Controller
    {
        public IActionResult Api(string encodedPath)
        {
            if (!string.IsNullOrEmpty(encodedPath))
            {
                string decryptedPath = UrlCryptoHelper.Decrypt(encodedPath);
                if (!string.IsNullOrEmpty(decryptedPath))
                {
                    Uri uri;

                    // Check if the decrypted path is a valid URI
                    if (Uri.TryCreate(decryptedPath, UriKind.Absolute, out uri))
                    {
                        // It's a valid full URL, so just use its AbsolutePath
                        string relativePath = uri.AbsolutePath;
                        return Redirect(relativePath); // Only redirect to the path
                    }
                    else
                    {
                        // It's not a full URL, so assume it's a relative path
                        string baseUrl = $"{Request.Scheme}://{Request.Host}";
                        return Redirect(baseUrl + decryptedPath); // Combine the base URL with the relative path
                    }
                }
            }
            return Redirect("/");
        }

    }
}
