using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;
using ParkingManagementSystem.BLL.Interfaces;

namespace ParkingManagementSystem.BLL.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<bool> CreateReservation(Reservation reservation)
        {
            if (reservation == null) return false;

            await _reservationRepository.CreateReservation(reservation);

            return true;
        }

        public async Task<bool> DeleteReservation(int id)
        {
            if (id <= 0) return false;

            await _reservationRepository.DeleteReservation(id);

            return true;
        }

        public async Task<bool> EditReservation(Reservation reservation)
        {
            if (reservation == null) return false;

            await _reservationRepository.EditReservation(reservation);

            return true;
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            if (id <= 0) return null;

            return await _reservationRepository.GetReservationById(id);
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            return await _reservationRepository.GetAllReservations();
        }

        public async Task<List<Reservation>> GetReservationsByUserId(int id)
        {
            if (id <= 0) return null;

            return await _reservationRepository.GetReservationsByUserId(id);
        }

        public async Task UpdateStatus(int id)
        {
            if (id <= 0) return;

            await _reservationRepository.UpdateStatus(id);
        }
    }
}
