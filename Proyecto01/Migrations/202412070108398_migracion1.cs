namespace Proyecto01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracion1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Equipamientoes",
                c => new
                    {
                        IdEquipamiento = c.Int(nullable: false, identity: true),
                        NombreEquipamiento = c.String(nullable: false, maxLength: 50),
                        SalasReunion_IdSala = c.Int(),
                    })
                .PrimaryKey(t => t.IdEquipamiento)
                .ForeignKey("dbo.SalasReunions", t => t.SalasReunion_IdSala)
                .Index(t => t.SalasReunion_IdSala);
            
            CreateTable(
                "dbo.SalasEquipamientos",
                c => new
                    {
                        IdSala = c.Int(nullable: false),
                        IdEquipamiento = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdSala, t.IdEquipamiento })
                .ForeignKey("dbo.Equipamientoes", t => t.IdEquipamiento, cascadeDelete: true)
                .ForeignKey("dbo.SalasReunions", t => t.IdSala, cascadeDelete: true)
                .Index(t => t.IdSala)
                .Index(t => t.IdEquipamiento);
            
            CreateTable(
                "dbo.SalasReunions",
                c => new
                    {
                        IdSala = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 20),
                        Capacidad = c.Int(nullable: false),
                        Ubicacion = c.String(nullable: false, maxLength: 50),
                        HoraInicio = c.Time(nullable: false, precision: 7),
                        HoraFin = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.IdSala);
            
            CreateTable(
                "dbo.Estadisticas",
                c => new
                    {
                        IdEstadistica = c.Int(nullable: false, identity: true),
                        IdSala = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        HorasUso = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumeroReservas = c.Int(nullable: false),
                        PorcentajeOcupacion = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.IdEstadistica)
                .ForeignKey("dbo.SalasReunions", t => t.IdSala, cascadeDelete: true)
                .Index(t => t.IdSala);
            
            CreateTable(
                "dbo.Reservas",
                c => new
                    {
                        IdReserva = c.Int(nullable: false, identity: true),
                        IdSala = c.Int(nullable: false),
                        IdUsuario = c.String(nullable: false, maxLength: 128),
                        FechaReserva = c.DateTime(nullable: false),
                        HoraInicio = c.Time(nullable: false, precision: 7),
                        HoraFin = c.Time(nullable: false, precision: 7),
                        Aprobacion = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.IdReserva)
                .ForeignKey("dbo.SalasReunions", t => t.IdSala, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdSala)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SalasEquipamientos", "IdSala", "dbo.SalasReunions");
            DropForeignKey("dbo.Reservas", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservas", "IdSala", "dbo.SalasReunions");
            DropForeignKey("dbo.Estadisticas", "IdSala", "dbo.SalasReunions");
            DropForeignKey("dbo.Equipamientoes", "SalasReunion_IdSala", "dbo.SalasReunions");
            DropForeignKey("dbo.SalasEquipamientos", "IdEquipamiento", "dbo.Equipamientoes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Reservas", new[] { "IdUsuario" });
            DropIndex("dbo.Reservas", new[] { "IdSala" });
            DropIndex("dbo.Estadisticas", new[] { "IdSala" });
            DropIndex("dbo.SalasEquipamientos", new[] { "IdEquipamiento" });
            DropIndex("dbo.SalasEquipamientos", new[] { "IdSala" });
            DropIndex("dbo.Equipamientoes", new[] { "SalasReunion_IdSala" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Reservas");
            DropTable("dbo.Estadisticas");
            DropTable("dbo.SalasReunions");
            DropTable("dbo.SalasEquipamientos");
            DropTable("dbo.Equipamientoes");
        }
    }
}
