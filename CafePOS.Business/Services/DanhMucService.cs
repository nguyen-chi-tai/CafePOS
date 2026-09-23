using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class DanhMucService
{
    private readonly DanhMucRepository _repo = new();

    public List<DanhMuc> LayTatCa() => _repo.LayTatCa();
}