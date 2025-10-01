using System;
using System.Collections.Generic;
using EquipShare.Models;
using EquipShare.Models.ViewModels;

namespace EquipShare.Services
{
    public interface IBookingService
    {
        Booking CreateBooking(BookingCreateModel model, int renterId);
        List<Booking> GetUserBookings(int userId);
        List<Booking> GetOwnerBookings(int ownerId);
        Booking GetBookingById(int id);
        bool UpdateBookingStatus(int bookingId, string status, int ownerId);
        bool IsEquipmentAvailable(int equipmentId, DateTime startDate, DateTime endDate);
        decimal CalculateTotalPrice(int equipmentId, DateTime startDate, DateTime endDate);
        (decimal EquipmentCost, decimal PlatformCost, decimal OwnerReceivableAmount, decimal TotalPrice) CalculatePriceBreakdown(int equipmentId, DateTime startDate, DateTime endDate);
        List<object> GetBookedDates(int equipmentId);
    }
}