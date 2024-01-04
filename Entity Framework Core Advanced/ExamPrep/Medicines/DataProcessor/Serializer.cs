namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            throw new NotImplementedException();
        }

     //   public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
      //  {


      //  var medicines = context.Medicines
            //    .Where(m => m.Category == (Category)medicineCategory)
              //  .Where(p => p.Pharmacy.IsNonStop == true)
             //   .Select(m => new
             //   {
                 //   Name = m.Name,
                 //   Price = m.Price.ToString("f2"),
                 //   Pharmacy = m.Pharmacy
                  ///  .Select(p => new
                 //   {
                    //    Name=p.Name,
                    //    PhoneNumber=p.PhoneNumber,

                  //  })

               // })



        }
    }

