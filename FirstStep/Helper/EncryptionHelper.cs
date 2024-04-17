using System.Security.Cryptography;
using System.Text;

namespace FirstStep.Helper
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string data)
        {
            using (Aes myAes = Aes.Create())
            {
                // Encrypt the string to an array of bytes.
                byte[] encrypted = EncryptStringToBytes_Aes(data, myAes.Key, myAes.IV);

                // convert into string
                string roundtrip = Encoding.UTF8.GetString(encrypted);

                return roundtrip;
            }
        }

        public static string Decrypt(string encrypted)
        {
            using (Aes myAes = Aes.Create())
            {
                // convert into byte array
                byte[] encryptedBytes = Encoding.UTF8.GetBytes(encrypted);

                // Decrypt the bytes to a string.
                string roundtrip = DecryptStringFromBytes_Aes(encryptedBytes, myAes.Key, myAes.IV);

                return roundtrip;
            }
        }

        private static byte[] EncryptStringToBytes_Aes(string decryptedText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (decryptedText == null || decryptedText.Length <= 0)
                throw new ArgumentNullException("decryptedText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(decryptedText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string decryptedText = "";

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return decryptedText;
        }
    }
}
