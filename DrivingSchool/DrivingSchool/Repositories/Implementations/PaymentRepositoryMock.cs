using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class PaymentRepositoryMock : IPaymentRepository
    {
        private readonly List<Payment> _payments = new List<Payment>();

        public IEnumerable<Payment> ReadPayments() => _payments;

        public void CreatePayment(Payment payment)
        {
            _payments.Add(payment);
        }

        public void DeletePayment(int id)
        {
            // Убедитесь, что у сущности Payment есть свойство PaymentId
            var payment = _payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment != null)
            {
                _payments.Remove(payment);
            }
        }
    }
}