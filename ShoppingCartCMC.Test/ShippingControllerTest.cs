using FakeItEasy;
using ShoppingCartCMC.Server.Shared.Billing;
using ShoppingCartCMC.Server.Shared.Product;
using ShoppingCartCMC.Server.Shared.Shipping;
using ShoppingCartCMC.Shared;
using ShoppingCartCMC.Shared.DTO;
using System;
using Xunit;

namespace ShoppingCartCMC.Test
{
    //PW: we simple test Repository, as Controller ust Repository all calls to its Repository.

    public class ShippingControllerTest
    {
        //PW: we simple test Repository, as Controller ust Repository all calls to its Repository.

        [Fact]
        public async void Get()
        {
            //Arrange
            var repo = A.Fake<iShippingRepository>();
            var dto = A.Fake<iBillingDto>();

            // (1) cart price < 50 AUD
            //ACT
            var actionResult = await repo.GetShippingFee(40M, "AUD");

            //Assert
            decimal actual = actionResult;
            decimal expect = 0M;
            Assert.Equal(expect, actual);


            // (2) cart price >= 50 AUD
            actionResult = await repo.GetShippingFee(60M, "AUD");

            //Assert
            actual = actionResult;
            expect = 0M;
            Assert.Equal(expect, actual);

            /** *
             * Patrick: [all cases should be test below].
             * todo....
             */

            //// (3) cart price < 53 NZD, which less than AUD 50.
            ////ACT
            //actionResult = await repo.GetShippingFee(40M, "NZD");

            ////Assert
            //actual = actionResult;
            //expect = 20M;
            //Assert.Equal(expect, actual);


            //// (4) cart price < 60 NZD, which greater than AUD 50.
            //actionResult = await repo.GetShippingFee(60M, "NZD");

            ////Assert
            //actual = actionResult;
            //expect = 20M;
            //Assert.Equal(expect, actual);

            //// (3) cart price < 53 NZD, which less than AUD 50.
            ////ACT
            //actionResult = await repo.GetShippingFee(40M, "USD");

            ////Assert
            //actual = actionResult;
            //expect = 20M;
            //Assert.Equal(expect, actual);


            //// (4) cart price < 60 NZD, which greater than AUD 50.
            //actionResult = await repo.GetShippingFee(60M, "USD");

            ////Assert
            //actual = actionResult;
            //expect = 20M;
            //Assert.Equal(expect, actual);
        }
    }
}
