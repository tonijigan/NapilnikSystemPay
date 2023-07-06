using System;
using System.Collections.Generic;

namespace IMJunior
{
    public interface IPaySystem
    {
        void PaymentOperation();
    }

    class PaySystem : IPaySystem
    {
        public string Name { get; private set; }

        public PaySystem(string name)
        {
            Name = name;
        }

        public void PaymentOperation()
        {
            OnPaymentOperation();
        }

        protected virtual void OnPaymentOperation() { }
    }

    class QIWI : PaySystem
    {
        public QIWI(string name) : base(name) { }

        protected override void OnPaymentOperation()
        {
            Console.WriteLine($"Перевод на страницу {Name}...");
        }
    }

    class WebMoney : PaySystem
    {
        public WebMoney(string name) : base(name) { }

        protected override void OnPaymentOperation()
        {
            Console.WriteLine($"Вызов API {Name}...");
        }
    }

    class Card : PaySystem
    {
        public Card(string name) : base(name) { }

        protected override void OnPaymentOperation()
        {
            Console.WriteLine($"Вызов API банка эмиттера карты {Name}...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<PaySystem> paySystems = new List<PaySystem>();
            paySystems.Add(new QIWI("QIWI"));
            paySystems.Add(new WebMoney("WebMoney"));
            paySystems.Add(new Card("Card"));

            var orderForm = new OrderForm(paySystems);
            var paymentHandler = new PaymentHandler(paySystems);

            orderForm.ShowForm();
            orderForm.SystemOperation();
            //if (systemId == "QIWI")
            //    Console.WriteLine("Перевод на страницу QIWI...");
            //else if (systemId == "WebMoney")
            //    Console.WriteLine("Вызов API WebMoney...");
            //else if (systemId == "Card")
            //    Console.WriteLine("Вызов API банка эмитера карты Card...");

            // paymentHandler.ShowPaymentResult(systemId);
        }
    }

    class OrderForm
    {
        private List<PaySystem> _paySystems;

        public OrderForm(List<PaySystem> paySystems)
        {
            _paySystems = paySystems;
        }

        public void ShowForm()
        {
            Console.WriteLine("Мы принимаем: ");

            foreach (var paySystem in _paySystems)
            {
                Console.WriteLine(paySystem.Name);
            }
        }

        public void SystemOperation()
        {
            if (TryGetSystemPay(out PaySystem paySystem) == true)
                paySystem.PaymentOperation();
            else
                Console.WriteLine("Вы ввели не верные данные!");
        }

        private bool TryGetSystemPay(out PaySystem paySystem)
        {
            paySystem = null;
            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            string systemName = Console.ReadLine();

            foreach (var systemPay in _paySystems)
            {
                if (systemName.ToLower() == systemPay.Name.ToLower())
                    paySystem = systemPay;
            }

            return paySystem != null;
        }
    }

    class PaymentHandler
    {
        private List<PaySystem> _paySystem;

        public PaymentHandler(List<PaySystem> paySystems) => _paySystem = paySystems;

        public void ShowPaymentResult(string systemId)
        {
            Console.WriteLine($"Вы оплатили с помощью {systemId}");

            if (systemId == "QIWI")
                Console.WriteLine("Проверка платежа через QIWI...");
            else if (systemId == "WebMoney")
                Console.WriteLine("Проверка платежа через WebMoney...");
            else if (systemId == "Card")
                Console.WriteLine("Проверка платежа через Card...");

            Console.WriteLine("Оплата прошла успешно!");
        }
    }
}