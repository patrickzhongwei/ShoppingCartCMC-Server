using FakeItEasy;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Shared;
using ShoppingCartCMC.Shared.DTO;
using System;
using Xunit;

namespace ShoppingCartCMC.Test
{
    public class ProductControllerTest
    {
        //PW: we simple test Repository, as Controller ust Repository all calls to its Repository.

        [Fact]
        public async void Get()
        {
            //Arrange
            var repo = A.Fake<iProductRepository>();
            var dto = A.Fake<iProductDto>();
            var mockData = MockData.MockProductsInBaseCcy;

            //ACT
            var actionResult = await repo.Get("L1HnndxVc2-KaJ10Skc", "AUD");

            //Assert
            Assert.NotNull(actionResult);
        }
    }
}
