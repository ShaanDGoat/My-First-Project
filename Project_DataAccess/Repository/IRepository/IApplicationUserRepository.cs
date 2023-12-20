using Project_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DataAccess.Repository.IRepository
{
    public interface IApplicationRepository : IRepository<ApplicationType>
    {
        void Update(ApplicationType obj);
    }
}
