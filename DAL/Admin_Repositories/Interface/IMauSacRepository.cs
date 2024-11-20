using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin_Repositories.Interface
{
    public interface IMauSacRepository
    {
        List<MauSac> GetAll();
        MauSac GetById(int id);
        bool Add(MauSac obj);
        bool Update(MauSac obj);
        bool Delete(int id);
    }
}
