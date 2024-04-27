using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Security.Cryptography;
using System;

[TestFixture]
public class AesEncryptionTests
{
    [Test]
    public void Constructor_WithValidKeyAndIV_CreatesObject()
    {
        // Act
        var aesEncryption = new AesEncryption();

        // Assert
        Assert.IsNotNull(aesEncryption);
    }

    [Test]
    public void Constructor_WithInvalidKeyOrIV_ThrowsArgumentException()
    {
        // Arrange
        var invalidKey = "InvalidKey";
        var invalidIV = "InvalidIV";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new AesEncryption(invalidKey, invalidIV));
    }

    [TestCase("Hello, World!", ExpectedResult = "SGVsbG8sIFdvcmxkIQ==")]
    [TestCase("", ExpectedResult = "")]
    public string EncodeToBase64_WithValidInput_ReturnsEncodedString(string input)
    {
        // Arrange
        var aesEncryption = new AesEncryption();

        // Act
        var encodedString = aesEncryption.EncodeToBase64(input);

        // Assert
        return encodedString;
    }

    [TestCase("SGVsbG8sIFdvcmxkIQ==", ExpectedResult = "Hello, World!")]
    public string DecodeFromBase64_WithValidInput_ReturnsDecodedString(string input)
    {
        // Arrange
        var aesEncryption = new AesEncryption();

        // Act
        var decodedString = aesEncryption.DecodeFromBase64(input);

        // Assert
        return decodedString;
    }

    [Test]
    public void DecodeFromBase64_WithInvalidInput_ThrowsFormatException()
    {
        // Arrange
        var aesEncryption = new AesEncryption();
        var invalidBase64String = "InvalidBase64String";

        // Act & Assert
        Assert.Throws<FormatException>(() => aesEncryption.DecodeFromBase64(invalidBase64String));
    }

    [TestCase("Hello, World!")]
    [TestCase("")]
    public void Encrypt_WithValidInput_ReturnsEncryptedString(string input)
    {
        // Arrange
        var aesEncryption = new AesEncryption();

        // Act
        var encryptedString = aesEncryption.Encrypt(input);

        // Assert
        Assert.AreNotEqual(input, encryptedString);
    }

    [Test]
    public void Decrypt_WithValidInput_ReturnsDecryptedString()
    {
        // Arrange
        var aesEncryption = new AesEncryption();
        var plainText = "Hello, World!";
        var encryptedText = aesEncryption.Encrypt(plainText);

        // Act
        var decryptedString = aesEncryption.Decrypt(encryptedText);

        // Assert
        Assert.AreEqual(plainText, decryptedString);
    }

    [Test]
    public void EncryptDecrypt_WithDifferentInstances_ReturnsOriginalText()
    {
        // Arrange
        var plainText = "Hello, World!";
        var aesEncryption1 = new AesEncryption();
        var aesEncryption2 = new AesEncryption();

        // Act
        var encryptedText = aesEncryption1.Encrypt(plainText);
        var decryptedText = aesEncryption2.Decrypt(encryptedText);

        // Assert
        Assert.AreEqual(plainText, decryptedText);
    }

    [Test]
    public void Encrypt_WithDifferentInstances_ReturnsSameEncryptedStrings()
    {
        // Arrange
        var plainText = "Hello, World!";
        var aesEncryption1 = new AesEncryption();
        var aesEncryption2 = new AesEncryption();

        // Act
        var encryptedText1 = aesEncryption1.Encrypt(plainText);
        var encryptedText2 = aesEncryption2.Encrypt(plainText);

        // Assert
        Assert.AreEqual(encryptedText1, encryptedText2);
    }

    [Test]
    public void EncryptDecrypt_WithMaximumLengthText_ReturnsOriginalText()
    {
        // Arrange
        var maxLengthPlainText = new string('A', 1024);
        var aesEncryption = new AesEncryption();

        // Act
        var encryptedText = aesEncryption.Encrypt(maxLengthPlainText);
        var decryptedText = aesEncryption.Decrypt(encryptedText);

        // Assert
        Assert.AreEqual(maxLengthPlainText, decryptedText);
    }
}

