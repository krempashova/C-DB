using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Common
{
    public static class Validations
    {

        //Product
        public const int NameMaxLength = 30;
        public const int NameMinLenght = 9;

        //Adress
        public const int AdresStreetNameMaxlength = 20;
        public const int CityMaxLengt = 15;
        public const int AdressStreetNAMEmINlENGHT = 10;
        public const int CityMINLengt = 5;


        //Client
        public const int ClientNameMaxLengt = 25;
        public const int ClientnumberVatmaxlenght = 15;
        public const int ClientNameMinLength = 10;
        public const int ClientNumberVatMinLenght = 10;

        // iNVOICES

        public const int InvoiceNumberMin = 1000000000;
        public const int InvoiceNumberMax = 1500000000;
        public const int CurrencyTypeMin = 0;
        public const int CurencyTypeMax = 2;
    }
}
