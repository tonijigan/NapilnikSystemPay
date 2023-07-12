using System;
using System.Collections.Generic;

namespace IMJunior
{
    public interface IPaySystem
    {
        string Name { get; }

        void PaymentOperation();
    }

    public interface IPaySystemCreate
    {
        IPaySystem Create();
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<IPaySystem> paySystems = new List<IPaySystem> { new QIWI(), new WebMoney(), new Card() };
            var orderForm = new OrderForm(paySystems);
            orderForm.ShowForm();
            var newSystemPayCreate = new SystemPayCreate(paySystems).Create();
            newSystemPayCreate?.PaymentOperation();
            var paymentHandler = new PaymentHandler(paySystems);
            paymentHandler.ShowPaymentResult(newSystemPayCreate);
        }
    }

    class QIWI : IPaySystem
    {
        public string Name { get; private set; }

        public QIWI() => Name = "QIWI";

        public void PaymentOperation() => Console.WriteLine($"\nПеревод на страницу {Name}...\n");
    }

    class WebMoney : IPaySystem
    {
        public string Name { get; private set; }

        public WebMoney() => Name = "WebMoney";

        public void PaymentOperation() => Console.WriteLine($"\nВызов API {Name}...\n");
    }

    class Card : IPaySystem
    {
        public string Name { get; private set; }

        public Card() => Name = "Card";

        public void PaymentOperation() => Console.WriteLine($"\nВызов API банка эмиттера карты {Name}...\n");
    }

    class SystemPayCreate : IPaySystemCreate
    {
        private List<IPaySystem> _paySystems;

        public SystemPayCreate(List<IPaySystem> paySystems) => _paySystems = paySystems;

        public IPaySystem Create()
        {
            if (TryGetSystemPay(out IPaySystem paySystem) == true)
                return paySystem;

            return null;
        }

        private bool TryGetSystemPay(out IPaySystem paySystem)
        {
            paySystem = null;
            Console.WriteLine("Какое системой вы хотите совершить оплату?\n");
            string systemName = Console.ReadLine();

            foreach (var systemPay in _paySystems)
            {
                if (systemName.ToLower() == systemPay.Name.ToLower())
                    paySystem = systemPay;
            }

            return paySystem != null;
        }
    }

    class OrderForm
    {
        private List<IPaySystem> _paySystems;

        public OrderForm(List<IPaySystem> paySystems) => _paySystems = paySystems;

        public void ShowForm()
        {
            Console.WriteLine("Мы принимаем:\n");

            foreach (var paySystem in _paySystems) Console.WriteLine(paySystem.Name + "\n");
        }
    }

    class PaymentHandler
    {
        private List<IPaySystem> _paySystems;

        public PaymentHandler(List<IPaySystem> paySystems) => _paySystems = paySystems;

        public void ShowPaymentResult(IPaySystem paySystemCreated)
        {
            if (PaymentVerification(paySystemCreated) == true)
            {
                Console.WriteLine($"Вы оплатили с помощью {paySystemCreated.Name}");
                Console.WriteLine($"Проверка платежа через {paySystemCreated.Name} ...");
                Console.WriteLine("Оплата прошла успешно!\n");
            }
        }

        private bool PaymentVerification(IPaySystem paySystemCreated)
        {
            foreach (var paySystem in _paySystems)
            {
                if (paySystem == paySystemCreated)
                    return true;
            }
            return paySystemCreated != null;
        }
    }
}