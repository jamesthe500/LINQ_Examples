using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    // this class will represent the car database
    // derives from the class in the entityFW DbContext
    public class CardDb : DbContext
    {
        // this represents a table in the DB.  
        // by convention, it will know to name the table "Cars"
        // also, it will try to connect to the localdb instance without instruction
        // it will also assume we want the data in a db of teh same name as teh class, i.e. "Cars.CarsDb"
        // it will also make columns based on the definition of Car provided by the schema
        public DbSet<Car> Cars { get; set; }
    }
}
