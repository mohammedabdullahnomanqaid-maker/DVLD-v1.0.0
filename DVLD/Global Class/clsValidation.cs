using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Course19
{
    class clsValidation
    {
        public static bool ValidateEmail(string EmailAddress)
        {

            // ^ start from here and prevent to start before it
            // a-z from letter a to letter z allow small
            // A-Z from letter A to letter Z allow Capital
            // 0-9 form digit 0 to digit 9
            // .  
            //.!#$%&'*+-/=?^_`{|}~  it is ok if you use them
            // [ ] choose one of the letters inside it just
            //[ ] + when add + this mean choose one or more than one of the letters inside the prackt
            // @ because of this write outside the prackt so you should write it is mandatory
            // . that's mean any letter
            // \. but here mean you should add dot(.) it is a mandatory
            // (?:\.[a-zA-Z0-9-]+) this mean should have dot and com or net or etc
            // * when we add star we make it allow more than sub domain Ex: @email.google.com.etc or "" none
            // $ that's mean the text ended

            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            var regex = new Regex(pattern);
            return regex.IsMatch(EmailAddress);
        }
    }
}
