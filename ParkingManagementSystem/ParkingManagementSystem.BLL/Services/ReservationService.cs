using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;
using ParkingManagementSystem.DAL.Repositories;

namespace ParkingManagementSystem.BLL.Services
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;

        public ReservationService(ReservationRepository reservationRepository)
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
    }
}
