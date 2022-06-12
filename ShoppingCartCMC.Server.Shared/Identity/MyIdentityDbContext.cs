using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShoppingCartCMC.Server.Shared.Identity
{
    public class MyIdentityDbContext : IdentityDbContext<MyIdentityUser>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options) : base(options)
        {
        }
    }
}
