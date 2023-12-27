using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;

namespace ParkingManagementSystem.BLL.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotRepository _parkingLotRepository;

        public ParkingLotService(ParkingLotRepository parkingLotRepository)
        {
            _parkingLotRepository = parkingLotRepository;
        }

        public async Task<bool> CreateParkingLot(ParkingLot parkingLot)
        {
            if (parkingLot == null) return false;

            await _parkingLotRepository.CreateParkingLot(parkingLot);

            return true;
        }

        public async Task<bool> DeleteParkingLot(int id)
        {
            if (id <= 0) return false;

            await _parkingLotRepository.DeleteParkingLot(id);

            return true;
        }

        public async Task<bool> EditParkingLot(ParkingLot parkingLot)
        {
            if (parkingLot == null) return false;

            await _parkingLotRepository.EditParkingLot(parkingLot);

            return true;
        }
    }
}
