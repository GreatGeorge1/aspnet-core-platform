using System;
using Abp.BackgroundJobs;
using Abp.Dependency;
using JetBrains.Annotations;
using Platform.Orders;
using Platform.Payment.Models;

namespace Platform.Background
{
    public class EasyPayProcessNotifyJob:BackgroundJob<EasyPayProcessNotifyArgs>, ITransientDependency
    {
        [NotNull] private readonly IOrderManager _orderManager;

        public EasyPayProcessNotifyJob([NotNull] IOrderManager orderManager)
        {
            _orderManager = orderManager ?? throw new ArgumentNullException(nameof(orderManager));
        }

        public override async void Execute(EasyPayProcessNotifyArgs args)
        {
            await _orderManager.CompleteOrder(args.Notify, args.Body, args.Sign);
            
        }
    }

    public class EasyPayProcessNotifyArgs
    {
        public string Sign { get; set; }
        public string Body { get; set; }
        public EasyPayNotify Notify { get; set; }
    }
}