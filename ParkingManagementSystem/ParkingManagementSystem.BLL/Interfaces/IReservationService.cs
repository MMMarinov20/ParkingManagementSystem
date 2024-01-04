using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingManagementSystem.DAL.Models;

namespace ParkingManagementSystem.BLL.Interfaces
{
    public interface IReservationService
    {
        Task<bool> CreateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int id);
        Task<bool> EditReservation(Reservation reservation);
        Task<Reservation> GetReservationById(int id);
        Task<List<Reservation>> GetAllReservations();
        Task<List<Reservation>> GetReservationsByUserId(int id);
    }
}
