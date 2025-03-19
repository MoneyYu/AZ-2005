using Microsoft.EntityFrameworkCore;

namespace LightManager.Models
{
    /// <summary>
    /// 電燈管理系統的資料庫上下文類別
    /// </summary>
    public class LightDbContext : DbContext
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="options">DbContext 選項</param>
        public LightDbContext(DbContextOptions<LightDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 取得或設定電燈資料集
        /// </summary>
        public DbSet<Light> Lights { get; set; }

        /// <summary>
        /// 模型建立設定
        /// </summary>
        /// <param name="modelBuilder">模型建立器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Light>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Brightness).HasDefaultValue(100);
                entity.Property(e => e.IsOn).HasDefaultValue(false);
            });
        }
    }
}