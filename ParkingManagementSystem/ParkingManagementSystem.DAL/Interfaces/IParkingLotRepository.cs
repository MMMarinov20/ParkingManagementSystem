using ParkingManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementSystem.DAL.Interfaces
{
    public interface IParkingLotRepository
    {
        Task<List<ParkingLot>> GetAllLots();
        Task CreateParkingLot(ParkingLot parkingLot);
        Task EditParkingLot(ParkingLot parkingLot);
        Task<bool> DeleteParkingLot(int id);
        Task UpdateLotAvailability(int id, bool isAvailable);
    }
}
