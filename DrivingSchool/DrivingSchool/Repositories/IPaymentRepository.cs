using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface IPaymentRepository
    {
        IEnumerable<Payment> ReadPayments();
        void CreatePayment(Payment payment);
        void DeletePayment(int id);
    }
}