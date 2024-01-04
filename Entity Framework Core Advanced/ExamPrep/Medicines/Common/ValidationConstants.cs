using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Medicines.Common
{
    public static class ValidationConstants
    {
        //Pharmacy
        public const int PharmacyNameMaxLenght = 50;
        public const int pharmacyMin = 2;
        public const int PharmacyPhoneNuMBERLenght = 14;
        public const string RegexPhoneNumber= @"(\(\d{3}\) )\d{3}-\d{4}";

        public const int NameMin = 5;
        public const int GenderMin = 0;
            public const int genderMax = 1;

        public const int AgeMin = 0;
        public const int AgeMAX = 2;

        //Medicine
        public const int MedicinenameMaxlenght = 150;
        public const int MedicineNameMin = 3;
        public const int ProducerMAxLENGHT = 100;
        public const int ProsucerMiN = 3;
        //Patient
        public const int PatientFullNmaemaxLength = 100;

        //category
        public const int CATEGORYMIN = 0;
        public const int categoryMax = 4;
        //pricemedicne
        public const double PriceMin = 0.01;
        public const double PriceMax = 1000.00;
    }
}
