using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TestMagnetron.Models;
using TestMagnetron.Models.Views;

namespace TestMagnetron
{
    public partial class TestMagnetronContext : DbContext
    {
        public TestMagnetronContext()
        {
        }

        public TestMagnetronContext(DbContextOptions<TestMagnetronContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FactDetalle> FactDetalles { get; set; } = null!;
        public virtual DbSet<FactEncabezado> FactEncabezados { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<TiposDocumento> TiposDocumentos { get; set; } = null!;
        public virtual DbSet<VistaMargenGananciaPorProducto> VistaMargenGananciaPorProductos { get; set; } = null!;
        public virtual DbSet<VistaPersonaProductoMasCaro> VistaPersonaProductoMasCaros { get; set; } = null!;
        public virtual DbSet<VistaProductosPorCantidadFacturadum> VistaProductosPorCantidadFacturada { get; set; } = null!;
        public virtual DbSet<VistaTotalFacturado> VistaTotalFacturados { get; set; } = null!;
        public virtual DbSet<VistaUtilidadPorProducto> VistaUtilidadPorProductos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=127.0.0.1; Database=TestMagnetron; User=Sa; Password=140815Matias.;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactDetalle>(entity =>
            {
                entity.HasKey(e => e.FdetId)
                    .HasName("PK__Fact_Det__1F4F24C0E27B2CD3");

                entity.ToTable("Fact_Detalle");

                entity.Property(e => e.FdetId).HasColumnName("FDet_ID");

                entity.Property(e => e.FdetCantidad).HasColumnName("FDet_Cantidad");

                entity.Property(e => e.FdetFencId).HasColumnName("FDet_FEnc_ID");

                entity.Property(e => e.FdetLinea)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FDet_Linea");

                entity.Property(e => e.FdetProdId).HasColumnName("FDet_Prod_ID");

                entity.HasOne(d => d.FdetFenc)
                    .WithMany(p => p.FactDetalles)
                    .HasForeignKey(d => d.FdetFencId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Fact_Deta__FDet___4222D4EF");

                entity.HasOne(d => d.FdetProd)
                    .WithMany(p => p.FactDetalles)
                    .HasForeignKey(d => d.FdetProdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Fact_Deta__FDet___412EB0B6");
            });

            modelBuilder.Entity<FactEncabezado>(entity =>
            {
                entity.HasKey(e => e.FencId)
                    .HasName("PK__Fact_Enc__7421F085C78F6D54");

                entity.ToTable("Fact_Encabezado");

                entity.Property(e => e.FencId).HasColumnName("FEnc_ID");

                entity.Property(e => e.FencFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("FEnc_Fecha");

                entity.Property(e => e.FencNumero)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FEnc_Numero");

                entity.Property(e => e.FencPerId).HasColumnName("FEnc_Per_ID");

                entity.HasOne(d => d.FencPer)
                    .WithMany(p => p.FactEncabezados)
                    .HasForeignKey(d => d.FencPerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Fact_Enca__FEnc___3E52440B");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.PerId)
                    .HasName("PK__Persona__2705F960745B98DA");

                entity.ToTable("Persona");

                entity.Property(e => e.PerId).HasColumnName("Per_ID");

                entity.Property(e => e.PerApellido)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Per_Apellido");

                entity.Property(e => e.PerDocumento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Per_Documento");

                entity.Property(e => e.PerNombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Per_Nombre");

                entity.Property(e => e.PerTipoDocumento).HasColumnName("Per_TipoDocumento");

                entity.HasOne(d => d.PerTipoDocumentoNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.PerTipoDocumento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Persona__Per_Tip__398D8EEE");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.ProdId)
                    .HasName("PK__Producto__C55BDFF3DFFC18A3");

                entity.ToTable("Producto");

                entity.Property(e => e.ProdId).HasColumnName("Prod_ID");

                entity.Property(e => e.ProdCosto)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Prod_Costo");

                entity.Property(e => e.ProdDescripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Prod_Descripcion");

                entity.Property(e => e.ProdNombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Prod_Nombre");

                entity.Property(e => e.ProdPrecio)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Prod_Precio");

                entity.Property(e => e.ProdUm)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Prod_UM");
            });

            modelBuilder.Entity<TiposDocumento>(entity =>
            {
                entity.ToTable("TiposDocumento");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VistaMargenGananciaPorProducto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vista_MargenGananciaPorProducto");

                entity.Property(e => e.MargenGanancia).HasColumnType("decimal(38, 16)");

                entity.Property(e => e.ProdId).HasColumnName("Prod_ID");

                entity.Property(e => e.ProdNombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Prod_Nombre");
            });

            modelBuilder.Entity<VistaPersonaProductoMasCaro>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vista_PersonaProductoMasCaro");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(101)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PerDocumento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Per_Documento");

                entity.Property(e => e.PerId).HasColumnName("Per_ID");

                entity.Property(e => e.PrecioProducto).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Producto)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VistaProductosPorCantidadFacturadum>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vista_ProductosPorCantidadFacturada");

                entity.Property(e => e.ProdId).HasColumnName("Prod_ID");

                entity.Property(e => e.ProdNombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Prod_Nombre");
            });

            modelBuilder.Entity<VistaTotalFacturado>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vista_TotalFacturado");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(101)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PerDocumento)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Per_Documento");

                entity.Property(e => e.PerId).HasColumnName("Per_ID");

                entity.Property(e => e.TotalFacturado).HasColumnType("decimal(38, 2)");
            });

            modelBuilder.Entity<VistaUtilidadPorProducto>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vista_UtilidadPorProducto");

                entity.Property(e => e.ProdId).HasColumnName("Prod_ID");

                entity.Property(e => e.ProdNombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Prod_Nombre");

                entity.Property(e => e.UtilidadGenerada).HasColumnType("decimal(38, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
