using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;

namespace ParkingManagementSystem.BLL.Interfaces
{
    public interface IParkingLotService
    {
        Task UpdateLotAvailability(int id, bool isAvailable);
        Task<List<ParkingLot>> GetAllLots();
        Task CreateParkingLot(ParkingLot parkingLot);
        Task EditParkingLot(ParkingLot parkingLot);
        Task<bool> DeleteParkingLot(int id);
    }
}
