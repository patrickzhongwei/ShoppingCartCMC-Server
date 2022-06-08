using FakeItEasy;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Shared.DTO;
using System;
using Xunit;

namespace ShoppingCartCMC.Test
{
    public class BillingController
    {
        //PW: we simple test Repository, as Controller ust Repository all calls to its Repository.

        [Fact]
        public async void Put()
        {
            //Arrange
            var repo = A.Fake<iBillingRepository>();
            var dto = A.Fake<iBillingDto>();

            //ACT
            var actionResult = await repo.PlaceOrder(dto);

            //Assert
            int actual = 12345;
            int expect = 12345;
            //Assert.Equal(expect, actual);
        }
    }
}
