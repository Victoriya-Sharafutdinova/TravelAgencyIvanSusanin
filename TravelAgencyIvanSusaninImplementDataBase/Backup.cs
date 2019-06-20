using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgencyIvanSusaninImplementDataBase
{
    public static class Backup
    {
        public static void SaveEntity(IEnumerable entity)
        {
            var jsonFormatter = new DataContractJsonSerializer(entity.GetType());

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/backup"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/backup");
            }

            using (var fs = new FileStream(Directory.GetCurrentDirectory() + $"/backup/{GetNameEntity(entity)}.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, entity);
            }
        }

        private static string GetNameEntity(IEnumerable entity)
        {
            return entity.AsQueryable().ElementType.ToString().Split('.')[1];
        }
    }
}
