﻿using DTO.VuvietanhDTO.Cuahangs;
using DTO.VuvietanhDTO.Danhmucs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.VuVietAnhService.IRepository.IDanhmuc
{
    public interface IDanhmucService
    {
        Task<IEnumerable<FullDanhMucDTO>> GetAllDanhMuc();
        Task<DanhMucDTO> GetDanhMucById(int id);
        Task<CreatDanhMucDTO> CreateDanhMuc(CreatDanhMucDTO creatDanhMucDTO);
        Task<UpdateDanhMucDTO> UpdateDanhMuc(int id, UpdateDanhMucDTO updateDanhMucDTO);
        Task<bool> DeleteDanhMuc( int id );
        Task<int> GetIdByTenDanhMuc(string tenDanhMuc);
    }
}
