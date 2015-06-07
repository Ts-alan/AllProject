using Crystal.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crystal.DomainModel;

namespace Crystal.Models
{
    public static class BusinessLogicFactory
    {
        public static ICrystalLogic GetBL(Plants plant = Plants.plant_none)
        {
            if (plant == Plants.plant_none)
                plant = GetPlant();
#if DEBUG
            return new WcfLogic.WcfLogics(plant);
#else
            return new Crystal.VFPOleDBLogic.BusinessLogicOleDb(plant);
#endif

        }
        public static Plants GetPlant(Plants plant = Plants.plant_none)
        {
            string plant_name = plant.ToString();
            if (plant == Plants.plant_none)
                plant_name = HttpContext.Current.Request.RequestContext.RouteData.Values["plant"] as string;

            // Преобразование строки в перечисление. 
            Plants pl = Plants.plant_none;
            if (plant_name != null)
            {
                if (Enum.IsDefined(typeof(Plants), plant_name))
                    pl = (Plants)Enum.Parse(typeof(Plants), plant_name, true);
                else
                    throw new InvalidCastException(string.Format("Error PlantName\nCheck Plants enum for {0}",
                                                                 plant_name));
            }
            return pl;
        }
    }
}