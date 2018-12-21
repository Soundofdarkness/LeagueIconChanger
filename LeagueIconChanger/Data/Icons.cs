using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueIconChanger.Data
{
    public class Icons
    {
        public static List<int> icons
        {
            get
            {
                List<int> data = Enumerable.Range(50, 28).ToList();
                data.Remove(75);
                return data;
            }
        }

        public static String GetIconUrl(int iconId)
        {
            return $"https://cdn.communitydragon.org/latest/profile-icon/{iconId.ToString()}.jpg";
        }

        
    }
}
