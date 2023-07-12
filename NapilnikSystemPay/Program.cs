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

    class QIWI : IPaySystem
    {
        public string Name { get; private set; }
        public QIWI()
        {
            Name = "QIWI";
        }

        public void PaymentOperation()
        {
            Console.WriteLine($"Перевод на страницу {Name}...");
        }
    }

    class WebMoney : IPaySystem
    {
        public string Name { get; private set; }
        public WebMoney()
        {
            Name = "WebMoney";
        }

        public void PaymentOperation()
        {
            Console.WriteLine($"Вызов API {Name}...");
        }
    }

    class Card : IPaySystem
    {
        public string Name { get; private set; }
        public Card()
        {
            Name = "Card";
        }

        public void PaymentOperation()
        {
            Console.WriteLine($"Вызов API банка эмиттера карты {Name}...");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<IPaySystem> paySystems = new List<IPaySystem>();
            paySystems.Add(new QIWI());
            paySystems.Add(new WebMoney());
            paySystems.Add(new Card());

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
        private List<IPaySystem> _paySystems;

        public OrderForm(List<IPaySystem> paySystems)
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
            if (TryGetSystemPay(out IPaySystem paySystem) == true)
                paySystem.PaymentOperation();
            else
                Console.WriteLine("Вы ввели не верные данные!");
        }

        private bool TryGetSystemPay(out IPaySystem paySystem)
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
        private List<IPaySystem> _paySystem;

        public PaymentHandler(List<IPaySystem> paySystems) => _paySystem = paySystems;

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