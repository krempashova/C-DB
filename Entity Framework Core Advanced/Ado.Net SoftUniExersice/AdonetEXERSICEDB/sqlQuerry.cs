using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdonetEXERSICEDB
{
    public static class sqlQuerry
    {


        public const string GetVilliansWithMinions = @" SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                                        FROM Villains AS v 
                                                        JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                                        GROUP BY v.Id, v.Name 
                                                          HAVING COUNT(mv.VillainId) > 3 
                                                        ORDER BY COUNT(mv.VillainId)";

        public const string GetMinionsNamesByid = @"SELECT Name FROM Villains WHERE Id = @Id";
        public const string GetOrderedMinionsNamesByVilianiD = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS RowNum,
                                                        m.Name, 
                                                        m.Age
                                                        FROM MinionsVillains AS mv
                                                        JOIN Minions As m ON mv.MinionId = m.Id
                                                        WHERE mv. = @Id
                                                        ORDER BY m.Name";

    }
}
