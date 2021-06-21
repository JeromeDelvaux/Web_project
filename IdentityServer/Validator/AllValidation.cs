using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Validator
{
    public class AllValidation
    {
        public bool ValidationNumTel(string newValue)
        {
            try
            {
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(newValue, "BE");
                return phoneNumberUtil.IsValidNumber(phoneNumber);
            }
            catch (PhoneNumbers.NumberParseException)
            {
                return false;
            }
        }
    }
}
