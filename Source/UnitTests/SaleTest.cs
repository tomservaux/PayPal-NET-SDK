﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayPal.Api;
using System;
using System.Net;

namespace PayPal.UnitTest
{
    [TestClass]
    public class SaleTest
    {
        public static readonly string SaleJson =
            "{\"amount\":" + AmountTest.AmountJson + "," +
            "\"parent_payment\":\"103\"," +
            "\"state\":\"Approved\"," +
            "\"create_time\":\"2013-01-17T18:12:02.347Z\"," +
            "\"links\":[" + LinksTest.LinksJson + "]}";

        public static Sale GetSale()
        {
            return JsonFormatter.ConvertFromJson<Sale>(SaleJson);
        }

        [TestMethod]
        public void SaleObjectTest()
        {
            var sale = GetSale();
            Assert.AreEqual("103", sale.parent_payment);
            Assert.AreEqual("Approved", sale.state);
            Assert.AreEqual("2013-01-17T18:12:02.347Z", sale.create_time);
            Assert.IsNotNull(sale.amount);
            Assert.IsNotNull(sale.links);
        }

        [TestMethod]
        public void SaleNullIdTest()
        {
            UnitTestUtil.AssertThrownException<System.ArgumentNullException>(() => Sale.Get(UnitTestUtil.GetApiContext(), null));
        }

        [TestMethod]
        public void SaleConvertToJsonTest()
        {
            Assert.IsFalse(GetSale().ConvertToJson().Length == 0);
        }

        [TestMethod]
        public void SaleToStringTest()
        {
            Assert.IsFalse(GetSale().ToString().Length == 0);
        }

        [TestMethod]
        public void SaleGetTest()
        {
            var saleId = "4V7971043K262623A";
            var sale = Sale.Get(UnitTestUtil.GetApiContext(), saleId);
            Assert.IsNotNull(sale);
            Assert.AreEqual(saleId, sale.id);
        }

        [TestMethod]
        public void SaleRefundTest()
        {
            // Create a credit card sale payment
            var payment = PaymentTest.CreatePaymentForSale();
            
            // Get the sale resource
            var sale = payment.transactions[0].related_resources[0].sale;

            var refund = new Refund
            {
                amount = new Amount
                {
                    currency = "USD",
                    total = "0.01"
                }
            };

            var response = sale.Refund(UnitTestUtil.GetApiContext(), refund);
            Assert.IsNotNull(response);
            Assert.AreEqual("completed", response.state);
        }
    }
}
