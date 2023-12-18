
/**
* Copyright (C) Fernando Holguin Weber, and Studio High Ground - All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential.
* Written by Fernando Holguin <fernando@studiohighground.com>
* 
* UNDER NO CIRCUMSTANCES IS FERNANDO HOLGUIN WEBER, OR Studio High Ground, ITS PROGRAM
* DEVELOPERS OR SUPPLIERS LIABLE FOR ANY OF THE FOLLOWING, EVEN IF INFORMED OF THEIR
* POSSIBILITY: LOSS OF, OR DAMAGE TO, DATA; DIRECT, SPECIAL, INCIDENTAL, OR INDIRECT
* DAMAGES, OR FOR ANY ECONOMIC CONSEQUENTIAL DAMAGES; OR  LOST PROFITS, BUSINESS,
* REVENUE, GOODWILL, OR ANTICIPATED SAVINGS.
* 
*/


using System;
using System.Security.Cryptography;
using System.Text;
public static class CryptoService
{
    public static string Sha256(string randomString)
    {
        var crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
        foreach (byte theByte in crypto)
        {
            hash += theByte.ToString("x2");
        }
        return hash;
    }
}