using DataSource.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataSource
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        

        public virtual DbSet<SYSTEMUSERS> SYSTEMUSERS { get; set; } = null!;
        public virtual DbSet<TADDRESSES> TADDRESSES { get; set; } = null!;
        public virtual DbSet<TADMINLOGS> TADMINLOGS { get; set; } = null!;
        public virtual DbSet<TCCITIES> TCCITIES { get; set; } = null!;
        public virtual DbSet<TCCONDITION> TCCONDITION { get; set; } = null!;
        public virtual DbSet<TCDOCUMENTTYPE> TCDOCUMENTTYPE { get; set; } = null!;
        public virtual DbSet<TCGENDER> TCGENDER { get; set; } = null!;
        public virtual DbSet<TCHANGESLOG> TCHANGESLOG { get; set; } = null!;
        public virtual DbSet<TCMARITALSTATUS> TCMARITALSTATUS { get; set; } = null!;
        public virtual DbSet<TCOCCUPATIONS> TCOCCUPATIONS { get; set; } = null!;
        public virtual DbSet<TCONTACTS> TCONTACTS { get; set; } = null!;
        public virtual DbSet<TCPROCEDURALSTAGE> TCPROCEDURALSTAGE { get; set; } = null!;
        public virtual DbSet<TCREGISTERSTATUS> TCREGISTERSTATUS { get; set; } = null!;
        public virtual DbSet<TCSPACES> TCSPACES { get; set; } = null!;
        public virtual DbSet<TCSTAGE> TCSTAGE { get; set; } = null!;
        public virtual DbSet<TCSTATESv2> TCSTATESv2 { get; set; } = null!;
        public virtual DbSet<TCSTATUS> TCSTATUS { get; set; } = null!;
        public virtual DbSet<TCTITLES> TCTITLES { get; set; } = null!;
        public virtual DbSet<TCTYPE> TCTYPE { get; set; } = null!;
        public virtual DbSet<TDOCUMENTS> TDOCUMENTS { get; set; } = null!;
        public virtual DbSet<TEMAILVALIDATION> TEMAILVALIDATION { get; set; } = null!;
        public virtual DbSet<TFILES> TFILES { get; set; } = null!;
        public virtual DbSet<TLOGS> TLOGS { get; set; } = null!;
        public virtual DbSet<TPARAMETERS> TPARAMETERS { get; set; } = null!;
        public virtual DbSet<TPASSWORDRECOVERY> TPASSWORDRECOVERY { get; set; } = null!;
        public virtual DbSet<TPROPERTIES> TPROPERTIES { get; set; } = null!;
        public virtual DbSet<TPROPERTYREQUEST> TPROPERTYREQUEST { get; set; } = null!;
        public virtual DbSet<TRUSERPROPERTIES> TRUSERPROPERTIES { get; set; } = null!;
        public virtual DbSet<TRUSERSTATUSREGISTER> TRUSERSTATUSREGISTER { get; set; } = null!;
        public virtual DbSet<TUSERS> TUSERS { get; set; } = null!;
        public virtual DbSet<TUSERSETTINGS> TUSERSETTINGS { get; set; } = null!;
        public virtual DbSet<TUSERSINFO> TUSERSINFO { get; set; } = null!;
        public virtual DbSet<WEBDOXREQUEST> WEBDOXREQUEST { get; set; } = null!;
        #region StoredProcedure
        public virtual DbSet<SP_Get_Properties_Filter?> SP_Get_Properties_Filter { get; set; } = null;
        #endregion StoredProcedure

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region StoredProcedure
            modelBuilder.Entity<SP_Get_Properties_Filter>().HasNoKey();            
            #endregion StoredProcedure
            modelBuilder.Entity<SYSTEMUSERS>(entity =>
            {
                entity.ToTable("SYSTEMUSERS", "PRO");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FULLNAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ROLE)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.SYSTEMUSERS)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__SYSTEMUSE__IDSTA__753864A1");
            });

            modelBuilder.Entity<TADDRESSES>(entity =>
            {
                entity.ToTable("TADDRESSES", "PRO");

                entity.HasIndex(e => e.IDPROPERTY, "UQ__TADDRESS__B92DE70C8EB5C9B0")
                    .IsUnique();

                entity.Property(e => e.CREATE_AT).HasColumnType("datetime");

                entity.Property(e => e.EXTERIORNUMBER)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.INTERIORNUMBER)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.POSTCODE)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SETTLEMENT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.STREETNAME)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDCITYv2Navigation)
                    .WithMany(p => p.TADDRESSES)
                    .HasForeignKey(d => d.IDCITYv2)
                    .HasConstraintName("FK_ADDRESSES_CITIES_");

                entity.HasOne(d => d.IDPROPERTYNavigation)
                    .WithOne(p => p.TADDRESSES)
                    .HasForeignKey<TADDRESSES>(d => d.IDPROPERTY)
                    .HasConstraintName("FK__TADDRESSE__IDPRO__4865BE2A");
            });

            modelBuilder.Entity<TADMINLOGS>(entity =>
            {
                entity.ToTable("TADMINLOGS", "PRO");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TCCITIES>(entity =>
            {
                entity.ToTable("TCCITIES", "PRO");

                entity.HasIndex(e => new { e.CODE, e.CODESTATE }, "UNIQUE_CODE_CITIES")
                    .IsUnique();

                entity.Property(e => e.CODE).HasMaxLength(4);

                entity.Property(e => e.CODESTATE).HasMaxLength(3);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(100);

                entity.HasOne(d => d.CODESTATENavigation)
                    .WithMany(p => p.TCCITIES)
                    .HasPrincipalKey(p => p.CODE)
                    .HasForeignKey(d => d.CODESTATE)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STATES_CITIES");
            });

            modelBuilder.Entity<TCCONDITION>(entity =>
            {
                entity.ToTable("TCCONDITION", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.InverseIDSTATUSNavigation)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TCCONDITI__IDSTA__214BF109");
            });

            modelBuilder.Entity<TCDOCUMENTTYPE>(entity =>
            {
                entity.ToTable("TCDOCUMENTTYPE", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TCDOCUMENTTYPE)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TCDOCUMEN__IDSTA__66EA454A");
            });

            modelBuilder.Entity<TCGENDER>(entity =>
            {
                entity.ToTable("TCGENDER", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TCGENDER)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TCGENDER__IDSTAT__056ECC6A");
            });

            modelBuilder.Entity<TCHANGESLOG>(entity =>
            {
                entity.ToTable("TCHANGESLOG", "PRO");

                entity.Property(e => e.CHANGE)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CREATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IP)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.USERNAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TCMARITALSTATUS>(entity =>
            {
                entity.ToTable("TCMARITALSTATUS", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TCOCCUPATIONS>(entity =>
            {
                entity.ToTable("TCOCCUPATIONS", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TCONTACTS>(entity =>
            {
                entity.ToTable("TCONTACTS", "PRO");

                entity.Property(e => e.CELLPHONE)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CREATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FULLNAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MESSAGE)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TCPROCEDURALSTAGE>(entity =>
            {
                entity.ToTable("TCPROCEDURALSTAGE", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IDSTATUS).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TCPROCEDURALSTAGE)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TCPROCEDU__IDSTA__336AA144");
            });

            modelBuilder.Entity<TCREGISTERSTATUS>(entity =>
            {
                entity.ToTable("TCREGISTERSTATUS", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TCSPACES>(entity =>
            {
                entity.ToTable("TCSPACES", "PRO");

                entity.Property(e => e.CREATEDAT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION).HasMaxLength(30);

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TCSPACES)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK_STATUS_SPACES");
            });

            modelBuilder.Entity<TCSTAGE>(entity =>
            {
                entity.ToTable("TCSTAGE", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.DESCRPTION)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TCSTAGE)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TCSTAGE__IDSTATU__24285DB4");
            });

            modelBuilder.Entity<TCSTATESv2>(entity =>
            {
                entity.ToTable("TCSTATESv2", "PRO");

                entity.HasIndex(e => e.CODE, "UNIQUE_CODE_STATES")
                    .IsUnique();

                entity.Property(e => e.CODE).HasMaxLength(3);

                entity.Property(e => e.DESCRIPTION).HasMaxLength(100);
            });

            modelBuilder.Entity<TCSTATUS>(entity =>
            {
                entity.ToTable("TCSTATUS", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TCTITLES>(entity =>
            {
                entity.ToTable("TCTITLES", "PRO");

                entity.HasIndex(e => e.IDPROCEDURALSTAGE, "PROCEDURALSTAGE_UNIQUE_TITLES")
                    .IsUnique();

                entity.Property(e => e.CREATEDAT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION).HasMaxLength(100);

                entity.Property(e => e.UPDATEDAT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDPROCEDURALSTAGENavigation)
                    .WithOne(p => p.TCTITLES)
                    .HasForeignKey<TCTITLES>(d => d.IDPROCEDURALSTAGE)
                    .HasConstraintName("FK_PROCEDURALSTAGE_TITLES");
            });

            modelBuilder.Entity<TCTYPE>(entity =>
            {
                entity.ToTable("TCTYPE", "PRO");

                entity.Property(e => e.ID).ValueGeneratedNever();

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IDSTATUS).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TCTYPE)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TCTYPE__IDSTATUS__382F5661");
            });

            modelBuilder.Entity<TDOCUMENTS>(entity =>
            {
                entity.ToTable("TDOCUMENTS", "PRO");

                entity.Property(e => e.CREATE_AT).HasColumnType("datetime");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FILEEXTENCION)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PATH)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.TITLE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.URI)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IDDOCUMENTTYPENavigation)
                    .WithMany(p => p.TDOCUMENTS)
                    .HasForeignKey(d => d.IDDOCUMENTTYPE)
                    .HasConstraintName("FK__TDOCUMENT__IDDOC__6BAEFA67");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TDOCUMENTS)
                    .HasForeignKey(d => d.IDUSER)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TDOCUMENT__IDUSE__6ABAD62E");
            });

            modelBuilder.Entity<TEMAILVALIDATION>(entity =>
            {
                entity.ToTable("TEMAILVALIDATION", "PRO");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TOKEN)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.USED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TEMAILVALIDATION)
                    .HasForeignKey(d => d.IDUSER)
                    .HasConstraintName("FK__TEMAILVAL__IDUSE__28B808A7");
            });

            modelBuilder.Entity<TFILES>(entity =>
            {
                entity.ToTable("TFILES", "PRO");

                entity.Property(e => e.CREATE_AT).HasColumnType("datetime");

                entity.Property(e => e.DESCRIPTION).IsUnicode(false);

                entity.Property(e => e.FILEEXTENCION)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PATH).IsUnicode(false);

                entity.Property(e => e.PREVIEW).HasDefaultValueSql("((0))");

                entity.Property(e => e.TITLE).IsUnicode(false);

                entity.Property(e => e.UPDATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.URI).IsUnicode(false);

                entity.HasOne(d => d.IDPROPERTYNavigation)
                    .WithMany(p => p.TFILES)
                    .HasForeignKey(d => d.IDPROPERTY)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TFILES__IDPROPER__6225902D");

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TFILES)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TFILES__IDSTATUS__6319B466");
            });

            modelBuilder.Entity<TLOGS>(entity =>
            {
                entity.ToTable("TLOGS", "PRO");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TPARAMETERS>(entity =>
            {
                entity.ToTable("TPARAMETERS", "PRO");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GROUPNAME)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NAME)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.VALUE)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TPASSWORDRECOVERY>(entity =>
            {
                entity.ToTable("TPASSWORDRECOVERY", "PRO");

                entity.Property(e => e.CREATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EXPIRATION_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(dateadd(hour,(12),getdate()))");

                entity.Property(e => e.TOKEN)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.USED_AT).HasColumnType("datetime");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TPASSWORDRECOVERY)
                    .HasForeignKey(d => d.IDUSER)
                    .HasConstraintName("FK__TPASSWORD__IDUSE__408F9238");
            });

            modelBuilder.Entity<TPROPERTIES>(entity =>
            {
                entity.ToTable("TPROPERTIES", "PRO");

                entity.HasIndex(e => e.CREDITNUMBER, "UQ__TPROPERT__0BEE214D3EDBEC36")
                    .IsUnique();

                entity.Property(e => e.ACQUISITIONDEADLINE).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.CODE)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CREDITNUMBER)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CURRENCY)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.GUARANTEEVALUE).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MAINTENANCEFEE).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SALEPRICE).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TITLE)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TOTALDEBT).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UPDATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDBATHROOMNavigation)
                    .WithMany(p => p.TPROPERTIESIDBATHROOMNavigation)
                    .HasForeignKey(d => d.IDBATHROOM)
                    .HasConstraintName("FK_PROPERTIES_BATHROOMS");

                entity.HasOne(d => d.IDBEDROOMNavigation)
                    .WithMany(p => p.TPROPERTIESIDBEDROOMNavigation)
                    .HasForeignKey(d => d.IDBEDROOM)
                    .HasConstraintName("FK_PROPERTIES_BEDROOMS");

                entity.HasOne(d => d.IDCONDITIONNavigation)
                    .WithMany(p => p.TPROPERTIES)
                    .HasForeignKey(d => d.IDCONDITION)
                    .HasConstraintName("FK__TPROPERTI__IDCON__3EDC53F0");

                entity.HasOne(d => d.IDHALFBATHROOMNavigation)
                    .WithMany(p => p.TPROPERTIESIDHALFBATHROOMNavigation)
                    .HasForeignKey(d => d.IDHALFBATHROOM)
                    .HasConstraintName("FK_PROPERTIES_HALFBATHROOM");

                entity.HasOne(d => d.IDLEVELNavigation)
                    .WithMany(p => p.TPROPERTIESIDLEVELNavigation)
                    .HasForeignKey(d => d.IDLEVEL)
                    .HasConstraintName("FK_PROPERTIES_LEVELS");

                entity.HasOne(d => d.IDPARKINGSPACENavigation)
                    .WithMany(p => p.TPROPERTIESIDPARKINGSPACENavigation)
                    .HasForeignKey(d => d.IDPARKINGSPACE)
                    .HasConstraintName("FK_PROPERTIES_PARKINGSPACE");

                entity.HasOne(d => d.IDPROCEDURALSTAGENavigation)
                    .WithMany(p => p.TPROPERTIES)
                    .HasForeignKey(d => d.IDPROCEDURALSTAGE)
                    .HasConstraintName("FK__TPROPERTI__IDPRO__40C49C62");

                entity.HasOne(d => d.IDSTAGENavigation)
                    .WithMany(p => p.TPROPERTIES)
                    .HasForeignKey(d => d.IDSTAGE)
                    .HasConstraintName("FK__TPROPERTI__IDSTA__42ACE4D4");

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TPROPERTIES)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TPROPERTI__IDSTA__41B8C09B");

                entity.HasOne(d => d.IDTYPENavigation)
                    .WithMany(p => p.TPROPERTIES)
                    .HasForeignKey(d => d.IDTYPE)
                    .HasConstraintName("FK__TPROPERTI__IDTYP__3FD07829");
            });

            modelBuilder.Entity<TPROPERTYREQUEST>(entity =>
            {
                entity.ToTable("TPROPERTYREQUEST", "PRO");

                entity.Property(e => e.CREATEDAT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SHAREDTO).HasMaxLength(200);

                entity.Property(e => e.UPDATEDAT).HasColumnType("datetime");

                entity.HasOne(d => d.IDPROPERTYNavigation)
                    .WithMany(p => p.TPROPERTYREQUEST)
                    .HasForeignKey(d => d.IDPROPERTY)
                    .HasConstraintName("FK_PROPERTYREQUEST_PROPERTIES");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TPROPERTYREQUEST)
                    .HasForeignKey(d => d.IDUSER)
                    .HasConstraintName("FK_PROPERTYREQUEST_USERS");
            });

            modelBuilder.Entity<TRUSERPROPERTIES>(entity =>
            {
                entity.HasKey(e => new { e.IDUSER, e.IDPROPERTY })
                    .HasName("PK_USER_PROPERTY");

                entity.ToTable("TRUSERPROPERTIES", "PRO");

                entity.Property(e => e.CREATE_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AD)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDPROPERTYNavigation)
                    .WithMany(p => p.TRUSERPROPERTIES)
                    .HasForeignKey(d => d.IDPROPERTY)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TRUSERPRO__IDPRO__55BFB948");

                entity.HasOne(d => d.IDSTATUSNavigation)
                    .WithMany(p => p.TRUSERPROPERTIES)
                    .HasForeignKey(d => d.IDSTATUS)
                    .HasConstraintName("FK__TRUSERPRO__IDSTA__52E34C9D");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TRUSERPROPERTIES)
                    .HasForeignKey(d => d.IDUSER)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TRUSERPRO__IDUSE__54CB950F");
            });

            modelBuilder.Entity<TRUSERSTATUSREGISTER>(entity =>
            {
                entity.HasKey(e => new { e.IDUSER, e.IDREGISTERSTATUS })
                    .HasName("PK_USER_REGISTERSTATUS");

                entity.ToTable("TRUSERSTATUSREGISTER", "PRO");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDREGISTERSTATUSNavigation)
                    .WithMany(p => p.TRUSERSTATUSREGISTER)
                    .HasForeignKey(d => d.IDREGISTERSTATUS)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TRUSERSTA__IDREG__10E07F16");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TRUSERSTATUSREGISTER)
                    .HasForeignKey(d => d.IDUSER)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TRUSERSTA__IDUSE__11D4A34F");
            });

            modelBuilder.Entity<TUSERS>(entity =>
            {
                entity.ToTable("TUSERS", "PRO");

                entity.Property(e => e.CELLPHONE)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CELLPHONE_VALIDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CONTRACT_SIGN_AT).HasColumnType("datetime");

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.EMAIL)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EMAIL_VALIDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.IDENTITY_VALIDATED_AT).HasColumnType("datetime");

                entity.Property(e => e.LOGIN).HasDefaultValueSql("((0))");

                entity.Property(e => e.PASSWORD).IsUnicode(false);

                entity.Property(e => e.PROCESSCONTRACT).HasColumnType("datetime");

                entity.Property(e => e.TOKEN).IsUnicode(false);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TUSERSETTINGS>(entity =>
            {
                entity.ToTable("TUSERSETTINGS", "PRO");

                entity.Property(e => e.ADVERTISINGEMAIL).HasDefaultValueSql("((1))");

                entity.Property(e => e.ADVERTISINGSMS).HasDefaultValueSql("((1))");

                entity.Property(e => e.EMAILNOTIFICATION).HasDefaultValueSql("((1))");

                entity.Property(e => e.SMSNOTIFICATION).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.TUSERSETTINGS)
                    .HasForeignKey(d => d.IDUSER)
                    .HasConstraintName("FK__TUSERSETT__IDUSE__725BF7F6");
            });

            modelBuilder.Entity<TUSERSINFO>(entity =>
            {
                entity.ToTable("TUSERSINFO", "PRO");

                entity.HasIndex(e => e.IDUSER, "UQ__TUSERSIN__94F7C058980AC3FC")
                    .IsUnique();

                entity.HasIndex(e => e.IDUSER, "UQ__TUSERSIN__94F7C058D1FDA916")
                    .IsUnique();

                entity.Property(e => e.ADDRESS)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.BIRTHDAY).HasColumnType("datetime");

                entity.Property(e => e.BIRTHPLACE)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CREATED_AT).HasColumnType("datetime");

                entity.Property(e => e.CURP)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.HOMETOWN)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.LASTNAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LASTNAME2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NAMES)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NUMBERDOCUMENT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PLACEDOCUMENT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RFC)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UPDATED_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IDGENDERNavigation)
                    .WithMany(p => p.TUSERSINFO)
                    .HasForeignKey(d => d.IDGENDER)
                    .HasConstraintName("FK__TUSERSINF__IDGEN__0B27A5C0");

                entity.HasOne(d => d.IDMARITALSTATUSNavigation)
                    .WithMany(p => p.TUSERSINFO)
                    .HasForeignKey(d => d.IDMARITALSTATUS)
                    .HasConstraintName("FK__TUSERSINF__IDMAR__62E4AA3C");

                entity.HasOne(d => d.IDOCCUPATIONNavigation)
                    .WithMany(p => p.TUSERSINFO)
                    .HasForeignKey(d => d.IDOCCUPATION)
                    .HasConstraintName("FK__TUSERSINF__IDOCC__5F141958");

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithOne(p => p.TUSERSINFO)
                    .HasForeignKey<TUSERSINFO>(d => d.IDUSER)
                    .HasConstraintName("FK__TUSERSINF__IDUSE__0A338187");
            });

            modelBuilder.Entity<WEBDOXREQUEST>(entity =>
            {
                entity.ToTable("WEBDOXREQUEST", "PRO");

                entity.Property(e => e.CREATE_AT)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DESCRIPTION)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FORMWEBDOXID)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.JSON)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IDUSERNavigation)
                    .WithMany(p => p.WEBDOXREQUEST)
                    .HasForeignKey(d => d.IDUSER)
                    .HasConstraintName("FK__WEBDOXREQ__IDUSE__1A34DF26");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
