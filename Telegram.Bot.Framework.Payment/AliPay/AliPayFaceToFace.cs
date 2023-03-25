//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Kernel;
using Alipay.EasySDK.Kernel.Util;
using Alipay.EasySDK.Payment.FaceToFace.Models;
using QRCoder;
using SixLabors.ImageSharp;

namespace Telegram.Bot.Framework.Payment.AliPay
{
    /// <summary>
    /// 
    /// </summary>
    public class AliPayFaceToFace : IPaymentMethod
    {
        public void Test()
        {
            // 1. 设置参数（全局只需设置一次）

            Factory.SetOptions(GetConfig());

            try

            {

                // 2. 发起API调用（以创建当面付收款二维码为例）

                var response = Factory.Payment.FaceToFace()

                    .PreCreate("Apple iPhone11 128G", "2234567234890", "0.01");

                // 3. 处理响应或异常

                if (ResponseChecker.Success(response))
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(response.QrCode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Image qrCodeImage = qrCode.GetGraphic(20);

                    using (FileStream fs = new FileStream("Test.jpg", FileMode.OpenOrCreate))
                    {
                        qrCodeImage.SaveAsJpeg(fs);
                    }

                    Console.WriteLine("调用成功");

                }

                else

                {

                    //Console.WriteLine("调用失败，原因：" + response.Msg + "，" + response.SubMsg);

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine("调用遭遇异常，原因：" + ex.Message);

                throw ex;

            }

        }

        static private Config GetConfig()

        {

            return new Config()

            {

                Protocol = "https",

                GatewayHost = "openapi.alipay.com/gateway.do",

                SignType = "RSA2",

                AppId = "2021003184694549",

                // 为避免私钥随源码泄露，推荐从文件中读取私钥字符串而不是写入源码中

                MerchantPrivateKey = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDEdwTpb9yx7VXBhfETz+DSif/X1MwEmDlWHyaoRt4wOEl/C3hInfbrt70AfDXEoZftpjC5vO10a3amHZtUdb/1k8MD0B3uEe7Qqqvzg4M0QDOsV6EwkAFgiDRJTmnmb3axBSqvqhgHtl+m3CtWgzfwvMKSbzQY+cLZR6nFaEemY/vE7ZQk1QE7XRyhgLIz3nym5AqC0lvPYMoxnZ37S1+S7RIj29RS4gm6jxTxzVLR5RYas4mkf6Bvr6onc8E/RMijMWjtCA9sGnbvEe9VU/N1EF3+DjG07qxj1WhUknKCUr6Uoruy5unE0OlNBMHYsXeuPVFLmLNSHqj95zJ6SJiJAgMBAAECggEBAKMzn2/vTay8U/WLt4TWtZMPsejvy36xmUeSXwGAxUfXKi6QL55bImgTXLzPcbbi6Zsv5+ATMkn4jzpU89iIbCqrUV+UfZhrazAJ5wdFozM/de1fafY2Wf5/hreynMQgqb9JZHlCWe3mMLUixhl6rXicxSDxoZxxtcva/QuQoZx0k215vGSa2HIwIXI+IJo/Wzqg+9vg3ofbjjmRNAoy2SB0KUQwPht0CIAeJWSVBryiO7N0ruDiIiwCq459MGtIE6hybqmwAIyPbjtTBNTYstLuLE8aFY7tW8hl4+Vg7c80oS14PGU4LJ2dc7a8fmDEOCLTvV6Q69R29+2wNKjcQIECgYEA4rMy0j+tHZoVn6+44kSkWcQzD0nG6JC9YN1BnnOtYb8Y4YAuePjs7WkoflxHvpgM1XyI+KzWPXnuvoZAxnkr9OLqd0+SkZliR7XYDhwaVcNr3LL3+nCYYClwqE4tFQSet+sro9xCh9sTsSMYujYNDihhLM2Lc9x0hNvIrs2ww1kCgYEA3dtvgHXjUccEe0Eq2obvIu2LF6FbpFVrtjxWer5ErHwoONt/CZREC1ZNXzOCTMtZbcQ2CdVrT4xMuRVgVYWjw0HsxDyofyRPUY+oma2XYdW7qbabzS/UodSi8RyGpvR8p3jZua07C9Gk37PV/UstCgI/LEEntMH0z/taPLOUyLECgYBJNW14EDArmxLYmrqt91TL2FxU15JwkC1YFpuWO+0F39x9FhPr5Rc+I/N7uvlAR4H9f1YMWGRnPfUnedt5PLBBSPZGyV+fvVTLq6G2DMVEZPN0Nid2rN3IUBmdgIF2hCNtaXiSVhL2JH1beuYFXGci509aVJZwOAU40nEgYjRvIQKBgDwMCm/QeIhHv3TEvJ6M6uifNohcyfr+i7q1NgreuKOerxxEfGvRT2FqKGLeBCRY3YmSE7Yxp2vOY22s1XgQRbSxgS3T5R8Uast+gHmnvFNkj/htTloI2ho6/ScZO3Cwt5R0ZymUM2kNgvxxJjf6QuR0mziVIfQQkvw/4bqQOHLhAoGAdT4YZCJ55uSO1DqhYdg8Q1MSPPn+emsr2U1XGN1ynOOtTvxvjl8PqD9kL5QilUQ5ZywjALczXzeHl3EEvU9zk0UE1Nck4xA2xydp2vZq0xeLBRHxYLncWpKlRubov8EelJ3X1kpdWBGGpQ+htbc31L/agiG6f2j+vf2/YkuxsWA=",

                //MerchantCertPath = "<-- 请填写您的应用公钥证书文件路径，例如：/foo/appCertPublicKey_2019******521003.crt -->",

                //AlipayCertPath = "<-- 请填写您的支付宝公钥证书文件路径，例如：/foo/alipayCertPublicKey_RSA2.crt -->",

                //AlipayRootCertPath = "<-- 请填写您的支付宝根证书文件路径，例如：/foo/alipayRootCert.crt -->",

                // 如果采用非证书模式，则无需赋值上面的三个证书路径，改为赋值如下的支付宝公钥字符串即可

                AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjDdzHrt2alBCx97nIUkGbfG9rDnyh+Y+cVIwKjaiA4jGdx5uUK0Jw5GtlR/wNjSeamlNu+1VMqK5JSwGNzXhU8UFcjB2ADgojfpI5yJYecGVm+DgBgJdquj+qKWsP5ta7genAnSfD0bTBnyfhcQ20vRvAEXsFeO3s02+19jPxpZQdV2B6uAldafv80hBauC/5Q+LQMDnq+0IiO4VTUn3uxo5EHjvRxpruv3ThzLnQjNociNPEsCe9cFANiwnPRfttEaORZYghsYOFHb16DoqmzTpgzHeCLsgZrOMWgzsxod5Xm8D5pZDaaUQ+I8z+iLEbVJ/5M6P32MTqF0md1ZVXQIDAQAB",

                //可设置异步通知接收服务地址（可选）

                //NotifyUrl = "<-- 请填写您的支付类接口异步通知接收服务地址，例如：https://www.test.com/callback -->",

                //可设置AES密钥，调用AES加解密相关接口时需要（可选）

                //EncryptKey = "<-- 请填写您的AES密钥，例如：aa4BtZ4tspm2wnXLb1ThQA== -->"

            };

        }

        public Task Pay()
        {
            Test();
            return Task.CompletedTask;
        }
    }

}
