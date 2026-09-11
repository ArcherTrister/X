using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

#if USEX
using X.EntityFrameworkCore.FieldEncryption.Providers;
#else
using SoftFluent.EntityFrameworkCore.DataEncryption.Providers;
using SoftFluent.EntityFrameworkCore.DataEncryption;
#endif

namespace X.EntityFrameworkCore.FieldEncryption.Demo;

internal class Program
{
#if !USEX
    public class AesFieldEncryptionProvider : IEncryptionProvider
    {
        /// <summary>
        /// AES block size constant.
        /// </summary>
        public const int AesBlockSize = 128;

        /// <summary>
        /// Initialization vector size constant.
        /// </summary>
        public const int InitializationVectorSize = 16;

        private readonly byte[] _key;
        private readonly byte[] _iv;
        private readonly CipherMode _mode;
        private readonly PaddingMode _padding;

        /// <summary>
        /// Creates a new <see cref="AesFieldEncryptionProvider"/> instance used to perform symmetric encryption and decryption on strings.
        /// </summary>
        /// <param name="key">AES key used for the symmetric encryption.</param>
        /// <param name="initializationVector">AES Initialization Vector used for the symmetric encryption.</param>
        /// <param name="mode">Mode for operation used in the symmetric encryption.</param>
        /// <param name="padding">Padding mode used in the symmetric encryption.</param>
        public AesFieldEncryptionProvider(byte[] key, byte[] initializationVector, CipherMode mode = CipherMode.CBC,
            PaddingMode padding = PaddingMode.PKCS7)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key), "");
            _iv = initializationVector ?? throw new ArgumentNullException(nameof(initializationVector), "");
            _mode = mode;
            _padding = padding;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                return null;
            }

            var input = Encoding.UTF8.GetBytes(plainText);
            using Aes aes = CreateCryptographyProvider(_key, _iv, _mode, _padding);
            using ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, transform, CryptoStreamMode.Write);

            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            memoryStream.Seek(0L, SeekOrigin.Begin);

            return Convert.ToBase64String(StreamToBytes(memoryStream));
        }

        /// <inheritdoc />
        public byte[] Encrypt(byte[] plainText)
        {
            if (plainText is null || plainText.Length == 0)
            {
                return null;
            }

            using Aes aes = CreateCryptographyProvider(_key, _iv, _mode, _padding);
            using ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, transform, CryptoStreamMode.Write);

            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            memoryStream.Seek(0L, SeekOrigin.Begin);

            return StreamToBytes(memoryStream);
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return null;
            }

            var input = Encoding.UTF8.GetBytes(cipherText);
            using Aes aes = CreateCryptographyProvider(_key, _iv, _mode, _padding);
            using ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new(input);
            using CryptoStream cryptoStream = new(memoryStream, transform, CryptoStreamMode.Read);

            return Encoding.UTF8.GetString(StreamToBytes(cryptoStream)).Trim('\0');
        }

        /// <inheritdoc />
        public byte[] Decrypt(byte[] cipherText)
        {
            if (cipherText is null || cipherText.Length == 0)
            {
                return null;
            }

            using Aes aes = CreateCryptographyProvider(_key, _iv, _mode, _padding);
            using ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new(cipherText);
            using CryptoStream cryptoStream = new(memoryStream, transform, CryptoStreamMode.Read);

            return StreamToBytes(cryptoStream);
        }

        /// <summary>
        /// Converts a <see cref="Stream"/> into a byte array.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <returns>The stream's content as a byte array.</returns>
        internal static byte[] StreamToBytes(Stream stream)
        {
            if (stream is MemoryStream ms)
            {
                return ms.ToArray();
            }

            using var output = new MemoryStream();
            stream.CopyTo(output);
            return output.ToArray();
        }

        /// <summary>
        /// Generates an AES cryptography provider.
        /// </summary>
        /// <returns></returns>
        private static Aes CreateCryptographyProvider(byte[] key, byte[] iv, CipherMode mode, PaddingMode padding)
        {
            var aes = Aes.Create();

            aes.Mode = mode;
            aes.KeySize = key.Length * 8;
            aes.BlockSize = AesBlockSize;
            aes.FeedbackSize = AesBlockSize;
            aes.Padding = padding;
            aes.Key = key;
            aes.IV = iv;

            return aes;
        }

        // /// <summary>
        // /// Generates an AES key.
        // /// </summary>
        // /// <remarks>
        // /// The key size of the Aes encryption must be 128, 192 or 256 bits.
        // /// Please check https://blogs.msdn.microsoft.com/shawnfa/2006/10/09/the-differences-between-rijndael-and-aes/ for more informations.
        // /// </remarks>
        // /// <param name="keySize">AES Key size</param>
        // /// <returns></returns>
        // public static AesKeyInfo GenerateKey(AesKeySize keySize)
        // {
        //     using var aes = Aes.Create();
        //
        //     aes.KeySize = (int)keySize;
        //     aes.BlockSize = AesBlockSize;
        //
        //     aes.GenerateKey();
        //     aes.GenerateIV();
        //
        //     return new AesKeyInfo(aes.Key, aes.IV);
        // }
    }

    public class AbpAesFieldEncryptionProvider : IEncryptionProvider
    {
        private int Keysize = 256;
        //private string DefaultPassPhrase = "gsKnGZ041HLL4IM8";
        private string DefaultPassPhrase = "WBN0szwYr7wL8Dou";
        private byte[] InitVectorBytes = Encoding.ASCII.GetBytes("jkE49230Tf093b42");
        private byte[] DefaultSalt = Encoding.ASCII.GetBytes("hgt!16kl");

        public byte[] Encrypt(byte[] input)
        {
            var passPhrase = DefaultPassPhrase;
            var salt = DefaultSalt;

            using var password = new Rfc2898DeriveBytes(passPhrase, salt);
            var keyBytes = password.GetBytes(Keysize / 8);
            using var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            using var encryptor = symmetricKey.CreateEncryptor(keyBytes, InitVectorBytes);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        public byte[] Decrypt(byte[] input)
        {
            var passPhrase = DefaultPassPhrase;
            var salt = DefaultSalt;

            using var password = new Rfc2898DeriveBytes(passPhrase, salt);
            var keyBytes = password.GetBytes(Keysize / 8);
            using var symmetricKey = Aes.Create();
            symmetricKey.Mode = CipherMode.CBC;
            using var decryptor = symmetricKey.CreateDecryptor(keyBytes, InitVectorBytes);
            using var memoryStream = new MemoryStream(input);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[input.Length];
            var totalReadCount = 0;
            while (totalReadCount < input.Length)
            {
                var buffer = new byte[input.Length];
                var readCount = cryptoStream.Read(buffer, 0, buffer.Length);
                if (readCount == 0)
                {
                    break;
                }

                for (var i = 0; i < readCount; i++)
                {
                    plainTextBytes[i + totalReadCount] = buffer[i];
                }

                totalReadCount += readCount;
            }

            return plainTextBytes;
        }
    }
#endif

    static void Main(string[] args)
    {
        using SqliteConnection connection = new("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(connection)
            .Options;

#if USEX
        // AES key randomly generated at each run.
        AesKeyInfo keyInfo = AesFieldEncryptionProvider.GenerateKey(AesKeySize.AES256Bits);
        var encryptionProvider = new AesFieldEncryptionProvider(keyInfo.Key, keyInfo.IV);
#else
        // AES key randomly generated at each run.
        // AesKeyInfo keyInfo = AesProvider.GenerateKey(AesKeySize.AES256Bits);
        // var encryptionProvider = new AesProvider(keyInfo.Key, keyInfo.IV);
        // var encryptionProvider = new AesFieldEncryptionProvider(keyInfo.Key, keyInfo.IV);
        var encryptionProvider = new AbpAesFieldEncryptionProvider();
#endif

        using (var context = new DatabaseContext(options, encryptionProvider))
        {
            context.Database.EnsureCreated();

            var user = new FluentUserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                PhoneNumber = "15108772402",
                Notes = "Hello world!",
                EncryptedData = new byte[2] { 1, 2 },
                EncryptedDataAsString = new byte[2] { 3, 4 }
            };

            context.FluentUsers.Add(user);
            context.SaveChanges();

            Console.WriteLine($"Users count: {context.FluentUsers.Count()}");
        }

        using (var context = new EncryptedDatabaseContext(options))
        {
            FluentUserEntity user = context.FluentUsers.First();

            Console.WriteLine($"Encrypted User: {user.FirstName} {user.LastName} - {user.Email}【{user.PhoneNumber}】(Notes: {user.Notes})");
        }

        using (var context = new DatabaseContext(options, encryptionProvider))
        {
            FluentUserEntity user = context.FluentUsers.First();

            Console.WriteLine($"User: {user.FirstName} {user.LastName} - {user.Email}【{user.PhoneNumber}】(Notes: {user.Notes})");
        }

        using (var context = new DatabaseContext(options, encryptionProvider))
        {
            context.Database.EnsureCreated();

            var user = new UserEntity
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                PhoneNumber = "15108772402",
                Notes = "Hello world!",
                EncryptedData = new byte[2] { 1, 2 },
                EncryptedDataAsString = new byte[2] { 3, 4 }
            };

            context.Users.Add(user);
            context.SaveChanges();

            Console.WriteLine($"Users count: {context.Users.Count()}");
        }

        using (var context = new DatabaseContext(options, encryptionProvider))
        {
            UserEntity user = context.Users.First();

            Console.WriteLine($"User: {user.FirstName} {user.LastName} - {user.Email}【{user.PhoneNumber}】(Notes: {user.Notes})");
        }
    }
}
