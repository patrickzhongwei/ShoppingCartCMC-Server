using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ShoppingCartCMC.Server.Shared.DB.Identity
{
    public partial class ShoppingCartCmcIdentityContext : DbContext
    {
        public ShoppingCartCmcIdentityContext()
        {
        }

        public ShoppingCartCmcIdentityContext(DbContextOptions<ShoppingCartCmcIdentityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiResource> ApiResources { get; set; }
        public virtual DbSet<ApiResourceClaim> ApiResourceClaims { get; set; }
        public virtual DbSet<ApiResourceProperty> ApiResourceProperties { get; set; }
        public virtual DbSet<ApiResourceScope> ApiResourceScopes { get; set; }
        public virtual DbSet<ApiResourceSecret> ApiResourceSecrets { get; set; }
        public virtual DbSet<ApiScope> ApiScopes { get; set; }
        public virtual DbSet<ApiScopeClaim> ApiScopeClaims { get; set; }
        public virtual DbSet<ApiScopeProperty> ApiScopeProperties { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientClaim> ClientClaims { get; set; }
        public virtual DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public virtual DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public virtual DbSet<ClientIdPrestriction> ClientIdPrestrictions { get; set; }
        public virtual DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public virtual DbSet<ClientProperty> ClientProperties { get; set; }
        public virtual DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public virtual DbSet<ClientScope> ClientScopes { get; set; }
        public virtual DbSet<ClientSecret> ClientSecrets { get; set; }
        public virtual DbSet<DeviceCode> DeviceCodes { get; set; }
        public virtual DbSet<IdentityResource> IdentityResources { get; set; }
        public virtual DbSet<IdentityResourceClaim> IdentityResourceClaims { get; set; }
        public virtual DbSet<IdentityResourceProperty> IdentityResourceProperties { get; set; }
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=ShoppingCartCmc.Identity;Trusted_Connection=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ApiResource>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_ApiResources_Name")
                    .IsUnique();

                entity.Property(e => e.AllowedAccessTokenSigningAlgorithms).HasMaxLength(100);

                entity.Property(e => e.Created).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NonEditable)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<ApiResourceClaim>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId, "IX_ApiResourceClaims_ApiResourceId");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiResourceClaims)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiResourceProperty>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId, "IX_ApiResourceProperties_ApiResourceId");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiResourceProperties)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiResourceScope>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId, "IX_ApiResourceScopes_ApiResourceId");

                entity.Property(e => e.Scope)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiResourceScopes)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiResourceSecret>(entity =>
            {
                entity.HasIndex(e => e.ApiResourceId, "IX_ApiResourceSecrets_ApiResourceId");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.ApiResource)
                    .WithMany(p => p.ApiResourceSecrets)
                    .HasForeignKey(d => d.ApiResourceId);
            });

            modelBuilder.Entity<ApiScope>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_ApiScopes_Name")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<ApiScopeClaim>(entity =>
            {
                entity.HasIndex(e => e.ScopeId, "IX_ApiScopeClaims_ScopeId");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Scope)
                    .WithMany(p => p.ApiScopeClaims)
                    .HasForeignKey(d => d.ScopeId);
            });

            modelBuilder.Entity<ApiScopeProperty>(entity =>
            {
                entity.HasIndex(e => e.ScopeId, "IX_ApiScopeProperties_ScopeId");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Scope)
                    .WithMany(p => p.ApiScopeProperties)
                    .HasForeignKey(d => d.ScopeId);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.BaseCurrency).HasMaxLength(50);

                entity.Property(e => e.Company).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.IsVip).HasColumnName("IsVIP");

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.ServicePlan).HasMaxLength(100);

                entity.Property(e => e.TimeCreated).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.TimeLastUpdated).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_Clients_ClientId")
                    .IsUnique();

                entity.Property(e => e.AllowedIdentityTokenSigningAlgorithms).HasMaxLength(100);

                entity.Property(e => e.BackChannelLogoutUri).HasMaxLength(2000);

                entity.Property(e => e.ClientClaimsPrefix).HasMaxLength(200);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ClientName).HasMaxLength(200);

                entity.Property(e => e.ClientUri).HasMaxLength(2000);

                entity.Property(e => e.Created).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.FrontChannelLogoutUri).HasMaxLength(2000);

                entity.Property(e => e.LogoUri).HasMaxLength(2000);

                entity.Property(e => e.NonEditable)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.PairWiseSubjectSalt).HasMaxLength(200);

                entity.Property(e => e.ProtocolType)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UserCodeType).HasMaxLength(100);
            });

            modelBuilder.Entity<ClientClaim>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientClaims_ClientId");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientClaims)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientCorsOrigin>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientCorsOrigins_ClientId");

                entity.Property(e => e.Origin)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCorsOrigins)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientGrantType>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientGrantTypes_ClientId");

                entity.Property(e => e.GrantType)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientGrantTypes)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientIdPrestriction>(entity =>
            {
                entity.ToTable("ClientIdPRestrictions");

                entity.HasIndex(e => e.ClientId, "IX_ClientIdPRestrictions_ClientId");

                entity.Property(e => e.Provider)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientIdPrestrictions)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientPostLogoutRedirectUris_ClientId");

                entity.Property(e => e.PostLogoutRedirectUri)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientPostLogoutRedirectUris)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientProperty>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientProperties_ClientId");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientProperties)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientRedirectUri>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientRedirectUris_ClientId");

                entity.Property(e => e.RedirectUri)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientRedirectUris)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientScope>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientScopes_ClientId");

                entity.Property(e => e.Scope)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientScopes)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<ClientSecret>(entity =>
            {
                entity.HasIndex(e => e.ClientId, "IX_ClientSecrets_ClientId");

                entity.Property(e => e.Created).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientSecrets)
                    .HasForeignKey(d => d.ClientId);
            });

            modelBuilder.Entity<DeviceCode>(entity =>
            {
                entity.HasKey(e => e.UserCode);

                entity.HasIndex(e => e.DeviceCode1, "IX_DeviceCodes_DeviceCode")
                    .IsUnique();

                entity.HasIndex(e => e.Expiration, "IX_DeviceCodes_Expiration");

                entity.Property(e => e.UserCode).HasMaxLength(200);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DeviceCode1)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DeviceCode");

                entity.Property(e => e.SessionId).HasMaxLength(100);

                entity.Property(e => e.SubjectId).HasMaxLength(200);
            });

            modelBuilder.Entity<IdentityResource>(entity =>
            {
                entity.HasIndex(e => e.Name, "IX_IdentityResources_Name")
                    .IsUnique();

                entity.Property(e => e.Created).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.DisplayName).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.NonEditable)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<IdentityResourceClaim>(entity =>
            {
                entity.HasIndex(e => e.IdentityResourceId, "IX_IdentityResourceClaims_IdentityResourceId");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.IdentityResource)
                    .WithMany(p => p.IdentityResourceClaims)
                    .HasForeignKey(d => d.IdentityResourceId);
            });

            modelBuilder.Entity<IdentityResourceProperty>(entity =>
            {
                entity.HasIndex(e => e.IdentityResourceId, "IX_IdentityResourceProperties_IdentityResourceId");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.HasOne(d => d.IdentityResource)
                    .WithMany(p => p.IdentityResourceProperties)
                    .HasForeignKey(d => d.IdentityResourceId);
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.HasKey(e => e.Key);

                entity.HasIndex(e => e.Expiration, "IX_PersistedGrants_Expiration");

                entity.HasIndex(e => new { e.SubjectId, e.ClientId, e.Type }, "IX_PersistedGrants_SubjectId_ClientId_Type");

                entity.HasIndex(e => new { e.SubjectId, e.SessionId, e.Type }, "IX_PersistedGrants_SubjectId_SessionId_Type");

                entity.Property(e => e.Key).HasMaxLength(200);

                entity.Property(e => e.ClientId)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.SessionId).HasMaxLength(100);

                entity.Property(e => e.SubjectId).HasMaxLength(200);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
