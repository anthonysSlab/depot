using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depot
{
    public class Badbois
    {
        public List<Badboi> Badboiz { get; set; } = new();

        public static Badbois Ins { get; } = Load();

        public Badboi GetOrAdd(ulong id)
        {
            Badboi? badboi = Badboiz.Find(x => x.Id == id);

            if (badboi == null)
            {
                badboi = new(id);
                Badboiz.Add(badboi);
            }
            return badboi;
        }

        public Badboi? Get(IUser user)
        {
            return Badboiz.FirstOrDefault(x => x.Id == user.Id);
        }

        internal void Save()
        {
            File.WriteAllText("badbois.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        internal static Badbois Load()
        {
            if (File.Exists("badbois.json"))
            {
                return JsonConvert.DeserializeObject<Badbois>(File.ReadAllText("badbois.json")) ?? new();
            }
            else
            {
                Badbois dad = new();
                dad.Save();
                return dad;
            }
        }
    }
}