using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class PaymentRepository : EfRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(Payment.PaymentId), x => x.PaymentId)
        {
        }

        public IEnumerable<Payment> ReadPayments() => ReadAll(nameof(ReadPayments));

        public void CreatePayment(Payment payment) => Create(nameof(CreatePayment), payment);

        public void DeletePayment(int id) => Delete(nameof(DeletePayment), id);
    }
}
