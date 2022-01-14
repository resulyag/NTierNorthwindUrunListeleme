using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Tools
{
    public class DbInstance
    {
        private static NorthwindEntities _instance;
        public static NorthwindEntities Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NorthwindEntities();
                }
                return _instance;
            }
        }
    }
}
