﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.BLL.Interfaces;

namespace ParkingManagementSystem.BLL.Services
{
    public class ParkingLotService : IParkingLotService 
    {
        private readonly IParkingLotRepository _parkingLotRepository;
        private readonly IReservationRepository _reservationRepository;

        public ParkingLotService(IParkingLotRepository parkingLotRepository, IReservationRepository reservationRepository)
        {
            _parkingLotRepository = parkingLotRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task UpdateLotAvailability()
        {

        }

        public async Task CreateParkingLot(ParkingLot parkingLot)
        {
            await _parkingLotRepository.CreateParkingLot(parkingLot);
        }

        public async Task<bool> DeleteParkingLot(int id)
        {
            return await _parkingLotRepository.DeleteParkingLot(id);
        }

        public async Task EditParkingLot(ParkingLot parkingLot)
        {
            await _parkingLotRepository.EditParkingLot(parkingLot);
        }

        public async Task<List<ParkingLot>> GetAllLots()
        {
            return await _parkingLotRepository.GetAllLots();
        }
    }
}
