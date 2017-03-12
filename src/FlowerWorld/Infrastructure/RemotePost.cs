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
        private List<string[]> Inputs = new List<string[]>();
        private HttpContext context;
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";
        public RemotePost(HttpContext _context)
        {
            context = _context;
        }

        public async Task PostAsync()
        {

            context.Response.Clear();
            await context.Response.WriteAsync("<html><head>");
            await context.Response.WriteAsync(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            await context.Response.WriteAsync(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >",
                FormName, Method, Url));
            foreach (string[] ss in Inputs)
            {
                await context.Response.WriteAsync(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", ss[0], ss[1]));
            }
            await context.Response.WriteAsync("</form>");
            await context.Response.WriteAsync("</body></html>");
            //context.Response.End();
        }

        public async static Task PaymentPost(HttpContext c, string paymentUrl, string merchantId, string returnUrl, string paymentTypeObjId, string amtStr, string merTransId)
        {
            string xmlKey = File.ReadAllText("D:\\" + merchantId + ".xml");
            RSAParameters PrvKeyInfo = RSAUtility.GetPrvKeyFromXmlString(xmlKey);
            RSACng rsa = new RSACng();
            rsa.ImportParameters(PrvKeyInfo);
            string orgString = merchantId + merTransId + paymentTypeObjId + amtStr + returnUrl;
            ASCIIEncoding byteConverter = new ASCIIEncoding();
            byte[] orgData = byteConverter.GetBytes(orgString);
            byte[] signedData = rsa.SignData(orgData, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
            string signedString = Convert.ToBase64String(signedData);

            RemotePost myremotepost = new RemotePost(c);
            myremotepost.Url = paymentUrl;
            myremotepost.Inputs.Add(new string[] { "MerId", merchantId });
            myremotepost.Inputs.Add(new string[] { "Amt", amtStr });
            myremotepost.Inputs.Add(new string[] { "PaymentTypeObjId", paymentTypeObjId });
            myremotepost.Inputs.Add(new string[] { "MerTransId", merTransId });
            myremotepost.Inputs.Add(new string[] { "ReturnUrl", returnUrl });
            myremotepost.Inputs.Add(new string[] { "CheckValue", signedString });
            await myremotepost.PostAsync();
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