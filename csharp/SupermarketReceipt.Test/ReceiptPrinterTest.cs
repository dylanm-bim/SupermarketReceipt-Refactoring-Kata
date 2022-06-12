
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace SupermarketReceipt.Test
{
    [UsesVerify]
    public class ReceiptPrinterTest
    {
        readonly Product _toothbrush = new Product("toothbrush", ProductUnit.Each);
        readonly Product _apples = new Product("apples", ProductUnit.Kilo);
        Receipt _receipt = new Receipt();

        [Fact]
        public Task oneLineItem()
        {
            _receipt.AddProduct(_toothbrush, 1, 0.99, 0.99);
            return VerifyReceipt(_receipt);
        }
        
        [Fact]
        public Task quantityTwo()
        {
            _receipt.AddProduct(_toothbrush, 2, 0.99, 0.99 * 2);
            return VerifyReceipt(_receipt);
        }
        
        [Fact]
        public Task looseWeight()
        {
            _receipt.AddProduct(_apples, 2.3, 1.99, 1.99 * 2.3);
            return VerifyReceipt(_receipt);
        }

        [Fact]
        public Task total()
        {

            _receipt.AddProduct(_toothbrush, 1, 0.99, 2 * 0.99);
            _receipt.AddProduct(_apples, 0.75, 1.99, 1.99 * 0.75);
            return VerifyReceipt(_receipt);
        }

        [Fact]
        public Task discounts()
        {
            _receipt.AddDiscount(new Discount(_apples, "3 for 2", 0.99));
            return VerifyReceipt(_receipt);
        }

        [Fact]
        public Task printWholeReceipt()
        {
            _receipt.AddProduct(_toothbrush, 1, 0.99, 0.99);
            _receipt.AddProduct(_toothbrush, 2, 0.99, 2 * 0.99);
            _receipt.AddProduct(_apples, 0.75, 1.99, 1.99 * 0.75);
            _receipt.AddDiscount(new Discount(_toothbrush, "3 for 2", 0.99));
            
            return VerifyReceipt(_receipt);
        }

        public static Task VerifyReceipt(Receipt receipt)
        {
            var printout = new ReceiptPrinter().PrintReceipt(receipt);
            var settings = new VerifySettings();
            settings.ScrubLinesContaining("Date:");
            return Verifier.Verify(target: printout, settings: settings);
        }
    }
}