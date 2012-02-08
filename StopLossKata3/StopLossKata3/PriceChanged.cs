using System;

namespace StopLossKata3
{
    public class PriceChanged : Message
    {
        public readonly Guid PriceId;

        public readonly decimal Price;

        public PriceChanged(decimal price, Guid priceId)
        {
            PriceId = priceId;
            Price = price;
        }
    }
}