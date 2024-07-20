using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OBTEST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : ControllerBase
    {
        private static string rsaKey = "<RSAKeyValue><Modulus>psxff5FLoANp7HbWMRe3pvV/eh8qCY4dlJPeqqz1PwSalnK+O+ih77bRfUDcCCS+yvtGaqXT2fF7EtDIa2MbcBJMdZXwBwf3ylgwy+g4lkyc5Rh0UMOpNBSlpTCryo/0+psG4fOWrDDf1WsZ/5DI4WRSbEuk1cx+4x3NUTAUB7k=</Modulus><Exponent>AQAB</Exponent><P>6oQ150mEIyA5TNVBZ1TMhWA8r2hEKxRwCkDwFmB+qjdw33hkTkZhK50FNIO6bmdE6kpZIrpCTJ8ZK4rCWhMp+w==</P><Q>thQQNR7OIVTiBsle4XzWso2z/jtHUFHJlY8RZjf5vsiZrKtQx3LD+DASP1DRD1UnHRWMnZ3zTdLnucmHcRD62w==</Q><DP>mPNnkJRDCQHAPVssz+7fgPGWQrSXGR24QQe/Tmja07ta83S6vs5qG57KQUjUs6LIsKGS5vJhwUVWji5uuX6cNw==</DP><DQ>r1l1HmNTaqA/qP+Hg6rhbUWwoVdfX4fUllcZD5M6zrSL4tF90wbAmiVZfWaMX7LHH2hgam7yIPHLPo5KBOawXw==</DQ><InverseQ>jT6lqTbj5BNjX/K4RUUWGbXU6r/GUi8Q6aIZjuq+wNhqUBhhiLW18sb8BA+K8O2hu6TEZLFS8CdYZ9miotwKGQ==</InverseQ><D>Z2PBQkKevNXA35kd1Zpc9TmxRdJxbTDRNxqdZ/ADqIdDB0SilGHzlrIckmYUvVuBhDJTCKI3eh2L6zLNOHtbMo96X/8V1j9mDvRnYnDREJXeiJR5SgArvDGVh36Eh2SsaOzhdhskSKUa2/Oc8oEoNMtjbXCFE7tSbxsQMjwn0d0=</D></RSAKeyValue>";
        private static string aesKey = "NmFjMDE3YTFlODM4ODdhYzU0MTJkOThhNmMxZjE2Zjc=";
        private static string aesIV = "N2ZkMThjMWE4NjM3OGFmNA==";

        /// <summary>
        /// RSA加密
        /// </summary>
        [HttpPost("rsa/encrypt")]
        public IActionResult RsaEncrypt([FromBody] string plainText)
        {
            return Ok(EncryptRSA(plainText));
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        [HttpPost("rsa/decrypt")]
        public IActionResult RsaDecrypt([FromBody] string cipherText)
        {
            return Ok(DecryptRSA(cipherText));
        }


        /// <summary>
        /// AES加密
        /// </summary>
        [HttpPost("aes/encrypt")]
        public IActionResult AesEncrypt([FromBody] string plainText)
        {
            return Ok(EncryptAES(plainText, aesKey, aesIV));
        }


        /// <summary>
        /// AES解密
        /// </summary>
        [HttpPost("aes/decrypt")]
        public IActionResult AesDecrypt([FromBody] string cipherText)
        {
            return Ok(DecryptAES(cipherText, aesKey, aesIV));
        }

        private static string EncryptRSA(string plainText)
        {
            string encryptedText = string.Empty;
            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(rsaKey);
                    encryptedText = Convert.ToBase64String(rsa.Encrypt(dataToEncrypt, false));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting RSA: {ex.Message}");
            }
            return encryptedText;
        }

        private static string DecryptRSA(string cipherText)
        {
            string decryptedText = string.Empty;
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(cipherText.Replace(' ', '+'));
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(rsaKey);
                    decryptedText = Encoding.UTF8.GetString(rsa.Decrypt(dataToDecrypt, false));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting RSA: {ex.Message}");
            }
            return decryptedText;
        }

        private static string EncryptAES(string plainText, string key, string iv)
        {
            string encryptedText = string.Empty;
            try
            {
                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Convert.FromBase64String(key);
                    aes.IV = Convert.FromBase64String(iv);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                            cs.FlushFinalBlock();
                            encryptedText = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting AES: {ex.Message}");
            }
            return encryptedText;
        }

        private static string DecryptAES(string cipherText, string key, string iv)
        {
            string decryptedText = string.Empty;
            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(cipherText.Replace(' ', '+'));
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    aes.Key = Convert.FromBase64String(key);
                    aes.IV = Convert.FromBase64String(iv);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                            cs.FlushFinalBlock();
                            decryptedText = Encoding.UTF8.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decrypting AES: {ex.Message}");
            }
            return decryptedText;
        }
    }
}

