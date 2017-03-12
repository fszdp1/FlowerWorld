using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace FlowerWorld.Infrastructure
{
    /// <summary>
    /// RemotePost 的摘要说明
    /// </summary>
    public class RemotePost
    {
        public static string getCheckValue(string merchantId, string returnUrl, string paymentTypeObjId, string amtStr, string merTransId)
        {
            string xmlKey = File.ReadAllText("D:\\" + merchantId + ".xml");
            RSAParameters PrvKeyInfo = RSAUtility.GetPrvKeyFromXmlString(xmlKey);
            RSACng rsa = new RSACng();
            rsa.ImportParameters(PrvKeyInfo);
            string orgString = merchantId + merTransId + paymentTypeObjId + amtStr + returnUrl;
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            byte[] orgData = byteConverter.GetBytes(orgString);
            byte[] signedData = rsa.SignData(orgData, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signedData);
        }
           
        public static bool PaymentVerify(HttpRequest curRequest, out string merId, out string amt, out string merTransId, out string transId, out string transTime)
        {
            merId = curRequest.Form["merId"].ToString();
            amt = curRequest.Form["amt"].ToString();
            merTransId = curRequest.Form["merTransId"].ToString();
            transId = curRequest.Form["transId"].ToString();
            transTime = curRequest.Form["transTime"].ToString();
            string checkValue = curRequest.Form["checkValue"].ToString();
            string PaymentPublicKey = File.ReadAllText("d:\\PaymentPublicKey.txt");
            RSAParameters PubKeyInfo = RSAUtility.GetPubKeyFromXmlString(PaymentPublicKey);
            string orgString = merId + merTransId + amt + transId + transTime;
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            byte[] orgData = byteConverter.GetBytes(orgString);
            byte[] signedData = Convert.FromBase64String(checkValue);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(PubKeyInfo);
            return rsa.VerifyData(orgData, signedData, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }
    }
    public class RSAUtility
    {
        public static RSAParameters GetPubKeyFromXmlString(string xmlString)
        {
            RSAParameters result = new RSAParameters();
            result.Modulus = getRSAKeyEle("Modulus", xmlString);
            result.Exponent = getRSAKeyEle("Exponent", xmlString);
            return result;
        }
        
        public static RSAParameters GetPrvKeyFromXmlString(string xmlString)
        {
            RSAParameters result = new RSAParameters();
            result.Modulus = getRSAKeyEle("Modulus", xmlString);
            result.Exponent = getRSAKeyEle("Exponent", xmlString);
            result.P = getRSAKeyEle("P", xmlString);
            result.Q = getRSAKeyEle("Q", xmlString);
            result.DP = getRSAKeyEle("DP", xmlString);
            result.DQ = getRSAKeyEle("DQ", xmlString);
            result.InverseQ = getRSAKeyEle("InverseQ", xmlString);
            result.D = getRSAKeyEle("D", xmlString);
            return result;
        }

        private static byte[] getRSAKeyEle(string keyName, string xmlString)
        {
            Regex r = new Regex("<" + keyName + @">[\w+=/]*</" + keyName + ">");
            string s = r.Match(xmlString).Value;
            return Convert.FromBase64String(s.Substring(keyName.Length + 2, s.Length - 2 * keyName.Length - 5));
        }
    }
    /*

     <RSAKeyValue>
      <Modulus>[base64-encoded value]</Modulus>
      <Exponent>[base64-encoded value]</Exponent>
    </RSAKeyValue>

    Public+Private:
    <RSAKeyValue>
      <Modulus>[base64-encoded value]</Modulus>
      <Exponent>[base64-encoded value]</Exponent>
      <P>[base64-encoded value]</P>
      <Q>[base64-encoded value]</Q>
      <DP>[base64-encoded value]</DP>
      <DQ>[base64-encoded value]</DQ>
      <InverseQ>[base64-encoded value]</InverseQ>
      <D>[base64-encoded value]</D>
    </RSAKeyValue>

     1、base64  to  string

        string strPath =  "aHR0cDovLzIwMy44MS4yOS40Njo1NTU3L1
    9iYWlkdS9yaW5ncy9taWRpLzIwMDA3MzgwLTE2Lm1pZA==";         
        byte[] bpath = Convert.FromBase64String(strPath);
        strPath = System.Text.ASCIIEncoding.Default.GetString(bpath);

    2、string   to  base64
        System.Text.Encoding encode = System.Text.Encoding.ASCII ;
        byte[]  bytedata = encode.GetBytes( "test");
        string strPath = Convert.ToBase64String(bytedata,0,bytedata.Length);
     */
}