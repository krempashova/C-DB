namespace Invoices.Shared
{
    public static class GlobalConstants
    {
        public const int ProductNameMaxLength = 30;
        public const int ProductNameMinLength = 9;
        public const decimal ProductPriceMaxValue = 1000m;
        public const decimal ProductPriceMinValue = 5m;

        public const int AddressStreetNameMaxLength = 20;
        public const int AddressStreetNameMinLength = 10;
        public const int AddressCityMaxLength = 15;
        public const int AddressCityMinLength = 5;
        public const int AddressCountryMaxLength = 15;
        public const int AddressCountryMinLength = 5;

        public const int InvoiceNumberMaxValue = 1500000000;
        public const int InvoiceNumberMinValue = 1000000000;

        public const int ClientNameMaxLength = 25;
        public const int ClientNameMinLength = 10;
        public const int ClientNumberVatMaxLength = 15;
        public const int ClientNumberVatMinLength = 10;
    }
}
