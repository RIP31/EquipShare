using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EquipShare.Data;
using EquipShare.Models;
using EquipShare.Models.ViewModels;

namespace EquipShare.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Booking CreateBooking(BookingCreateModel model, int renterId)
        {
            // Validate that the EquipmentId exists
            var equipment = _context.Equipment.Find(model.EquipmentId);
            if (equipment == null)
            {
                throw new ArgumentException($"Equipment with ID {model.EquipmentId} does not exist.");
            }

            var booking = new Booking
            {
                EquipmentId = model.EquipmentId,
                RenterId = renterId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                TotalPrice = model.TotalPrice,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return booking;
        }

        public List<Booking> GetUserBookings(int userId)
        {
            return _context.Bookings
                .Include(b => b.Equipment)
                .ThenInclude(e => e.Owner)
                .Where(b => b.RenterId == userId)
                .OrderByDescending(b => b.CreatedDate)
                .ToList();
        }

        public List<Booking> GetOwnerBookings(int ownerId)
        {
            return _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Renter)
                .Where(b => b.Equipment.OwnerId == ownerId)
                .OrderByDescending(b => b.CreatedDate)
                .ToList();
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.Equipment)
                .Include(b => b.Renter)
                .FirstOrDefault(b => b.Id == id);
        }

        public bool UpdateBookingStatus(int bookingId, string status, int ownerId)
        {
            var booking = _context.Bookings
                .Include(b => b.Equipment)
                .FirstOrDefault(b => b.Id == bookingId && b.Equipment.OwnerId == ownerId);

            if (booking == null)
                return false;

            booking.Status = status;
            _context.Bookings.Update(booking);
            _context.SaveChanges();

            return true;
        }

        public bool IsEquipmentAvailable(int equipmentId, DateTime startDate, DateTime endDate)
        {
            return !_context.Bookings.Any(b =>
                b.EquipmentId == equipmentId &&
                b.Status != "Rejected" &&
                ((startDate >= b.StartDate && startDate <= b.EndDate) ||
                 (endDate >= b.StartDate && endDate <= b.EndDate) ||
                 (startDate <= b.StartDate && endDate >= b.EndDate)));
        }

        public decimal CalculateTotalPrice(int equipmentId, DateTime startDate, DateTime endDate)
        {
            var equipment = _context.Equipment.Find(equipmentId);
            if (equipment == null) return 0;

            var days = (endDate - startDate).Days + 1;
            return equipment.PricePerDay * days;
        }

        public List<object> GetBookedDates(int equipmentId)
        {
            return _context.Bookings
                .Where(b => b.EquipmentId == equipmentId && b.Status != "Rejected")
                .Select(b => new { start = b.StartDate, end = b.EndDate })
                .ToList<object>();
        }
    }
}