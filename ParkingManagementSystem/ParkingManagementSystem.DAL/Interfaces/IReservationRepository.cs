using ParkingManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagementSystem.DAL.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservations();
        Task CreateReservation(Reservation reservation);
        Task EditReservation(Reservation reservation);
        Task DeleteReservation(int id);
        Task<Reservation> GetReservationById(int id);
        Task<List<Reservation>> GetReservationsByUserId(int id);
        Task UpdateStatus(int id);
    }
}
