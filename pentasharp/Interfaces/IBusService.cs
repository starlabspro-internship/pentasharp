using pentasharp.ViewModel.Bus;
using System.Threading.Tasks;

namespace pentasharp.Interfaces
{
    public interface IBusService
    {
        Task<List<BusViewModel>> GetBusesAsync(int userId);
        Task<bool> AddBusAsync(AddBusViewModel model, int userId);
        Task<bool> EditBusAsync(int id, EditBusViewModel model, int userId);
        Task<bool> DeleteBusAsync(int id, int userId);
    }
}