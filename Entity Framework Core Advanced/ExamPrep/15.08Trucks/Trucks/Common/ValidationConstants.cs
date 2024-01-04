using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Common
{
    public static class ValidationConstants
    {

        public const int TruckRegistrationNumerLength = 8;
        public const int TruckVinNumberlenght = 17;
        public const string TruckRegistrationNumberregex =
            @"[A-Z]{2}\d{4}[A-Z]{2}";

        public const int Clientnamemaxlebngth = 40;
        public const int CLIENTNamemin = 3;
        public const int ClientNationalityMaxlength = 40;
        public const int clientnationalityMIN = 2;
        public const int DespecherNameMaxLenght = 40;
        public const int DespacherMinLenght = 2;
       

        public const int TankCapacityMinLength = 950;
        public const int TankCapacityMaxlenght = 1420;
        public const int CargoCapacityMinlength = 5000;
        public const int CargoCapacityMaxLength = 29000;

        public const int categoryTypeminlenght = 0;
        public const int categoryTypeMAXLENGTH = 3;
        public const int MakeTypemin = 0;
        public const int MakeTypemax = 4;
    }
}
