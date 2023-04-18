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
            Config ff;
            return ff = new Config()

            {

                Protocol = "https",

                GatewayHost = "openapi.alipay.com",

                SignType = "RSA2",

                AppId = "2021003184694549",

                // 为避免私钥随源码泄露，推荐从文件中读取私钥字符串而不是写入源码中

                MerchantPrivateKey = "",

                //MerchantCertPath = "<-- 请填写您的应用公钥证书文件路径，例如：/foo/appCertPublicKey_2019******521003.crt -->",

                //AlipayCertPath = "<-- 请填写您的支付宝公钥证书文件路径，例如：/foo/alipayCertPublicKey_RSA2.crt -->",

                //AlipayRootCertPath = "<-- 请填写您的支付宝根证书文件路径，例如：/foo/alipayRootCert.crt -->",

                // 如果采用非证书模式，则无需赋值上面的三个证书路径，改为赋值如下的支付宝公钥字符串即可

                AlipayPublicKey = "",

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
