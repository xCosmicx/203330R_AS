﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Google.Authenticator;

namespace _203330R_AS
{
    [WebService(Namespace = "http://authenticatorapi.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class GoogleAuthenticatorAPI : WebService
    {

        [WebMethod]
        public PairingInfo Pair(string appName, string appInfo, string secretCode)
        {
            var tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode(appName, appInfo, secretCode, 300, 300);
            var html = "<a title='Manually pair with " + setupInfo.ManualEntryKey +
                       "' href='https://www.authenticatorapi.com'><img src='" + setupInfo.QrCodeSetupImageUrl.Replace("http://", "https://") +
                       "' border=0></a>";
            return new PairingInfo { ManualSetupCode = setupInfo.ManualEntryKey, Html = html };
        }

        [WebMethod]
        public bool ValidatePin(string pin, string secretCode)
        {
            var tfa = new TwoFactorAuthenticator();
            var isCorrectPin = tfa.ValidateTwoFactorPIN(secretCode, pin);
            return isCorrectPin;
        }
    }

    public class PairingInfo
    {
        public string ManualSetupCode { get; set; }
        public string Html { get; set; }
    }
}
