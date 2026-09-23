using CafePOS.Data.Models;
using CafePOS.Data.Repositories;

namespace CafePOS.Business.Services;

public class MonService
{
    private readonly MonRepository _repo = new MonRepository();

    public List<Mon> LayMenu() => _repo.LayMonDangBan();

    public List<Mon> TimMon(string tuKhoa)
    {
        tuKhoa = tuKhoa.Trim();
        return tuKhoa == "" ? _repo.LayMonDangBan() : _repo.TimKiem(tuKhoa);
    }
        _repo.TimKiem_KhongAnToan(tuKhoa.Trim());
}