## DES in C#

 A C# implementation of the Data Encryption Standart algorithm also known as DES. 

 ### Overview 

 DES is a block cipher that uses a Feistel structure to encrypt plaintext. It uses a 56-bit key.
 The algorithm was used from 1976 to 1999 where it was replaced by AES as the standart for encryption. 

 This implementation is not optimized in any way as the main goal was to just get a working algorithm. 

 ### Usage

 To encrypt a plaintext: 

 `string text = "text you want to encrypt";` <br>
  `string key = "O9ZssN32";` <br>
  `DES des = new DES();` <br>
  `var encrypted = des.Encrypt(input, key);` <br>

  To decrypt a ciphertext: 

  `var decrypted = des.Decrypt(encrypted, key);`
  
