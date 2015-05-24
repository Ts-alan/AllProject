using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCP.DAL.DataModels;
using CCP.WebApi.Models;

namespace CCP.WebApi.Helpers
{
    public static class AreaModelBuilder
    {
        public static List<AreaModel> Build(List<AreaRoleView> source)
        {
            var areaModelList = new List<AreaModel>();
            var groupedAreas = source.GroupBy(s => s.AreaName).ToList();
            foreach (var group in groupedAreas)
            {
                var areaModel = new AreaModel { AreaName = group.Key, Roles = new List<string>() };
                foreach (var areaRole in group)
                {
                    areaModel.Roles.Add(areaRole.RoleName);
                }
                areaModelList.Add(areaModel);
            }
            return areaModelList;
        }
    }
}